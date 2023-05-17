using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.MisExchangeService.Adapters
{
    public class EntitiesRequestAdapter : RequestAdapter
    {
        static class ExternalPriorities
        {
            internal const Int32 Priority_Low = 0;
            internal const Int32 Priority_High = 1;
        }

        #region DTO -> Request

        public override ExternalRequest ReadDTO(object obj)
        {
            var dto = (MisExchangeEntities.Request)obj;
            var request = new ExternalRequest();

            request.RequestCode = dto.RequestCode;
            request.RegistrationFormCode = (string)ProgramContext.Settings["registrationFormCode"];
            request.HospitalCode = dto.HospitalCode;
            request.HospitalName = dto.HospitalName;
            request.DepartmentCode = dto.DepartmentCode;
            request.DepartmentName = dto.DepartmentName;
            request.DoctorCode = dto.DoctorCode;
            request.DoctorName = dto.DoctorName;
            request.SamplingDate = dto.SamplingDate ?? DateTime.Now;
            request.SampleDeliveryDate = dto.SampleDeliveryDate;
            request.PregnancyDuration = dto.PregnancyDuration;
            request.CyclePeriod = dto.CyclePeriod;
            request.Readonly = dto.ReadOnly ?? false;
            CopyPatientFromDTO(dto.Patient, request.Patient);
            CopySamplesFromDTO(dto.Samples, request.Samples);
            CopyUserFieldsFromDTO(dto.UserFields, request.UserFields);

            return request;
        }

        private void CopyPatientFromDTO(MisExchangeEntities.Patient dto, ExternalRequestPatient patient)
        {
            patient.Code = dto.Code;
            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.MiddleName = dto.MiddleName;
            patient.BirthDay = dto.BirthDay;
            patient.BirthMonth = dto.BirthMonth;
            patient.BirthYear = dto.BirthYear;
            patient.Sex = ProgramContext.LisCommunicator.getSexById( dto.Sex );
            patient.Country = dto.Country;
            patient.City = dto.City;
            patient.Street = dto.Street;
            patient.Building = dto.Building;
            patient.Flat = dto.Flat;
            patient.InsuranceCompany = dto.InsuranceCompany;
            patient.PolicySeries = dto.PolicySeries;
            patient.PolicyNumber = dto.PolicyNumber;
            CopyPatientCardFromDTO(dto.PatientCard, patient.PatientCard);
            CopyUserFieldsFromDTO(dto.UserFields, patient.UserFields);
        }

        private void CopyUserFieldsFromDTO(ICollection<MisExchangeEntities.UserField> source, List<UserValue> target)
        {
            foreach (var uf in source)
            {
                target.Add(new UserValue() { Code = uf.Code, Name = uf.Name, Value = uf.Value });
            }
        }

        private void CopyPatientCardFromDTO(MisExchangeEntities.PatientCard dto, ExternalRequestPatientCard patientCard)
        {
            patientCard.CardNr = dto.CardNr;
            CopyUserFieldsFromDTO(dto.UserFields, patientCard.UserFields);
        }

        private void CopySamplesFromDTO(ICollection<MisExchangeEntities.Sample> sourceList, List<ExternalRequestSample> targetList)
        {
            foreach (var dto in sourceList)
            {
                var sample = new ExternalRequestSample();
                targetList.Add(sample);

                sample.Barcode = dto.Barcode;
                sample.BiomaterialCode = dto.BiomaterialCode;
                sample.Priority = dto.Priority;
                CopyTargetsFromDTO(dto.Targets, sample.Targets);
            }
        }

        private void CopyTargetsFromDTO(ICollection<MisExchangeEntities.Target> sourceList, List<ExternalRequestTarget> targetList)
        {
            foreach (var dto in sourceList)
            {
                var target = new ExternalRequestTarget();
                targetList.Add(target);

                target.Code = dto.Code;
                target.Priority = dto.Priority;
                target.ReadOnly = dto.ReadOnly ?? false;
            }
        }

        #endregion

        #region Request -> DTO

        public override object WriteDTO(ExternalRequest request)
        {
            return null;
        }

        #endregion
    }
}