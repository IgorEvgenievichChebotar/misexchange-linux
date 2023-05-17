using ru.novolabs.ExchangeDTOs;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ru.novolabs.MisExchange
{
    public static class CreateRequest3Adapter
    {
        public static CreateRequest3Request MakeRequest(ru.novolabs.ExchangeDTOs.Request requestDTO, int requestLisId)
        {
            var request = new CreateRequest3Request();
            //request.Id = requestLisId;
            request.Request = new ObjectRef(requestLisId);
            var errors = new List<String>();

            //Поля
            ProcessObject(request, requestDTO, errors);


            //Пользовательские поля

            PrepareUserValues(request.UserValues);


            //Исследования
            //Специфика запроса create-request-xxx такова, что профили должны быть разбиты на составные части с ссылкой на них. 
            //Пакеты просто преобразуются в содержащиеся исследования.
            List<CreateRequest3SampleInfo> newSamples = new List<CreateRequest3SampleInfo>();

            foreach (CreateRequest3SampleInfo sample in request.Samples)
            {
                foreach (CreateRequest3TargetInfo target in sample.Targets)
                {
                    TargetDictionaryItem dictTarget = (TargetDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.Target, target.Target.Id];
                    if (dictTarget != null)
                    {
                        //Если исследование является профилем
                        if (dictTarget.IsProfile())
                        {
                            //То распределяем его пробы
                            foreach (ProfileSample profSample in dictTarget.Samples)
                            {
                                CreateRequest3SampleInfo sampleX = null;

                                sampleX = GetSample(newSamples, sample, request, profSample);
                                foreach (TargetDictionaryItem subtarget in profSample.Targets)
                                {
                                    CreateRequest3TargetInfo targetInfo = new CreateRequest3TargetInfo();
                                    targetInfo.Target = subtarget;
                                    targetInfo.Tests = subtarget.Tests;
                                    targetInfo.Parents.Add(dictTarget);
                                    if (!sampleX.Targets.Contains(targetInfo))
                                    {
                                        sampleX.Targets.Add(targetInfo);
                                    }
                                }
                                if (!newSamples.Contains(sampleX))
                                    newSamples.Add(sampleX);

                            }
                        }
                        else
                            //Если пакет, то просто раскурочиваем из него составные исследования и переносим в текущую пробу вместо него
                            if (dictTarget.IsGroup())
                            {

                                CreateRequest3SampleInfo sampleX = null;
                                sampleX = GetSample(newSamples, sample, request);

                                foreach (TargetDictionaryItem subtarget in dictTarget.Targets)
                                {
                                    CreateRequest3TargetInfo targetX = new CreateRequest3TargetInfo();
                                    targetX.Target = subtarget;
                                    targetX.Tests = subtarget.Tests;
                                    if (!sampleX.Targets.Contains(targetX))
                                        sampleX.Targets.Add(targetX);
                                }
                                if (!newSamples.Contains(sampleX))
                                    newSamples.Add(sampleX);
                                //sample.Targets.Remove(target);
                            }
                            else
                                if (dictTarget.IsSimple())
                                {
                                    CreateRequest3SampleInfo sampleX = null;
                                    sampleX = GetSample(newSamples, sample, request);

                                    CreateRequest3TargetInfo targetX = new CreateRequest3TargetInfo();
                                    targetX.Target = dictTarget;
                                    targetX.Tests = dictTarget.Tests;

                                    if (!sampleX.Targets.Contains(targetX))
                                        sampleX.Targets.Add(targetX);

                                    if (!newSamples.Contains(sampleX))
                                        newSamples.Add(sampleX);
                                }
                    }

                    if (target.Priority == 1)
                        sample.Priority = 1;
                }
                if (sample.Priority == 1)
                    request.Cito = true;
            }
            request.Samples = newSamples;


            //Регистрационная форма
            RequestFormDictionaryItem regForm = null;
            Object regFormObj = ProgramContext.Settings["registrationFormCode"];

            if (null != regFormObj)
                regForm = (RequestFormDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.RequestForm, (string)regFormObj, true);

            if (null == regForm)
                errors.Add("Неверно указанна регистрационная форма");

            request.RequestForm = regForm;


            if (errors.Count > 0)
                throw new CustomDataCheckException(errors);

            SuperCore.LimsBusinessObjects.Patient patient = requestDTO.Patient.ToLimsPatient(errors);
            if (ProgramContext.LisCommunicator.ServerOptions.GetServerOption(ConstServerOptions.EDIT_PATIENT_FROM_REQUEST) == null || ProgramContext.LisCommunicator.ServerOptions.GetServerOption(ConstServerOptions.EDIT_PATIENT_FROM_REQUEST).Value != "true")
            {


                List<SuperCore.LimsBusinessObjects.Patient> foundPatients = ProgramContext.LisCommunicator.PatientsByCode(patient.Code, null);
                if (foundPatients.Count > 1)
                    // Это сообщение об ошибке увидят партнёры из МИС. Причина - несколько пациентов с указанным кодом в нашей БД. Ошибка критическая и лечить её нужно SQL-скриптами
                    throw new Exception("Некорректный код пациента. Обратитесь к разработчикам");
                else if (foundPatients.Count == 1)
                {
                    // В этом случае произойдёт редактирование существующего пациента
                    patient.Id = foundPatients.First().Id;
                    if (patient.PatientCards != null && patient.PatientCards.Count > 0)
                    {
                        ru.novolabs.SuperCore.LimsBusinessObjects.PatientCard card = foundPatients.First().PatientCards.Find(x => x.CardNr == patient.PatientCards[0].CardNr);
                        if (card != null)
                        {
                            patient.PatientCards[0].Id = card.Id;
                            request.PatientCard.Id = card.Id;

                        }
                        List<PatientCardId> patientCardIds;
                        ProgramContext.LisCommunicator.PatientSave(patient, out patientCardIds, null);
                        if (card == null && patientCardIds.Count > 0)
                        {
                            request.PatientCard.Id = patientCardIds[0].Id;
                            patient.PatientCards[0].Id = patientCardIds[0].Id;
                        }
                        //patient.PatientCards[0].Id = card.Id;
                    }

                }
                else
                {
                    List<PatientCardId> patientCardIds;
                    patient.Id = ProgramContext.LisCommunicator.PatientSave(patient, out patientCardIds, null);
                    if (patient.PatientCards != null && patient.PatientCards.Count > 0)
                    {
                        if (patientCardIds != null && patientCardIds.Count > 0)
                            request.PatientCard.Id = patientCardIds[0].Id;
                        //patient.PatientCards[0].Id = patientCardIds[0].Id;
                    }
                }
            }

            
            request.Patient = new ObjectRef(patient.Id);

            return request;
        }

        private static void PrepareUserValues(List<UserValue> userValues)
        {
            foreach (UserValue userValue in userValues)
            {
                UserFieldDictionaryItem userField = (UserFieldDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.UserField, userValue.UserField.Id];
                if (userField != null)
                {
                    UserDirectoryDictionaryItem userDictionary = userField.UserDirectory != null ? (UserDirectoryDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.UserDirectory, userField.UserDirectory.Id] : null;
                    switch (userField.FieldType)
                    {
                        case (int)UserFieldTypes.ENUMERATION:

                            if (userDictionary != null)
                            {
                                UserDictionaryValue dictionaryItem = (UserDictionaryValue)userDictionary.FindValueByName(userValue.Value);
                                if (dictionaryItem != null)
                                {
                                    userValue.Reference.Id = dictionaryItem.Id;
                                }
                            }
                            break;
                        case (int)UserFieldTypes.SET:
                            if (userDictionary != null)
                            {
                                String[] valueSet = userValue.Value.Trim().Split(new Char[] { ',' });
                                foreach (String value in valueSet)
                                {
                                    if (value.Trim() != "")
                                    {
                                        var dictionaryItem = (UserDictionaryValue)userDictionary.FindValueByName(value);
                                        if (dictionaryItem != null)
                                        {
                                            ObjectRef reference = new ObjectRef();
                                            reference.Id = dictionaryItem.Id;
                                            userValue.Values.Add(reference);
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            userValues.RemoveAll(uv => uv.UserField.Id == 0);
        }

        private static SuperCore.LimsBusinessObjects.Patient ToLimsPatient(this ExchangeDTOs.Patient patientDTO, List<String> errors)
        {
            SuperCore.LimsBusinessObjects.Patient limsPatient = new SuperCore.LimsBusinessObjects.Patient();
            
            limsPatient.Code = patientDTO.Code;
            limsPatient.FirstName = patientDTO.FirstName;
            limsPatient.LastName = patientDTO.LastName;
            limsPatient.MiddleName = patientDTO.MiddleName;
            limsPatient.BirthDay = (patientDTO.BirthDay > 0) ? (int?)patientDTO.BirthDay : null;
            limsPatient.BirthMonth = (patientDTO.BirthMonth > 0) ? (int?)patientDTO.BirthMonth : null;
            limsPatient.BirthYear = (patientDTO.BirthYear > 0) ? (int?)patientDTO.BirthYear : null;
            limsPatient.Sex = ProgramContext.LisCommunicator.getSexById(patientDTO.Sex);
            limsPatient.Country = patientDTO.Country;
            limsPatient.City = patientDTO.City;
            limsPatient.Street = patientDTO.Street;
            limsPatient.Building = patientDTO.Building;
            limsPatient.Flat = patientDTO.Flat;
            limsPatient.InsuranceCompany = patientDTO.InsuranceCompany;
            limsPatient.PolicySeries = patientDTO.PolicySeries;
            limsPatient.PolicyNumber = patientDTO.PolicyNumber;

            foreach (UserField userField in patientDTO.UserFields)
                limsPatient.UserValues.Add(new UserValue() { Name = userField.Name, Code = userField.Code, Value = userField.Value });

            limsPatient.PatientCards.Add(new SuperCore.LimsBusinessObjects.PatientCard() { CardNr = patientDTO.PatientCard.CardNr });
            PrepareUserValues(limsPatient.UserValues);
            return limsPatient;
        }

        private static CreateRequest3SampleInfo GetSample(List<CreateRequest3SampleInfo> Samples, CreateRequest3SampleInfo searchedSample, CreateRequest3Request request)
        {
            CreateRequest3SampleInfo sampleX = null;
            foreach (CreateRequest3SampleInfo sample in Samples)
            {
                if (sample.Biomaterial.Id == searchedSample.Biomaterial.Id)
                {
                    if (sample.InternalNr == searchedSample.InternalNr)
                    {
                        sampleX = sample;
                        break;
                    }
                }
            }
            if (sampleX == null)
            {
                if (searchedSample.InternalNr != null && searchedSample.InternalNr != "")
                    sampleX = new CreateRequest3SampleInfo(searchedSample.InternalNr);
                else
                    sampleX = new CreateRequest3SampleInfo(request.InternalNr);
                sampleX.Biomaterial = searchedSample.Biomaterial;
            }
            return sampleX;
        }

        private static CreateRequest3SampleInfo GetSample(List<CreateRequest3SampleInfo> Samples, CreateRequest3SampleInfo searchedSample, CreateRequest3Request request, ProfileSample profileSample)
        {
            CreateRequest3SampleInfo sampleX = null;
            foreach (CreateRequest3SampleInfo sample in Samples)
            {
                if (sample.Biomaterial.Id == profileSample.Biomaterial.Id)
                {
                    if (sample.InternalNr == searchedSample.InternalNr)
                    {
                        sampleX = sample;
                        break;
                    }
                }
            }
            if (sampleX == null)
            {
                if (searchedSample.InternalNr != null && searchedSample.InternalNr != "")
                    sampleX = new CreateRequest3SampleInfo(searchedSample.InternalNr);
                else
                    sampleX = new CreateRequest3SampleInfo(request.InternalNr);
                sampleX.Biomaterial = profileSample.Biomaterial;
            }
            return sampleX;
        }

        private static void ProcessObject(Object dest, Object source, List<String> errors)
        {

            foreach (PropertyInfo propInfo in dest.GetType().GetProperties())
            {
                Object[] attrs = propInfo.GetCustomAttributes(typeof(DTOv2), false);
                if (attrs.Length != 1)
                    continue;
                DTOv2 dto_2 = (DTOv2)attrs[0];

                Type pType = propInfo.PropertyType;

                if (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    pType = pType.GetGenericArguments()[0];



                if (!String.IsNullOrEmpty(dto_2.DictionaryName))
                {
                    ProcessDirectory(dest, propInfo, source, dto_2, errors);
                    continue;
                }

                Object value = GetoPropValue(source, dto_2.Field, errors);
                if (null == value) continue;

                if ((null != value.GetType().GetInterface("IList")) && (null != pType.GetInterface("IList")))
                {
                    ProcessList(propInfo.GetValue(dest, null), value, errors);
                    continue;
                }

                if (!pType.Equals(value.GetType()))
                {
                    if (pType.IsSubclassOf(typeof(DictionaryItem)))
                    {
                        if (value.GetType() == typeof(int) || value.GetType() == typeof(int?))
                        {
                            int id = (int)value;
                            DictionaryItem item = (DictionaryItem)Activator.CreateInstance(pType);
                            item.Id = id;
                            propInfo.SetValue(dest, item, null);
                            continue;
                        }
                    }
                    else
                    {
                        errors.Add(String.Format("Свойство {0} объекта {1} имеет тип {2} и не совпадает по типу с {3}", propInfo.Name,
                            source.GetType().Name, pType.Name, value.GetType().Name));
                        continue;
                    }
                }

                if (pType.IsClass && (propInfo.GetType().Equals(typeof(String))))
                {
                    Object obj = propInfo.GetValue(dest, null);
                    if (null == obj)
                    {
                        errors.Add(String.Format("Свойство {0} объекта {1} должно быть инициализированно", propInfo.Name, dest.GetType().Name));
                        continue;
                    }
                }

                propInfo.SetValue(dest, value, null);
            }
        }

        private static void ProcessDirectory(Object dest, PropertyInfo propInfo, Object source, DTOv2 dto_2, List<string> errors)
        {
            Object dict = ProgramContext.Dictionaries.GetDictionary(dto_2.DictionaryName);
            if (null == dict)
            {
                 errors.Add(String.Format("Справочник  {0} не найден", dto_2.DictionaryName));
                 return;
            }


            Object code = GetoPropValue(source, dto_2.CodeField, errors);
            Object name = GetoPropValue(source, dto_2.NameField, errors);
            bool mandatory = propInfo.GetCustomAttributes(typeof(Mandatory), true).Length != 0;

            if (String.IsNullOrEmpty((String)code) && String.IsNullOrEmpty((String)name) && !mandatory) return;

            if ((null == code) || !code.GetType().Equals(typeof(String))  || String.IsNullOrEmpty((string)code))
            {
               errors.Add(String.Format("Не указан код справочника в поле  [{0}] ", dto_2.CodeField));
               return;
            }

            if ((null != name) && !name.GetType().Equals(typeof(String)))
            {
               errors.Add(String.Format("Поле  [{0}]  не является строкой", dto_2.NameField));
               return;
            }



            DictionaryItem item = null;
            item = GetDictionaryItemWithParentFiltration(dest, source, dto_2, (String)code, errors);

            if ((null == item))
            {
                if (!dto_2.CanCreate)
                {
                    errors.Add(String.Format("Элемент с кодом [{0}] не найден в справочнике {1}", code, dto_2.DictionaryName));
                    return;
                }
            }

            if (propInfo.PropertyType.Equals(typeof(ObjectRef)))
            {
                propInfo.SetValue(dest, new ObjectRef(item.Id), null);
            }
            else
            {
                propInfo.SetValue(dest, item, null);
            }


        }

        private static DictionaryItem GetDictionaryItemWithParentFiltration(Object dest, Object source, DTOv2 dto_2, String code, List<string> errors)
        {
            
            DictionaryItem res = null;
            DictionaryItem parent = null;
            if (dto_2.ParentName != null)
            {
                //Получаем свойство родительского объекта из коренного
                PropertyInfo parentProp = dest.GetType().GetProperty(dto_2.ParentName);
                //Ищем атрибут dto в этом свойстве
                Object[] attrs = parentProp.GetCustomAttributes(typeof(DTOv2), false);
                DTOv2 dto = (DTOv2)attrs[0];
                Object parentValue = source.GetType().GetProperty(dto.CodeField).GetValue(source, null);
                String parentCode = "";
                if (parentValue != null)
                    parentCode = parentValue.ToString();
                Type parentType = dest.GetType().GetProperty(dto_2.ParentName).PropertyType;
                
                //Если и у этого родителя есть родитель - уходим в рекурсию
                if (!String.IsNullOrEmpty(dto.ParentName))
                {
                    parent = GetDictionaryItemWithParentFiltration(dest, source, dto, parentCode, errors);
                }
                else
                {
                    parent = (DictionaryItem)ProgramContext.Dictionaries[dto.DictionaryName, parentCode];
                }

                if (parent != null)
                {
                    IList items = (IList)(ProgramContext.Dictionaries[dto_2.DictionaryName].DictionaryElements);
                    foreach (Object it in items)
                    {
                        Boolean stop = false;
                        if (((DictionaryItem)it).Code == (String)code)
                        {
                            foreach (PropertyInfo prop in it.GetType().GetProperties())
                            {
                                if (prop.PropertyType == parentType)
                                {
                                    if (prop.GetValue(it, null) != null)
                                        if (parent.Code == ((DictionaryItem)prop.GetValue(it, null)).Code)
                                        {
                                            res = (DictionaryItem)it;
                                            stop = true;
                                            break;
                                        }
                                }
                                else
                                    if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)) && prop.PropertyType.GetGenericArguments()[0] == parentType)
                                    {
                                        IList list = (IList)prop.GetValue(it, null);
                                        foreach (Object element in list)
                                        {
                                            if (((DictionaryItem)element).Code == parent.Code)
                                            {
                                                res = (DictionaryItem)it;
                                                stop = true;
                                                break;
                                            }
                                        }
                                        if (stop == true) break;
                                    }
                            }
                        }
                        if (stop) break;
                    }
                }
                else
                {
                    res = (DictionaryItem)ProgramContext.Dictionaries[dto_2.DictionaryName, code];
                }
            }
            else
                res = (DictionaryItem)ProgramContext.Dictionaries[dto_2.DictionaryName, code];

            if (res == null)
            {
                if (!dto_2.CanCreate)
                {
                    errors.Add(String.Format("Элемент с кодом [{0}] не найден в справочнике {1}", code, dto_2.DictionaryName));
                    return null;
                }

                String name = GetoPropValue(source, dto_2.NameField, errors).ToString();
                if (String.IsNullOrEmpty((string)code) || string.IsNullOrEmpty((string)name))
                {
                    errors.Add(String.Format("Невозможно создать элемент справочника [{0}] не указан код или имя", dto_2.DictionaryName));
                    return null;
                }

                try
                {
                    if (parent != null)
                    {
                        PropertyInfo parentProp = dest.GetType().GetProperty(dto_2.ParentName);
                        Type parentType = parentProp.PropertyType;
                        Type objectType = ProgramContext.Dictionaries[dto_2.DictionaryName].DictionaryElements.GetType().GetGenericArguments()[0];
                        res = ProgramContext.Dictionaries.CreateItem(dto_2.DictionaryName, code, name,
                            objectType,
                            parentType, parent, ProgramContext.LisCommunicator.LimsUserSession);

                    }
                    else
                    {
                        res = ProgramContext.Dictionaries.CreateItem(dto_2.DictionaryName, code, name, ProgramContext.LisCommunicator.LimsUserSession);
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(String.Format("Невозможно создать элемент справочника: [{0}] ", ex.Message));
                    return null;
                }
            }
            return res;
        }

        private static void ProcessList(Object objDestination, Object objSource, List<string> errors)
        {
            IList dest = (IList)objDestination;
            IList source = (IList)objSource;
            Type elementType = null;

            
            if (dest.GetType().GetGenericArguments().Length != 0)
                elementType = dest.GetType().GetGenericArguments()[0];
            else
                elementType = dest.GetType().BaseType.GetGenericArguments()[0];

            dest.Clear();
            foreach (Object obj in source)
            {
                Object newObject;
                if (elementType != typeof(String))
                {
                    newObject = Activator.CreateInstance(elementType);
                }
                else
                {
                    newObject = null;
                }

                dest.Add(newObject);
                ProcessObject(newObject, obj, errors);
            }
        }

        private static Object GetoPropValue(Object requestDTO, String refPropName,  List<String> errors)
        {
            if (null == requestDTO) return null;
            if (String.IsNullOrEmpty(refPropName)) return null;
            
            if (refPropName.Contains("."))
            {
                String subObjName = refPropName.Substring(0, refPropName.IndexOf('.'));
                refPropName = refPropName.Substring(refPropName.IndexOf('.') + 1);
                PropertyInfo subProp = requestDTO.GetType().GetProperty(subObjName);
                if (null == subProp)
                {
                    errors.Add(String.Format("Объект {0} не содержит свойства {1}", requestDTO.GetType().Name, subObjName));
                    return null;
                }
                if (! subProp.PropertyType.IsClass)
                {
                    errors.Add(String.Format("Свойство {0} объекта {1} не является классом", subObjName, requestDTO.GetType().Name));
                    return null;
                }
                return GetoPropValue(subProp.GetValue(requestDTO, null), refPropName, errors);
            }
            else
            {
                PropertyInfo propInfo = requestDTO.GetType().GetProperty(refPropName);
                if (null == propInfo)
                {
                    errors.Add(String.Format("Объект {0} не содержит свойства {1}", requestDTO.GetType().Name, refPropName));
                    return null;
                }
                return propInfo.GetValue(requestDTO, null);
            }
        }
    }
}
