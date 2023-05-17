using ru.novolabs.SuperCore.LimsBusinessObjects.Outsource;
using System.Collections.Generic;

namespace ru.novolabs
{
    public class OutsourceAdapter
    {
        public OutsourceRequest ResultDtoToOutsourceRequest(object resultDTO)
        {
            var dto = (ru.novolabs.ExchangeDTOs.Result)resultDTO;
            var outsourceRequest = new OutsourceRequest();

            outsourceRequest.RequestCode = dto.RequestCode;
            //outsourceRequest.SampleDeliveryDate = dto.SampleDeliveryDate;
            //var hospital = String.IsNullOrEmpty(dto.HospitalCode) ? null : (HospitalDictionaryItem)ProgramContext.Dictionaries[LisDictionaryNames.Hospital, dto.HospitalCode];
            //outsourceRequest.Hospital = hospital;
            // to-do: уточнить эквивалентность констант в Request.State и OutsourceRequest.ResultState
            //outsourceRequest.ResultState = dto.State;

            // В данный момент редактирование данных о пациенте, карте пациента, пользовательских полях заявки/пациента/карты пациента не предусмотрено.
            // Если всё-таки понадобится, то нужно раскомментировать следующие строки и убрать атрибут "[SendToServer(false)]" у соответствующих свойств класса OutsourceRequest

            //CopyPatientFromDTO(dto.Patient, outsourceRequest.Patient);
            //CopyUserFieldsFromDTO(dto.UserFields, outsourceRequest.UserFields);

            CopySampleResultsFromDTO(dto.SampleResults, outsourceRequest.Samples);

            return outsourceRequest;
        }

        private void CopySampleResultsFromDTO(List<ExchangeDTOs.SampleResult> sourceList, List<OutsourceSample> targetList)
        {
            foreach (var dto in sourceList)
            {
                var sample = new OutsourceSample();
                targetList.Add(sample);

                sample.Barcode = dto.Barcode;
                sample.BiomaterialCode = dto.BiomaterialCode;
                sample.Comments = dto.Comments;
                CopyDefectsFromDTO(dto.Defects, sample.Defects);
                CopyTargetResultsFromDTO(dto.TargetResults, sample.Targets);
                //CopyMicroResultsFromDTO(dto.MicroResults, sample.MicroResults);
            }
        }

        private void CopyDefectsFromDTO(List<ExchangeDTOs.Defect> sourceList, List<OutsourceDefectInfo> targetList)
        {
            foreach (var dto in sourceList)
            {
                var defect = new OutsourceDefectInfo();
                targetList.Add(defect);

                defect.Code = dto.Code;
                defect.Name = dto.Name;
                defect.DefectCode = dto.Code;
            }
        }

        private void CopyTargetResultsFromDTO(List<ExchangeDTOs.TargetResult> sourceList, List<OutsourceTarget> targetList)
        {
            foreach (var dto in sourceList)
            {
                var targetResult = new OutsourceTarget();
                targetList.Add(targetResult);

                targetResult.Comments = dto.Comments;
                targetResult.Code = dto.Code;
                CopyWorksFromDTO(dto.Works, targetResult.Works);
            }
        }

        private void CopyWorksFromDTO(List<ExchangeDTOs.Work> sourceList, List<OutsourceWork> targetList)
        {
            foreach (var dto in sourceList)
            {
                var work = new OutsourceWork();
                targetList.Add(work);

                work.Code = dto.Code;
                work.Name = dto.Name;
                work.Value = dto.Value;
                work.UnitName = dto.UnitName;
                work.State = dto.State.ToString();
                CopyNormFromDTO(dto.Norm, work.Norm);
                CopyDefectsFromDTO(dto.Defects, work.Defects);
                work.ApprovingDoctor = dto.ApprovingDoctor;
                work.CreateDate = dto.CreateDate;
                work.ApproveDate = dto.ApproveDate;
                work.ModifyDate = dto.ModifyDate;
                //dto.Images = work.Images;
                work.Comments = dto.Comments;
            }
        }

        private void CopyNormFromDTO(ExchangeDTOs.Norm dto, OutsourceNorm outsourceNorm)
        {
            outsourceNorm.LowLimit = dto.LowLimit;
            outsourceNorm.HighLimit = dto.HighLimit;
            outsourceNorm.CriticalLowLimit = dto.CriticalLowLimit;
            outsourceNorm.CriticalHighLimit = dto.CriticalHighLimit;
            outsourceNorm.Norms = dto.Norms;
            outsourceNorm.NormComment = dto.NormComment;
            outsourceNorm.UnitName = dto.UnitName;
        }
    }
}
