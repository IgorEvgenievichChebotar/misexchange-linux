using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System;
using System.Collections.Generic;

namespace ru.novolabs.MisExchangeService.Adapters
{
    [Obsolete("This adapter is only for old ExchangeHelpers")]
    public class ExternalResultAdapterBase
    {
        public ExchangeDTOs.Result WriteDTO(ExternalResult result)
        {
            ExchangeDTOs.Result dto = ExchangeDTOs.DTOInitializer.BuildEmptyResult();

            dto.Id = result.Id;
            dto.RequestCode = result.RequestCode ?? string.Empty;
            dto.OrganizationCode = result.OrganizationCode;
            dto.OrganizationName = result.OrganizationName;
            dto.Ogrn = result.Ogrn;
            dto.HospitalCode = result.HospitalCode;
            dto.HospitalName = result.HospitalName;
            dto.State = result.State;
            dto.Priority = result.Priority;
            dto.LegalCode = result.LegalCode;
            dto.DepartmentCode = result.DepartmentCode;
            dto.DepartmentName = result.DepartmentName;
            dto.DoctorCode = result.DoctorCode;
            dto.DoctorName = result.DoctorName;
            dto.PayCategoryCode = result.PayCategoryCode;
            dto.PayCategoryName = result.PayCategoryName;
            CopyPatient(result.Patient, dto.Patient);
            CopySampleResults(result.SampleResults, dto.SampleResults);
            CopyUserFields(result.UserFields, dto.UserFields);

            dto.SampleDeliveryDate = result.SampleDeliveryDate == DateTime.MinValue ? null : (DateTime?)result.SampleDeliveryDate;
            dto.SamplingDate = result.SamplingDate == DateTime.MinValue ? null : (DateTime?)result.SamplingDate;
            dto.RegistrationDate = result.RegistrationDate == DateTime.MinValue ? null : (DateTime?)result.RegistrationDate;

            dto.Email = result.Email;
            dto.Telephone = result.Telephone;
            dto.ExecutorOrganizationCode = result.ExecutorOrganizationCode;
            dto.ExecutorOrganizationName = result.ExecutorOrganizationName;
            dto.CardAmbulatory = result.CardAmbulatory;
            dto.PatientWeightInKg = result.PatientWeightInKg;

            return dto;
        }

        private void CopyUserFields(List<UserValue> source, List<ExchangeDTOs.UserField> target)
        {
            source.ForEach(uv => target.Add(new ExchangeDTOs.UserField() { Code = uv.Code ?? string.Empty, Name = uv.Name ?? string.Empty, Value = uv.Value }));
        }

        private void CopyPatient(ExternalRequestPatient patient, ExchangeDTOs.Patient dto)
        {
            dto.Code = patient.Code ?? string.Empty;
            dto.FirstName = patient.FirstName ?? string.Empty;
            dto.LastName = patient.LastName ?? string.Empty;
            dto.MiddleName = patient.MiddleName ?? string.Empty;
            dto.BirthDay = patient.BirthDay ?? 0;
            dto.BirthMonth = patient.BirthMonth ?? 0;
            dto.BirthYear = patient.BirthYear ?? 0;
            dto.Sex = patient.Sex == null ? 0 : patient.Sex.Id;
            dto.Country = patient.Country;
            dto.Region = patient.Region;
            dto.Area = patient.Area;
            dto.City = patient.City;
            dto.Location = patient.Location;
            dto.Street = patient.Street;
            dto.Building = patient.Building;
            dto.Flat = patient.Flat;
            dto.LivingAddressCountry = patient.LivingAddressCountry;
            dto.LivingAddressRegion = patient.LivingAddressRegion;
            dto.LivingAddressArea = patient.LivingAddressArea;
            dto.LivingAddressCity = patient.LivingAddressCity;
            dto.LivingAddressLocation = patient.LivingAddressLocation;
            dto.LivingAddressStreet = patient.LivingAddressStreet;
            dto.LivingAddressBuilding = patient.LivingAddressBuilding;
            dto.LivingAddressFlat = patient.LivingAddressFlat;
            dto.InsuranceCompany = patient.InsuranceCompany;
            dto.PolicySeries = patient.PolicySeries;
            dto.PolicyNumber = patient.PolicyNumber;
            CopyPatientCard(dto.PatientCard, patient.PatientCard);
            CopyUserFields(patient.UserFields, dto.UserFields);
        }

        private void CopyPatientCard(ExchangeDTOs.PatientCard dto, ExternalRequestPatientCard patientCard)
        {
            dto.CardNr = patientCard.CardNr ?? string.Empty;
            CopyUserFields(patientCard.UserFields, dto.UserFields);
        }

        private void CopySampleResults(List<ExternalSampleResult> sourceList, List<ExchangeDTOs.SampleResult> targetList)
        {
            foreach (ExternalSampleResult sampleResult in sourceList)
            {
                ExchangeDTOs.SampleResult dto = new ExchangeDTOs.SampleResult();
                targetList.Add(dto);

                foreach (FileInfo image in sampleResult.Images)
                {
                    ExchangeDTOs.FileInfo dtoImage = new ExchangeDTOs.FileInfo();
                    dto.Images.Add(dtoImage);

                    dtoImage.Filename = image.PathFilename;
                    dtoImage.FileContent = image.PathFileContent;
                }

                dto.Id = sampleResult.Id;
                dto.Barcode = sampleResult.Barcode;
                dto.BiomaterialCode = sampleResult.BiomaterialCode ?? string.Empty;
                dto.Comments = sampleResult.Comments;
                dto.DepartmentNr = sampleResult.DepartmentNr;
                dto.EndDate = sampleResult.EndDate == DateTime.MinValue ? null : sampleResult.EndDate;
                dto.SampleDeliveryDate = sampleResult.SampleDeliveryDate;
                CopyDefects(sampleResult.Defects, dto.Defects);
                CopyTargetResults(sampleResult.TargetResults, dto.TargetResults);
                CopyMicroResults(sampleResult.MicroResults, dto.MicroResults);
            }
        }

        private void CopyDefects(List<ExternalDefect> sourceList, List<ExchangeDTOs.Defect> targetList)
        {
            foreach (ExternalDefect defect in sourceList)
            {
                ExchangeDTOs.Defect dto = new ExchangeDTOs.Defect();
                targetList.Add(dto);

                dto.Code = defect.Code;
                dto.Name = defect.Name;
            }
        }

        private void CopyTargetResults(List<ExternalTargetResult> sourceList, List<ExchangeDTOs.TargetResult> targetList)
        {
            foreach (ExternalTargetResult targetResult in sourceList)
            {
                ExchangeDTOs.TargetResult dto = new ExchangeDTOs.TargetResult();
                targetList.Add(dto);

                dto.Code = targetResult.Code ?? string.Empty;
                dto.Name = targetResult.Name;
                dto.Comments = targetResult.Comments;
                CopyWorks(targetResult.Works, dto.Works);
            }
        }

        private bool isCopyMicroorganismCFUResultToValue()
        {
            bool? setting = (bool?)ProgramContext.Settings["copyMicroorganismCFUResultToValue", false];
            return setting != null ? setting.Value : false;
        }

        private void CopyMicroResults(List<ExternalMicroResult> sourceList, List<ExchangeDTOs.MicroResult> targetList)
        {
            foreach (ExternalMicroResult microResult in sourceList)
            {
                ExchangeDTOs.MicroResult dto = new ExchangeDTOs.MicroResult();
                targetList.Add(dto);
                dto.Code = microResult.Code;
                dto.Name = microResult.Name;
                dto.Value = microResult.Value;
                dto.Comments = microResult.Comments;
                dto.Found = microResult.Found;
                if (isCopyMicroorganismCFUResultToValue())
                {
                    dto.Value = microResult.ColonyFormingUnitValue;
                }

                CopyWorks(microResult.Antibiotics, dto.Antibiotics);
                dto.ParentWork = CopyWork(microResult.ParentWork);
            }
        }

        private void CopyWorks(List<ExternalWork> sourceList, List<ExchangeDTOs.Work> targetList)
        {
            foreach (ExternalWork work in sourceList)
            {
                ExchangeDTOs.Work dto = CopyWork(work);
                targetList.Add(dto);
            }
        }

        protected virtual ExchangeDTOs.Work CopyWork(ExternalWork work)
        {
            if (work != null)
            {
                ExchangeDTOs.Work dto = ExchangeDTOs.DTOInitializer.BuildEmptyWork();
                dto.LisWorkId = work.Id;
                dto.Code = work.Code;
                dto.Name = work.Name;
                dto.Value = work.Value;
                dto.UnitName = work.UnitName;
                dto.State = work.State;
                CopyNorm(work.Norm, dto.Norm);
                dto.Normality = work.Normality;
                CopyDefects(work.Defects, dto.Defects);
                dto.ApprovingDoctor = work.ApprovingDoctor;
                dto.ApprovingDoctorCode = work.ApprovingDoctorCode;
                dto.ApprovingDoctorSnils = work.ApprovingDoctorSnils;
                dto.EquipmentCode = work.EquipmentCode;
                dto.EquipmentName = work.EquipmentName;
                dto.CreateDate = work.CreateDate.Value;
                dto.ApproveDate = work.ApproveDate == DateTime.MinValue ? null : work.ApproveDate;
                dto.ModifyDate = work.ModifyDate == DateTime.MinValue ? null : work.ModifyDate;
                dto.Diameter = work.Diameter;
                dto.Comments = work.Comments;
                dto.GroupRank = work.GroupRank;
                dto.RankInGroup = work.RankInGroup;
                dto.TestRank = work.TestRank;
                dto.Precision = !string.IsNullOrEmpty(work.Precision) ? int.Parse(work.Precision) : (int?)null;
                dto.PatientGroupCode = work.PatientGroupCode;
                if (ProgramContext.Settings["exchangeMode"].Equals("ExchangeHelper3_MIS_Statistics"))
                {
                    dto.ResultSource = work.ResultSource;
                    dto.AnalyzerCode = work.AnalyzerCode;
                    dto.AnalyzerName = work.AnalyzerName;
                }

                foreach (FileInfo image in work.Images)
                {
                    ExchangeDTOs.FileInfo dtoImage = new ExchangeDTOs.FileInfo();
                    dto.Images.Add(dtoImage);

                    dtoImage.Filename = image.PathFilename;
                    dtoImage.FileContent = image.PathFileContent;
                }

                return dto;
            }
            return null;
        }

        private void CopyNorm(ExternalNorm norm, ExchangeDTOs.Norm dto)
        {
            dto.LowLimit = norm.LowLimit;
            dto.HighLimit = norm.HighLimit;
            dto.CriticalLowLimit = norm.CriticalLowLimit;
            dto.CriticalHighLimit = norm.CriticalHighLimit;
            dto.Norms = norm.Norms;
            dto.NormComment = norm.NormComment;
            dto.UnitName = norm.UnitName;
        }
    }
}