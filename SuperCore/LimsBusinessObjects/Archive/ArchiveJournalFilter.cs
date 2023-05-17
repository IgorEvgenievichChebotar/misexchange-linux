using ru.novolabs.SuperCore.LimsDictionary;
using System;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class ArchiveJournalFilter : BaseJournalFilter
    {
        public ArchiveJournalFilter()
        {
            Clear();
        }

        /// <summary>
        /// Номер архивного хранилища
        /// </summary>
        [CSN("ArchiveStorageNr")]
        public string ArchiveStorageNr { get; set; }
        /// <summary>
        /// Номер штатива
        /// </summary>
        [CSN("ArchiveRackNr")]
        public string ArchiveRackNr { get; set; }
        /// <summary>
        /// Номер пробирки
        /// </summary>
        [CSN("ArchiveTubeNr")]
        public string ArchiveTubeNr { get; set; }
        /// <summary>
        /// Дата создания штатива (начальная)
        /// </summary>
        [CSN("ArchiveRackCreationDateFrom")]
        public DateTime? ArchiveRackCreationDateFrom { get; set; }
        /// <summary>
        /// Дата создания штатива (конечная)
        /// </summary>
        [CSN("ArchiveRackCreationDateTill")]
        public DateTime? ArchiveRackCreationDateTill { get; set; }
        /// <summary>
        /// Не учитывать дату создания штатива
        /// </summary>
        [CSN("SkipArchiveRackCreationDate")]
        [SendToServer(false)]
        public Boolean SkipArchiveRackCreationDate { get; set; }
        /// <summary>
        /// Дата создания заявки (начальная)
        /// </summary>
        [CSN("RequestCreationDateFrom")]
        public DateTime? RequestCreationDateFrom { get; set; }
        /// <summary>
        /// Дата создания заявки (конечная)
        /// </summary>
        [CSN("RequestCreationDateTill")]
        public DateTime? RequestCreationDateTill { get; set; }
        /// <summary>
        /// Не учитывать дату создания заявки
        /// </summary>
        [CSN("SkipRequestCreationDate")]
        [SendToServer(false)]
        public Boolean SkipRequestCreationDate { get; set; }
        /// <summary>
        /// Фамилия пациента
        /// </summary>
        [CSN("PatientLastName")]
        public string PatientLastName { get; set; }
        /// <summary>
        /// Имя пациента
        /// </summary>
        [CSN("PatientFirstName")]
        public string PatientFirstName { get; set; }
        /// <summary>
        /// Отчество пациента
        /// </summary>
        [CSN("PatientMiddleName")]
        public string PatientMiddleName { get; set; }
        /// <summary>
        /// Дата истечения срока годности (начальная)
        /// </summary>
        [CSN("ExpireDateFrom")]
        public DateTime? ExpireDateFrom { get; set; }
        /// <summary>
        /// Дата истечения срока годности (конечная)
        /// </summary>
        [CSN("ExpireDateTill")]
        public DateTime? ExpireDateTill { get; set; }        
        /// <summary>
        /// Не учитывать дату истечения срока годности
        /// </summary>
        [CSN("SkipExpireDate")]
        [SendToServer(false)]
        public Boolean SkipExpireDate { get; set; }
        /// <summary>
        /// Штатив "Закрыт" (более не учитывается)
        /// </summary>
        [CSN("Closed")]
        public int? Closed { get; set; }


        public override void Clear()
        {
            ArchiveRackCreationDateFrom = ArchiveRackCreationDateTill = RequestCreationDateFrom = RequestCreationDateTill = DateTime.Now;
            SkipArchiveRackCreationDate = SkipRequestCreationDate = SkipExpireDate = true;
            ArchiveRackNr = ArchiveStorageNr = ArchiveTubeNr = PatientLastName = PatientFirstName = PatientMiddleName = String.Empty;
            Closed = (int)YesNoIgnore.No;
        }

        public override void PrepareToSend()
        {
            if (SkipArchiveRackCreationDate)
            {
                ArchiveRackCreationDateFrom = ArchiveRackCreationDateTill = null;
            }
            if (SkipRequestCreationDate)
            {
                RequestCreationDateFrom = RequestCreationDateTill = null;
            }
            if (SkipExpireDate)
            {
                ExpireDateFrom = ExpireDateTill = null;
            }

            base.PrepareToSend();
        }
    }
}