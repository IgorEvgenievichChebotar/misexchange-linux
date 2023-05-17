using ru.novolabs.SuperCore.DictionaryCommon;
using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{

    

    public class Donor : BaseObject
    {
        public Donor()
        {
            NrPrefix = NrValue = NrSuffix = String.Empty;
            LastName = MiddleName = FirstName = String.Empty;
            BloodParameters = new List<BloodParameterValue>();
            LivingAddress = new AddressClass();
            DocumentAddress = new AddressClass();
            Transfusions = new List<Transfusion>();
            Invitations = new List<Donation>();
            //GroupStates = new List<StateGroupItem>();
            Sex = new SexDictionaryItem();
            //Department = new DepartmentDictionaryItem();
            //DonorCategory = new DonorCategoryDictionaryItem();
            //DocumentType = new DocumentTypeDictionaryItem();
            //PersonalDonors = new List<Donor>();
            //Recipients = new List<Donor>();
            DonorLinks = new List<DonorLink>();
            PatientCards = new List<PatientCard>();
            Quotas = new List<Quota>();
        }


        private Boolean copyDocumentAddress = false;

        [SendToServer(false)]
        [CSN("Nr")]
        public String Nr { get { return NrPrefix + NrValue + NrSuffix; } }

        [CSN("ExternalId")]
        public String ExternalId { get; set; }

        [CSN("NrValue")]
        public String NrValue { get; set; }

        [CSN("Department")]
        public DepartmentDictionaryItem Department { get; set; }

        [SendToServer(false)]
        [CSN("NrPrefix")]
        public String NrPrefix { get; set; }

        [SendToServer(false)]
        [CSN("NrSuffix")]
        public String NrSuffix { get; set; }

        /// <summary>
        /// Фамилия + инициалы
        /// </summary>
        [SendToServer(false)]
        [CSN("ShortFullName")]
        public String ShortFullName
        {
            get
            {
                string result = String.Empty;
                if (!String.IsNullOrEmpty(LastName))
                {
                    result += LastName;
                    if (!String.IsNullOrEmpty(FirstName))
                    {
                        result += " " + FirstName.Substring(0, 1) + ".";
                        if (!String.IsNullOrEmpty(MiddleName))
                            result += " " + MiddleName.Substring(0, 1) + ".";
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Возраст пациента
        /// </summary>
        [SendToServer(false)]
        [CSN("Age")]
        public String Age
        {
            get
            {
                string Result = String.Empty;

                if (BirthDate != null)
                {

                    int birthDay = int.Parse(BirthDate.ToString().Substring(0, 2));
                    int birthMonth = int.Parse(BirthDate.ToString().Substring(3, 2));
                    int birthYear = int.Parse(BirthDate.ToString().Substring(6, 4));

                    int nowDay = int.Parse(DateTime.Now.ToString().Substring(0, 2));
                    int nowMonth = int.Parse(DateTime.Now.ToString().Substring(3, 2));
                    int nowYear = int.Parse(DateTime.Now.ToString().Substring(6, 4));

                    int result = 0;

                    if (nowMonth < birthMonth)
                        result = (nowYear - birthYear) - 1;

                    if (nowMonth > birthMonth)
                        result = nowYear - birthYear;

                    if (nowMonth == birthMonth)
                    {
                        if (nowDay >= birthDay)
                            result = nowYear - birthYear;
                        else
                            result = (nowYear - birthYear) - 1;
                    }

                    Result = result.ToString();
                }
                
                return Result;
            }
        }

        [CSN("FirstName")]
        public String FirstName { get; set; }

        [CSN("LastName")]
        public String LastName { get; set; }

        [CSN("MiddleName")]
        public String MiddleName { get; set; }

        [CSN("Photo")]
        public ObjectRef Photo { get; set; }

        [CSN("Sex")]
        [SendAsInt(true)]
        public SexDictionaryItem Sex { get; set; }

        [CSN("BirthDate")]
        public DateTime? BirthDate { get; set; }

        [CSN("DonorStatus")]
        public Int32 DonorStatus { get; set; }

        [CSN("DonorCategory")]
        public DonorCategoryDictionaryItem DonorCategory { get; set; }

        [CSN("HasVisits")]
        public Boolean HasVisits { get; set; }

        [CSN("DocumentType")]
        public DocumentTypeDictionaryItem DocumentType { get; set; }

        [CSN("DocumentSeries")]
        public String DocumentSeries { get; set; }

        [CSN("DocumentNr")]
        public String DocumentNr { get; set; }

        [CSN("LivingAddress")]
        public AddressClass LivingAddress { get; set; }

        [SendToServer(false)]
        [CSN("CopyDocumentAddress")]
        public Boolean CopyDocumentAddress
        {
            get { return copyDocumentAddress; }
            set
            {
                copyDocumentAddress = value;
                if (value)
                    LivingAddress = DocumentAddress;
            }
        }

        [CSN("DocumentAddress")]
        public AddressClass DocumentAddress { get; set; }

        [CSN("PhoneHome")]
        public String PhoneHome { get; set; }

        [CSN("PhoneMobile")]
        public String PhoneMobile { get; set; }

        [CSN("PhoneWork")]
        public String PhoneWork { get; set; }

        [CSN("Occupation")]
        public Int32 Occupation { get; set; }

        [CSN("WorkPlace")]
        public String WorkPlace { get; set; }

        [CSN("Profession")]
        public String Profession { get; set; }

        [CSN("Comments")]
        public String Comments { get; set; }

        [CSN("NextVisitDate")]
        public DateTime? NextVisitDate { get; set; }

        [CSN("Honored")]
        public Boolean Honored { get; set; }

        [CSN("BloodParameters")]
        public List<BloodParameterValue> BloodParameters { get; set; }

        [CSN("OldDonationsCount")]
        public Int32 OldDonationsCount { get; set; }

        [CSN("NrEDC")]
        public String NrEDC { get; set; }

        [CSN("FirstDonationDate")]
        public DateTime? FirstDonationDate { get; set; }

        [SendToServer(false)]
        [CSN("FullName")]
        public string FullName
        {
            get { return LastName + ' ' + FirstName + ' ' + MiddleName; }
            set { setFullName(value); }
        }

        private void setFullName(string value)
        {
            String name1 = null;
            String name2 = null;
            String name3 = null;

            value = value.Trim();
            Int32 pos = value.LastIndexOf(' ');
            if (pos > 0)
            {
                name3 = value.Substring(pos + 1);
                value = value.Substring(0, pos).Trim();
                pos = value.LastIndexOf(' ');
                if (pos > 0)
                {
                    name2 = value.Substring(pos + 1);
                    name1 = value.Substring(0, pos).Trim();
                }
                else name2 = value;
            }
            if (null != name1) LastName = name1;
            if (null != name2) FirstName = name2;
            if (null != name3) MiddleName = name3;
        }

        [SendToServer(false)]
        [CSN("LastDonationsCount")]
        public Int32? LastDonationsCount { get; set; }

        [SendToServer(false)]
        [CSN("LastDonationDate")]
        public DateTime? LastDonationDate { get; set; }

        [SendToServer(false)]
        [CSN("Removed")]
        public Boolean Removed { get; set; }

        [SendToServer(false)]
        [CSN("Date")]
        public DateTime Date { get; set; }

        [SendToServer(false)]
        [CSN("Denies")]
        public List<DonorDeny> Denies { get; set; }

        [SendToServer(false)]
        [CSN("ActiveDeny")]
        public DonorDeny ActiveDeny { get; set; }

        [CSN("Transfusions")]
        public List<Transfusion> Transfusions { get; set; }

        [SendToServer(false)]
        [CSN("Invitations")]
        public List<Donation> Invitations { get; set; }

        ////[CSN("PersonalDonors")]
        ////public List<Donor> PersonalDonors { get; set; }

        ////[CSN("Recipients")]
        ////public List<Donor> Recipients { get; set; }

        [CSN("IsDonor")]
        public Boolean IsDonor { get; set; } 
        [CSN("IsPatient")]
        public Boolean IsPatient { get; set; }

        [SendToServer(false)]
        [CSN("GroupStates")]
        public List<StateGroupItem> GroupStates { get; set; }

        [SendToServer(false)]
        [CSN("DonorLinks")]
        public List<DonorLink> DonorLinks { get; set; }

        [CSN("ActiveDeny_StatusName")]
        public String ActiveDeny_StatusName
        {
            get
            {
                if (ActiveDeny != null)
                {
                    if (ActiveDeny.Deny == null || ActiveDeny.Deny.Id == 0)
                    {
                        return DonorDenyStatusName.DonorDenyStatusOk;
                    }
                    else
                        if (ActiveDeny.Removed)
                        {
                            return DonorDenyStatusName.DonorDenyStatusRemoved;
                        }
                        else
                        {
                            switch (ActiveDeny.Deny.DenyType)
                            {
                                case (int)DonorDenyStatus.BC_DONOR_DENY_STATUS_ABSOLUTE:
                                    //if (ActiveDeny.Waiting)
                                        return DonorDenyStatusName.DonorDenyStatusAbsolute;
                                case (int)DonorDenyStatus.BC_DONOR_DENY_STATUS_RELATIVE:
                                    if (ActiveDeny.Expired)
                                        return DonorDenyStatusName.DonorDenyStatusExpired;
                                    else
                                        return DonorDenyStatusName.DonorDenyStatusRelative;
                            }
                        }
                }
                    //switch (ActiveDeny.DonorCurrentStatus)
                    //{
                    //    case (int)DonorDenyStatus.BC_DONOR_DENY_STATUS_ABSOLUTE:
                    //        return DonorDenyStatusName.DonorDenyStatusAbsolute;
                    //    case (int)DonorDenyStatus.BC_DONOR_DENY_STATUS_ABSOLUTE_WAITING:
                    //        return DonorDenyStatusName.DonorDenyStatusAbsolute;
                    //    case (int)DonorDenyStatus.BC_DONOR_DENY_STATUS_EXPIRED:
                    //        return DonorDenyStatusName.DonorDenyStatusExpired;
                    //    case (int)DonorDenyStatus.BC_DONOR_DENY_STATUS_OK:
                    //        return DonorDenyStatusName.DonorDenyStatusOk;
                    //    case (int)DonorDenyStatus.BC_DONOR_DENY_STATUS_RELATIVE:
                    //        return DonorDenyStatusName.DonorDenyStatusRelative;
                    //    case (int)DonorDenyStatus.BC_DONOR_DENY_STATUS_RELATIVE_WAITING:
                    //        return DonorDenyStatusName.DonorDenyStatusRelative;
                    //    case (int)DonorDenyStatus.BC_DONOR_DENY_STATUS_REMOVED:
                    //        return DonorDenyStatusName.DonorDenyStatusRemoved;
                    //}
                return DonorDenyStatusName.DonorDenyStatusOk;
            }
            set
            {
            }

        }

        [CSN("Email")]
        public String Email { get; set; }

        [CSN("EmailSmsNotify")]
        public Boolean EmailSmsNotify { get; set; }

        /// <summary>
        /// Список карт пациента. Свойство, специфичное для объекта "Пациент" (в данный момент для доноров и пациентов используется единый класс Donor)
        /// </summary>
        [CSN("PatientCards")]
        [SendToServer(false)]
        public List<PatientCard> PatientCards { get; set; }

        [CSN("Quotas")]
        public List<Quota> Quotas { get; set; }

        [CSN("ActiveQuota")]
        [SendToServer(false)]
        public Quota ActiveQuota { get; set; }
    }
    
    public class Quota : BaseObject
    {
        [CSN("AdministrativePermissionNumber")]
        public string AdministrativePermissionNumber { get; set; }
        /// <summary>
        /// дата начала адм. разрешения
        /// </summary>
        [CSN("AdministrativePermissionDateFrom")]
        public DateTime? AdministrativePermissionDateFrom { get; set; }
        /// <summary>
        /// дата окончания адм. разрешения
        /// </summary>
        [CSN("AdministrativePermissionDateTill")]
        public DateTime? AdministrativePermissionDateTill { get; set; }
        /// <summary>
        /// номер квоты
        /// </summary>
        [CSN("QuotaNumber")]
        public string QuotaNumber { get; set; }
        /// <summary>
        /// дата начала квоты
        /// </summary>
        [CSN("QuotaDateFrom")]
        public DateTime? QuotaDateFrom { get; set; }
        /// <summary>
        /// дата окончания квоты
        /// </summary>
        [CSN("QuotaDateTill")]
        public DateTime? QuotaDateTill { get; set; }
        /// <summary>
        /// Сумма квоты
        /// </summary>
        [CSN("QuotaSum")]
        public float? QuotaSum { get; set; }
        /// <summary>
        /// код источника финансирования
        /// </summary>
        [CSN("FundingSourceCode")]
        public string FundingSourceCode { get; set; }
        /// <summary>
        /// наименование источника финансирования
        /// </summary>
        [CSN("FundingSourceName")]
        public string FundingSourceName { get; set; }
        /// <summary>
        /// код вида ВМП
        /// </summary>
        [CSN("VmpTypeCode")]
        public string VmpTypeCode { get; set; }
    }

    
    public class DonorSaveResult
    {
        public DonorSaveResult()
        {
            Errors = new List<ErrorMessage>();
        }

        [CSN("Id")]
        public int Id { get; set; }

        [CSN("Errors")]
        public List<ErrorMessage> Errors { get; set; }
    }

    public class DonorDenySave
    {
        [CSN("Waiting")]
        public bool Waiting { get; set; }

        [CSN("DateFrom")]
        public DateTime DateFrom { get; set; }

        [CSN("Donor")]
        public ObjectRef Donor { get; set; }

        [CSN("Deny")]
        public ObjectRef Deny { get; set; }

        [CSN("Source")]
        public ObjectRef Source { get; set; }

        [CSN("Duration")]
        public int Duration { get; set; }

        [CSN("DurationUnit")]
        public int DurationUnit { get; set; }

        [CSN("Comment")]
        public string Comment { get; set; }
    }

    /// <summary>
    /// Связь с донором
    /// </summary>
    public class DonorLink
    {
        /// <summary>
        /// Тип связи
        /// </summary>
        [CSN("Id")]
        public Int32 Id { get; set; }
        /// <summary>
        /// Дата создания связи
        /// </summary>
        [CSN("Date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Тип связи
        /// </summary>
        [CSN("RelationType")]
        public Int32 RelationType { get; set; }
        
        //    Типы связей.
        //10- Успешный реципиент. Продукты от данного донора были успешно перелиты связанному реципиенту.
        //20- Не успешный реципиент.При переливании продуктов от данного донора связанному возникли осложнения.
        //30- Успешный донор. Данному реципиенту были успешно перелиты продукты от связанного.
        //40- Не успешный донор. При переливании данному реципиента продуктов от связанного возникли осложнения.
        //50- Потенциальный реципиент. Данный донор может сдавать для связанного.
        //60- Потенциальный донор. Связанный донор может сдавать для данного.
        //70- Совмещенный донор. Донор был совмещен данному реципиенту.
        //80- Совмещенный реципиент. Реципиент был совмещен данному донору.

        /// <summary>
        /// Связанный донор
        /// </summary>
        [CSN("Donor")]
        public ObjectRef Donor { get; set; }

        /// <summary>
        /// Трансфузия, явившаяся основанием для создания связи
        /// </summary>
        [CSN("Transfusion")]
        public ObjectRef Transfusion { get; set; }

        /// <summary>
        /// Пользователь, создавший связь
        /// </summary>
        [CSN("User")]
        public ObjectRef User { get; set; }

        /// <summary>
        /// Связь удалена
        /// </summary>
        [CSN("Removed")]
        public ObjectRef Removed { get; set; }

        /// <summary>
        /// Дата удаления связи
        /// </summary>
        [CSN("RemoveDate")]
        public DateTime RemoveDate { get; set; }

        /// <summary>
        /// Пользователь, удаливший связь
        /// </summary>
        [CSN("RemoveUser")]
        public ObjectRef RemoveUser { get; set; }

        /// <summary>
        /// Трансфузия, ставшая причиной удаления
        /// </summary>
        [CSN("RemoveTransfusion")]
        public ObjectRef RemoveTransfusion { get; set; }
    }

}