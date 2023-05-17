using System;
using System.Collections.Generic;
using ru.novolabs.ExchangeDTOs;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.MisExchangeService.Adapters
{
    public class FilesRequestAdapter : RequestAdapter
    {
        static class ExternalPriorities
        {
            internal const Int32 Priority_Low = 0;
            internal const Int32 Priority_High = 1;
        }

        #region DTO -> Request

        public override ExternalRequest ReadDTO(object obj)
        {
            var dto = (ExchangeDTOs.Request)obj;
            var request = new ExternalRequest();

            request.RequestCode = dto.RequestCode;
            request.RegistrationFormCode = (string)ProgramContext.Settings["registrationFormCode"];
            request.HospitalCode = dto.HospitalCode;
            request.HospitalName = dto.HospitalName;
            request.DepartmentCode = dto.DepartmentCode;
            request.DepartmentName = dto.DepartmentName;
            request.DoctorCode = dto.DoctorCode;
            request.DoctorName = dto.DoctorName;
            request.SamplingDate = dto.SamplingDate ?? DateTime.MinValue;
            request.SampleDeliveryDate = dto.SampleDeliveryDate;
            request.PregnancyDuration = dto.PregnancyDuration;
            request.CyclePeriod = dto.CyclePeriod;
            request.Readonly = dto.ReadOnly?? false;
            CopyPatientFromDTO(dto.Patient, request.Patient);
            CopySamplesFromDTO(dto.Samples, request.Samples);
            CopyUserFieldsFromDTO(dto.UserFields, request.UserFields);

            return request;
        }

        private void CopyPatientFromDTO(ExchangeDTOs.Patient dto, ExternalRequestPatient patient)
        {
            patient.Code = dto.Code;
            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.MiddleName = dto.MiddleName;
            patient.BirthDay = dto.BirthDay;
            patient.BirthMonth = dto.BirthMonth;
            patient.BirthYear = dto.BirthYear;
            patient.Sex = ProgramContext.LisCommunicator.getSexById(dto.Sex);
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

        private void CopyUserFieldsFromDTO(List<UserField> source, List<UserValue> target)
        {
            source.ForEach(uf => target.Add(new UserValue() { Code = uf.Code, Name = uf.Name, Value = uf.Value }));
        }

        private void CopyPatientCardFromDTO(ExchangeDTOs.PatientCard dto, ExternalRequestPatientCard patientCard)
        {
            patientCard.CardNr = dto.CardNr;
            CopyUserFieldsFromDTO(dto.UserFields, patientCard.UserFields);
        }

        private void CopySamplesFromDTO(List<Sample> sourceList, List<ExternalRequestSample> targetList)
        {
            foreach (Sample dto in sourceList)
            {
                var sample = new ExternalRequestSample();
                targetList.Add(sample);

                sample.Barcode = dto.Barcode;
                sample.BiomaterialCode = dto.BiomaterialCode;
                sample.Priority = dto.Priority ?? 0;
                CopyTargetsFromDTO(dto.Targets, sample.Targets);
            }
        }

        private void CopyTargetsFromDTO(List<Target> sourceList, List<ExternalRequestTarget> targetList)
        {
            foreach (var dto in sourceList)
            {
                var target = new ExternalRequestTarget();
                targetList.Add(target);

                target.Code = dto.Code;
                //target.Cancel = dto.Cancel;
                target.Priority = dto.Priority ?? 0;
                target.ReadOnly = dto.ReadOnly ?? false;
            }
        }

        #endregion

        #region Request -> DTO

        public override object WriteDTO(ExternalRequest request)
        {
            var dto = ExchangeDTOs.DTOInitializer.BuildEmptyRequest();//new ExchangeDTOs.Request();

            dto.RequestCode = request.RequestCode;
            //dto.RegistrationFormCode = request.RegistrationFormCode;
            dto.HospitalCode = request.HospitalCode;
            dto.HospitalName = request.HospitalName;
            dto.DepartmentCode = request.DepartmentCode;
            dto.DepartmentName = request.DepartmentName;
            dto.DoctorCode = request.DoctorCode;
            dto.DoctorName = request.DoctorName;
            dto.SamplingDate = request.SamplingDate;
            dto.SampleDeliveryDate = request.SampleDeliveryDate ?? DateTime.MinValue;
            dto.PregnancyDuration = request.PregnancyDuration ?? 0;
            dto.CyclePeriod = request.CyclePeriod ?? 0;
            dto.ReadOnly = request.Readonly;
            CopyPatientToDTO(request.Patient, dto.Patient);
            CopySamplesToDTO(request.Samples, dto.Samples);
            CopyUserFieldsToDTO(request.UserFields, dto.UserFields);

            return dto;
        }

        private void CopyPatientToDTO(ExternalRequestPatient patient, ExchangeDTOs.Patient dto)
        {
            dto.Code = patient.Code;
            dto.FirstName = patient.FirstName;
            dto.LastName = patient.LastName;
            dto.MiddleName = patient.MiddleName;
            dto.BirthDay = patient.BirthDay ?? 0;
            dto.BirthMonth = patient.BirthMonth ?? 0;
            dto.BirthYear = patient.BirthYear ?? 0;
            dto.Sex = patient.Sex == null ? 0 : patient.Sex.Id;
            dto.Country = patient.Country;
            dto.City = patient.City;
            dto.Street = patient.Street;
            dto.Building = patient.Building;
            dto.Flat = patient.Flat;
            dto.InsuranceCompany = patient.InsuranceCompany;
            dto.PolicySeries = patient.PolicySeries;
            dto.PolicyNumber = patient.PolicyNumber;
            CopyPatientCardToDTO(patient.PatientCard, dto.PatientCard);
            CopyUserFieldsToDTO(patient.UserFields, dto.UserFields);
        }

        private void CopyPatientCardToDTO(ExternalRequestPatientCard patientCard, ExchangeDTOs.PatientCard dto)
        {
            dto.CardNr = patientCard.CardNr;
            CopyUserFieldsToDTO(patientCard.UserFields, dto.UserFields);
        }

        private void CopyUserFieldsToDTO(List<UserValue> source, List<UserField> target)
        {
            source.ForEach(uv => target.Add(new UserField() { Code = uv.Code, Name = uv.Name, Value = uv.Value }));
        }

        private void CopySamplesToDTO(List<ExternalRequestSample> sourceList, List<Sample> targetList)
        {
            foreach (var sample in sourceList)
            {
                var dto = new ExchangeDTOs.Sample();
                targetList.Add(dto);

                dto.Barcode = sample.Barcode;
                dto.BiomaterialCode = sample.BiomaterialCode;
                dto.Priority = sample.Priority;
                CopyTargetsToDTO(sample.Targets, dto.Targets);
            }
        }

        private void CopyTargetsToDTO(List<ExternalRequestTarget> sourceList, List<Target> targetList)
        {
            foreach (var target in sourceList)
            {
                var dto = new ExchangeDTOs.Target();
                targetList.Add(dto);
                

                dto.Code = target.Code;
                //dto.Cancel = target.Cancel;
                dto.Priority = target.Priority;
                dto.ReadOnly = target.ReadOnly;
            }
        }

        #endregion
    }
}