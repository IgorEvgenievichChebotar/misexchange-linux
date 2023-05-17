using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.MisExchangeService.Classes
{
    public static class RequestPrepareHelper
    {
        private static void PreparePriority(ExternalRequest request)
        {
            foreach (ExternalRequestSample sample in request.Samples)
            {
                foreach (ExternalRequestTarget target in sample.Targets)
                {
                    if (target.Priority == ExternalPriorities.Priority_High)
                    {
                        sample.Priority = target.Priority;
                    }
                }
                if (sample.Priority == ExternalPriorities.Priority_High)
                {
                    request.Priority = LisRequestPriorities.LIS_PRIORITY_HIGH;
                }
            }
        }
        private static void PrepareBarcodes(ExternalRequest request)
        {
            foreach (ExternalRequestSample sample in request.Samples)
            {
                bool? setting = (bool?)ProgramContext.Settings["useUniqueSampleBarcoding", false];
                bool useUniqueSampleBarcoding = setting != null ? setting.Value : false;

                if ((!useUniqueSampleBarcoding) && (String.IsNullOrEmpty(sample.Barcode)))
                {
                    sample.Barcode = request.RequestCode;
                }
            }
        }

        private static void PrepareTargets(ExternalRequest request)
        {
            foreach (ExternalRequestSample sample in request.Samples)
            {
                foreach (ExternalRequestTarget target in sample.Targets)
                {
                    var trg = ProgramContext.Dictionaries[LimsDictionaryNames.Target, target.Code] as TargetDictionaryItem;
                    if (trg != null)
                        target.Target = new ObjectRef(trg.Id);
                    else
                    {
                        ErrorMessage err = new ErrorMessage();
                        err.Message = String.Format("Исследование не найдено в справочнике по коду [{0}]", target.Code);
                        request.Warnings.Add(err);
                        // Для того, чтобы умный сервер ЛИС не назначил-таки исследование по коду, несмотря на отсутствие Id
                        // (Ситуация возможна только когда кэш справочников не синхронизирован с ЛИС [устарел])
                        target.Code = String.Empty;
                    }
                }
            }
        }

        private static void PrepareBiomaterials(ExternalRequest request)
        {
            foreach (ExternalRequestSample sample in request.Samples)
            {
                if (String.IsNullOrEmpty(sample.BiomaterialCode))
                {
                    sample.Biomaterial = null;
                    continue;
                }

                var biomaterial = ProgramContext.Dictionaries[LimsDictionaryNames.Biomaterial, sample.BiomaterialCode] as BiomaterialDictionaryItem;
                if (biomaterial != null)
                    sample.Biomaterial = new ObjectRef(biomaterial.Id);
                else
                {
                    ErrorMessage err = new ErrorMessage();
                    err.Message = String.Format("Биоматериал не найден в справочнике по коду [{0}]", sample.BiomaterialCode);
                    request.Warnings.Add(err);
                    // Для того, чтобы умный сервер ЛИС не проставил биоматериал, несмотря на отсутствие Id
                    // (Ситуация возможна в случаях, когда кэш справочников не синхронизирован с ЛИС [устарел] или если в XML заявки указан несуществующий код биоматериала)
                    sample.BiomaterialCode = String.Empty;
                }
            }
        }

        private static void PrepareRequestForm(ExternalRequest request)
        {
            string defaultRegistrationFormCode = (String)ProgramContext.Settings["defaultRegistrationFormCode", false];
            // Если RegistrationFormCode в XML заявки отсутствует или эквивалентно пустой строке, и при этом
            // в настройках задан код рег.формы по-умолчанию, то подставляем код по-умолчанию
            if (String.IsNullOrEmpty(request.RegistrationFormCode) && (!String.IsNullOrEmpty(defaultRegistrationFormCode)))
                request.RegistrationFormCode = defaultRegistrationFormCode;

            if ((request.RegistrationFormCode != null) && (!request.RegistrationFormCode.Equals(String.Empty)))
            {
                var requestForm = ProgramContext.Dictionaries[LimsDictionaryNames.RequestForm, request.RegistrationFormCode] as RequestFormDictionaryItem;
                if (requestForm != null)
                    request.RequestForm = new ObjectRef(requestForm.Id);
                else
                    throw new Exception(String.Format("Регистрационная форма не найдена в справочнике по коду [{0}]. Невозможно продолжить создание заявки", request.RegistrationFormCode));
            }
            else
                throw new Exception("Не указана регистрационная форма. Невозможно продолжить создание заявки");

        }

        private static void PrepareSampleDeliveryDate(ExternalRequest request)
        {
            bool? setting = (bool?)ProgramContext.Settings["resetSampleDeliveryDate", false];
            bool resetSampleDeliveryDate = setting != null ? setting.Value : false;

            if (resetSampleDeliveryDate)
            {
                request.SampleDeliveryDate = null;
            }
            else
            {
                // Если МИС не указала дату доставки биоматериала в лабораторию
                DateTime? defaultSampleDeliveryDate = (DateTime?)ProgramContext.Settings["defaultSampleDeliveryDate", false];

                if (request.SampleDeliveryDate == null)
                {
                    if (defaultSampleDeliveryDate == null)
                        request.SampleDeliveryDate = DateTime.Now; // Дату доставки биоматериала делаем равной дате обработки файла заявки 
                    else
                        request.SampleDeliveryDate = defaultSampleDeliveryDate; // Дату доставки биоматериала по умолчанию берём из файла настроек
                }
            }
        }

        public static void PrepareRequest(this ExternalRequest request)
        {
            CheckRequest(request);
            PreparePriority(request);
            PrepareBarcodes(request);
            PrepareTargets(request);
            PrepareBiomaterials(request);
            PrepareRequestForm(request);
            PrepareSampleDeliveryDate(request);

            // Если в файле с заявкой не указаны значения обязательных пользовательских полей, добавляем значения по-умолчанию из настроек
            var defaultUserFieldValues = (List<DefaultUserFieldValue>)ProgramContext.Settings["defaultUserFieldValues", false] ?? new List<DefaultUserFieldValue>();
            foreach (var duv in defaultUserFieldValues)
            {
                List<UserValue> userValues = null;
                switch (duv.UserObjectType)
                {
                    case UserObjectTypes.Request:
                        userValues = request.UserValues;
                        break;
                    case UserObjectTypes.Patient:
                        userValues = request.Patient.UserValues;
                        break;
                    case UserObjectTypes.PatientCard:
                        userValues = request.Patient.PatientCard.UserValues;
                        break;
                    default:
                        /* nop */
                        break;
                }

                if (!userValues.Exists(uv => uv.Code == duv.UserFieldCode))
                    userValues.Add(new UserValue() { Code = duv.UserFieldCode, Value = duv.DefaultValue });
            }

            UserFieldHelperOld.PrepareUserFieldsValuesByCodes(request.Patient.UserValues, request.Warnings);
            UserFieldHelperOld.PrepareUserFieldsValuesByCodes(request.Patient.PatientCard.UserValues, request.Warnings);
            UserFieldHelperOld.PrepareUserFieldsValuesByCodes(request.UserValues, request.Warnings);
        }

        private static void AddError(String message, List<ErrorMessage> errors)
        {
            errors.Add(new ErrorMessage() { Message = message, Severity = ErrorMessageTypes.ERROR });
        }

        private static void CheckRequest(ExternalRequest request)
        {
            var errors = new List<ErrorMessage>();

            if ((String.IsNullOrEmpty(request.Patient.LastName)))
                AddError("Не указана фамилия пациента. Невозможно продолжить создание заявки", errors);

            /*if ((String.IsNullOrEmpty(request.Patient.FirstName)))
                AddError("Не указано имя пациента. Невозможно продолжить создание заявки", errors);

            if ((String.IsNullOrEmpty(request.Patient.MiddleName)))
                AddError("Не указано отчество пациента. Невозможно продолжить создание заявки", errors);*/

            if (String.IsNullOrEmpty(request.HospitalCode))
                AddError("Код ЛПУ [HospitalCode] не может быть пустым. Невозможно продолжить создание заявки;", errors);
            else if (request.HospitalCode.Length > 16)
                AddError("Длина кода ЛПУ [HospitalCode] не должна превышать 16 символов. Невозможно продолжить создание заявки;", errors);

            var hospital = (HospitalDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.Hospital, request.HospitalCode, true);
            if (hospital == null)
            {
                if (String.IsNullOrEmpty(request.HospitalName))
                    AddError("Наименование ЛПУ [HospitalName] не может быть пустым. Невозможно продолжить создание заявки;", errors);
                else if (request.HospitalName.Length > 1024)
                    AddError("Длина наименования ЛПУ [HospitalName] не должна превышать 1024 символов. Невозможно продолжить создание заявки;", errors);
            }
            else
            {
                if (String.IsNullOrEmpty(request.HospitalName))
                    request.HospitalName = hospital.Name;
            }

            if (request.DepartmentCode != null)
                if (request.DepartmentCode.Length > 16)
                    AddError("Длина кода подразделения ЛПУ [DepartmentCode] не должна превышать 16 символов. Невозможно продолжить создание заявки;", errors);

            if (request.DoctorCode != null)
                if (request.DoctorCode.Length > 16)
                    AddError("Длина кода врача [DoctorCode] не должна превышать 16 символов. Невозможно продолжить создание заявки;", errors);


            if (errors.Count > 0)
            {
                request.Errors.AddRange(errors);
                throw new ExternalRequestCheckException();
            }
        }
    }
}
