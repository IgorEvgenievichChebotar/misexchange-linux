using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    [XmlType(TypeName = "RequestFilter")]
    public class ExternalRequestFilter : BaseRegistrationJournalFilter
    {
        public ExternalRequestFilter()
        {
            RequestCodes = new List<String>();
            PatientCodes = new List<String>();
            States = new List<Int32>();
            CustomStates = new List<String>();
            Targets = new List<String>();
            Hospitals = new List<String>();
            CustDepartments = new List<String>();
            Doctors = new List<String>();
            Departments = new List<String>();
            Errors = new List<ErrorMessage>();
        }

        [XmlArrayItem(ElementName = "Code")]
        [CSN("RequestCodes")]
        public List<String> RequestCodes { get; set; }
        [XmlArrayItem(ElementName = "Code")]
        [CSN("PatientCodes")]
        public List<String> PatientCodes { get; set; }
        [XmlArrayItem(ElementName = "State")]
        [CSN("States")]
        public List<Int32> States { get; set; } // Статусы заявок: 1 -  Регистрация; 2 - Открыта; 3 – Закрыта
        [XmlArrayItem(ElementName = "Code")]
        [CSN("CustomStates")]
        public List<String> CustomStates { get; set; }
        [XmlArrayItem(ElementName = "Code")]
        [CSN("Targets")]
        public List<String> Targets { get; set; }
        [XmlArrayItem(ElementName = "Code")]
        [CSN("Hospitals")]
        public List<String> Hospitals { get; set; }
        [XmlArrayItem(ElementName = "Code")]
        [CSN("CustDepartments")]
        public List<String> CustDepartments { get; set; }
        [XmlArrayItem(ElementName = "Code")]
        [CSN("Doctors")]
        public List<String> Doctors { get; set; }
        [XmlArrayItem(ElementName = "Code")]
        [CSN("Departments")]
        public List<String> Departments { get; set; }
        // Список ошибок, возникших при получении журнала заявок с использованием даного фильтра.
        // Используется только при помещении файла фильтра в папку с ошибками
        [SendToServer(false)]
        [CSN("Errors")]
        public List<ErrorMessage> Errors { get; set; }


        private void PrepareReferenceList(List<String> codes, String dictionaryName, List<ObjectRef> referenceList, String errorMessage)
        {
            foreach (String dictionaryItemCode in codes)
            {
                DictionaryItem dictionaryItem = (DictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(dictionaryName, dictionaryItemCode);
                if (dictionaryItem != null)
                    referenceList.Add(new ObjectRef(dictionaryItem.Id));
                else
                {
                    ErrorMessage error = new ErrorMessage();
                    error.Message = String.Format(errorMessage, dictionaryItemCode);
                    Errors.Add(error);
                }
            }
        }

        private void PrepareAndOrIdList(List<String> codes, String dictionaryName, AndOrIdList andOrIdList, String errorMessage)
        {
            foreach (String dictionaryItemCode in codes)
            {
                DictionaryItem dictionaryItem = (DictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(dictionaryName, dictionaryItemCode);
                if (dictionaryItem != null)
                    andOrIdList.IdList.Add(new ObjectRef(dictionaryItem.Id));
                else
                {
                    ErrorMessage error = new ErrorMessage();
                    error.Message = String.Format(errorMessage, dictionaryItemCode);
                    Errors.Add(error);
                }
            }
        }

        public RegistrationJournalFilter ToRequestJournalFilter()
        {
            String EM_DEPARTMENT_NOT_FOUND = "Подразделение лаборатории не найдено по коду \"{0}\"";
            String EM_TARGET_NOT_FOUND = "Исследование не найдено по коду \"{0}\"";
            String EM_REQUEST_CUSTOM_STATE_NOT_FOUND = "Дополнительный статус заявки не найден по коду \"{0}\"";

            RegistrationJournalFilter result = new RegistrationJournalFilter();
            result.FirstName = this.FirstName;
            result.LastName = this.LastName;
            result.MiddleName = this.MiddleName;
            result.BirthDate = this.BirthDate;
            result.Sex = this.Sex; // ???????????????????????????????????????
            // Заменяем значения констант на принятые в ЛИС
            /*  switch (this.Sex)
              {
                  case 0: result.Sex = SexConst.LIS_GENDER_NONE; break;
                  case 1: result.Sex = SexConst.LIS_GENDER_MALE; break;
                  case 2: result.Sex = SexConst.LIS_GENDER_FEMALE; break;
                  default: result.Sex = SexConst.LIS_GENDER_ALL; break;
              }*/

            result.SampleDeliveryDateFrom = DateTimeHelper.StartOfTheDay(this.SampleDeliveryDateFrom);
            result.SampleDeliveryDateTill = DateTimeHelper.EndOfTheDay(this.SampleDeliveryDateTill);
            result.EndDateFrom = DateTimeHelper.StartOfTheDay(this.EndDateFrom);
            result.EndDateTill = DateTimeHelper.EndOfTheDay(this.EndDateTill);
            result.Priority = this.Priority; //????????????????????????????????????????????????
            result.DefectState = this.DefectState; //???????????????????????????????????????????

            foreach (Int32 state in States)
                result.States.Add(new ObjectRef(state));

            result.InternalNrs.AddRange(this.RequestCodes);
            result.PatientCodes.AddRange(this.PatientCodes);

            // Заменяем коды справочных объектов ссылками
            lock (ProgramContext.Dictionaries)
            {
                ProcessHospitalDepartmentDoctorData(result);
                PrepareReferenceList(CustomStates, LimsDictionaryNames.RequestCustomState, result.CustomStates, EM_REQUEST_CUSTOM_STATE_NOT_FOUND);

                PrepareAndOrIdList(Departments, LimsDictionaryNames.Department, result.Departments, EM_DEPARTMENT_NOT_FOUND);
                PrepareAndOrIdList(Targets, LimsDictionaryNames.Target, result.Targets, EM_TARGET_NOT_FOUND);
            }

            return result;
        }

        private List<ObjectRef> GetDictionaryItemRefs(List<String> codes, string dictionaryName, string errorMessage, ref bool hasErrors)
        {
            var referenceList = new List<ObjectRef>();

            foreach (String dictionaryItemCode in codes)
            {
                DictionaryItem dictionaryItem = (DictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(dictionaryName, dictionaryItemCode);
                if (dictionaryItem != null)
                    referenceList.Add(new ObjectRef(dictionaryItem.Id));
                else
                {
                    hasErrors = true;
                    ErrorMessage error = new ErrorMessage();
                    error.Message = String.Format(errorMessage, dictionaryItemCode);
                    Errors.Add(error);
                }
            }

            return referenceList;
        }


        private void ProcessHospitalDepartmentDoctorData(RegistrationJournalFilter regJournalFilter)
        {
            String EM_CUST_DEPARTMENT_NOT_FOUND = "Отделение ЛПУ не найдено по коду \"{0}\"";
            String EM_HOSPITAL_NOT_FOUND = "ЛПУ не найдено по коду \"{0}\"";
            String EM_DOCTOR_NOT_FOUND = "Врач не найден по коду \"{0}\"";

            if (Doctors.Count > 0 || CustDepartments.Count > 0)
            {
                if (Hospitals.Count == 0)
                {
                    Errors.Add(new ErrorMessage() { Message = "При указанных кодах отделений или кодах врачей обязательно должны быть указаны коды ЛПУ", Severity = (int)ErrorMessageType.Error });
                    return;
                }
            }

            bool hasErrors, anyErrors;
            hasErrors = anyErrors = false;
            var refs = GetDictionaryItemRefs(Doctors, LimsDictionaryNames.Doctor, EM_DOCTOR_NOT_FOUND, ref anyErrors);
            refs = GetDictionaryItemRefs(CustDepartments, LimsDictionaryNames.CustDepartment, EM_CUST_DEPARTMENT_NOT_FOUND, ref hasErrors);
            anyErrors = anyErrors || hasErrors;
            refs = GetDictionaryItemRefs(Hospitals, LimsDictionaryNames.Hospital, EM_HOSPITAL_NOT_FOUND, ref hasErrors);
            anyErrors = anyErrors || hasErrors;
            if (anyErrors)
                return;

            foreach (var reference in refs)
            {
                var hospital = (HospitalDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.Hospital, reference.Id];
                if (CustDepartments.Count == 0)
                {
                    regJournalFilter.Hospitals.Add(reference);
                    continue;                
                }

                foreach (var custDepartmentCode in CustDepartments)
                {
                    var custDepartment = hospital.CustDepartments.Find(cd => !cd.Removed && cd.Code == custDepartmentCode);
                    if (custDepartment != null)
                    {
                        if (Doctors.Count == 0)
                        {
                            regJournalFilter.Hospitals.Add(reference);
                            regJournalFilter.CustDepartments.Add(new ObjectRef(custDepartment.Id));
                            continue;
                        }

                        foreach (var doctorCode in Doctors)
                        {
                            var doctor = custDepartment.Doctors.Find(d => !d.Removed && d.Code == doctorCode);
                            if (doctor != null)
                            {
                                regJournalFilter.Hospitals.Add(reference);
                                regJournalFilter.CustDepartments.Add(new ObjectRef(custDepartment.Id));
                                regJournalFilter.Doctors.Add(new ObjectRef(doctor.Id));
                            }
                        }
                    }
                }
            }
        }
    }
}