using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using ru.novolabs.SuperCore.LimsBusinessObjects.Outsource;
using ru.novolabs.SuperCore.LimsDictionary;
using MisExchangeAdapters.Result;

namespace ru.novolabs.MisExchangeService.Adapters
{
    public class EntitiesResult3Adapter : Result3Adapter
    {
        public override Object WriteDTO(ExchangeDTOs.Result result)
        {
            var dto = new MisExchangeEntities.Result();

            dto.RequestCode = result.RequestCode;
            dto.Patient = new MisExchangeEntities.Patient();
            CopyPatient(result.Patient, dto.Patient);
            CopySampleResults(result.SampleResults, dto.SampleResults);
            CopyUserFields(result.UserFields, dto.UserFields);

            return dto;
        }

        private void CopyUserFields(List<ExchangeDTOs.UserField> source, ICollection<MisExchangeEntities.UserField> target)
        {
            foreach (var uv in source)
            {
                target.Add(new MisExchangeEntities.UserField() { Code = uv.Code, Name = uv.Name, Value = uv.Value });
            }
        }

        private void CopyPatient(ExchangeDTOs.Patient patient, MisExchangeEntities.Patient dto)
        {
            dto.Code = patient.Code;
            dto.FirstName = patient.FirstName;
            dto.LastName = patient.LastName;
            dto.MiddleName = patient.MiddleName;
            dto.BirthDay = patient.BirthDay.HasValue ? patient.BirthDay.Value : 0;
            dto.BirthMonth = patient.BirthMonth.HasValue ? patient.BirthMonth.Value : 0;
            dto.BirthYear = patient.BirthYear.HasValue ? patient.BirthYear.Value : 0;
            dto.Sex = patient.Sex;
            dto.Country = patient.Country;
            dto.City = patient.City;
            dto.Street = patient.Street;
            dto.Building = patient.Building;
            dto.Flat = patient.Flat;
            dto.InsuranceCompany = patient.InsuranceCompany;
            dto.PolicySeries = patient.PolicySeries;
            dto.PolicyNumber = patient.PolicyNumber;
            dto.PatientCard = new MisExchangeEntities.PatientCard();
            CopyPatientCard(dto.PatientCard, patient.PatientCard);
            //dto.UserFields
            CopyUserFields(patient.UserFields, dto.UserFields);
        }

        private void CopyPatientCard(MisExchangeEntities.PatientCard dto, ExchangeDTOs.PatientCard patientCard)
        {
            dto.CardNr = patientCard.CardNr;
            CopyUserFields(patientCard.UserFields, dto.UserFields);
        }

        private void CopySampleResults(List<ExchangeDTOs.SampleResult> sourceList, ICollection<MisExchangeEntities.SampleResult> targetList)
        {
            foreach (var sampleResult in sourceList)
            {
                var dto = new MisExchangeEntities.SampleResult();
                targetList.Add(dto);

                dto.Barcode = sampleResult.Barcode;
                dto.BiomaterialCode = sampleResult.BiomaterialCode;
                dto.Comments = sampleResult.Comments;
                CopyDefects(sampleResult.Defects, dto.Defects);
                CopyTargetResults(sampleResult.TargetResults, dto.TargetResults);
                CopyMicroResults(sampleResult.MicroResults, dto.MicroResults);
            }
        }

        private void CopyDefects(List<ExchangeDTOs.Defect> sourceList, ICollection<MisExchangeEntities.Defect> targetList)
        {
            foreach (var defect in sourceList)
            {
                var dto = new MisExchangeEntities.Defect();
                targetList.Add(dto);

                dto.Code = defect.Code;
                dto.Name = defect.Name;
            }
        }

        private void CopyTargetResults(List<ExchangeDTOs.TargetResult> sourceList, ICollection<MisExchangeEntities.TargetResult> targetList)
        {
            foreach (var targetResult in sourceList)
            {
                var dto = new MisExchangeEntities.TargetResult();
                targetList.Add(dto);

                dto.Code = targetResult.Code;
                dto.Name = targetResult.Name;
                dto.Comments = targetResult.Comments;
                CopyWorks(targetResult.Works, dto.Works);
            }
        }

        private void CopyMicroResults(List<ExchangeDTOs.MicroResult> sourceList, ICollection<MisExchangeEntities.MicroResult> targetList)
        {
            foreach (var microResult in sourceList)
            {
                var dto = new MisExchangeEntities.MicroResult();
                targetList.Add(dto);

                dto.Code = microResult.Code;
                dto.Name = microResult.Name;
                dto.Value = microResult.Value;
                dto.Comments = microResult.Comments;
                CopyWorks(microResult.Antibiotics, dto.Antibiotics);
            }
        }

        private void CopyWorks(List<ExchangeDTOs.Work> sourceList, ICollection<MisExchangeEntities.Work> targetList)
        {
            foreach (var work in sourceList)
            {
                var dto = new MisExchangeEntities.Work();
                targetList.Add(dto);

                dto.Code = work.Code;
                dto.Name = work.Name;
                //dto.Mnemonics = work.Mnemonics;
                dto.Value = work.Value ?? String.Empty;
                dto.UnitName = work.UnitName;
                dto.State = work.State;
                dto.Norm = new MisExchangeEntities.Norm();
                CopyNorm(work.Norm, dto.Norm);
                dto.Normality = work.Normality;
                CopyDefects(work.Defects, dto.Defects);
                dto.ApprovingDoctor = work.ApprovingDoctor;
                //dto.ApprovingDoctorCode = work.ApprovingDoctorCode;
                //dto.EquipmentCode = work.EquipmentCode;
                dto.CreateDate = work.CreateDate;
                dto.ApproveDate = work.ApproveDate;
                dto.ModifyDate = work.ModifyDate;
                //dto.Images = work.Images;
                dto.Comments = work.Comments;
                dto.GroupRank = work.GroupRank;
                dto.RankInGroup = work.RankInGroup;
                dto.TestRank = work.TestRank;
                dto.Precision = work.Precision;
            }
        }

        private void CopyNorm(ExchangeDTOs.Norm norm, MisExchangeEntities.Norm dto)
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