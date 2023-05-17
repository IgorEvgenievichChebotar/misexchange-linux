using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public interface IDonorHeader
    {
        ObjectRef Donor { get; set; }
        string DonorNrPrefix { get; set; }
        string DonorNrValue { get; set; }
        string DonorNrSuffix { get; set; }
        string DonorNr { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string MiddleName { get; set; }
        int Sex { get; set; }
        DateTime? BirthDate { get; set; }
        int DonorCurrentStatus { get; set; }
        DonorCategoryDictionaryItem DonorCurrentCategory { get; set; }
        bool Honored { get; set; }
        List<BloodParameterValue> BloodParameters { get; set; }
    }

    public class DonorHeader : BaseObject, IDonorHeader
    {
        public DonorHeader()
        {
            Donor = new ObjectRef();
            DonorCurrentCategory = new DonorCategoryDictionaryItem();
            BloodParameters = new List<BloodParameterValue>();
        }

        [CSN("Donor")]
        public ObjectRef Donor { get; set; }
        [CSN("DonorNrPrefix")]
        public string DonorNrPrefix { get; set; }
        [CSN("DonorNrValue")]
        public string DonorNrValue { get; set; }
        [CSN("DonorNrSuffix")]
        public string DonorNrSuffix { get; set; }
        [CSN("FirstName")]
        public string FirstName { get; set; }
        [CSN("LastName")]
        public string LastName { get; set; }
        [CSN("MiddleName")]
        public string MiddleName { get; set; }
        [CSN("Sex")]
        public int Sex { get; set; }
        [CSN("BirthDate")]
        public DateTime? BirthDate { get; set; }
        [CSN("DonorCurrentStatus")]
        public int DonorCurrentStatus { get; set; }
        [CSN("DonorCurrentCategory")]
        public DonorCategoryDictionaryItem DonorCurrentCategory { get; set; }
        [CSN("NextVisitDate")]
        public DateTime? NextVisitDate { get; set; }
        [CSN("Honored")]
        public bool Honored { get; set; }
        [SendToServer(false)]
        [CSN("BloodParameters")]
        public List<BloodParameterValue> BloodParameters { get; set; }
        [SendToServer(false)]
        [CSN("DonorNr")]
        public string DonorNr
        {
            get { return String.Concat(DonorNrPrefix, DonorNrValue, DonorNrSuffix); }
        }
        [SendToServer(false)]
        [CSN("FullName")]
        public string FullName
        {
            get { return String.Concat(new string[] { LastName, " ", FirstName, " ", MiddleName }); }
        }

        //property ShortName: string read GetShortName;
        //property BirthDateForCustomLayout: TBirthDate read GetBirthDateForCustomLayout write SetBirthDateForCustomLayout;
        //property FullLivingAddress: string read FFullLivingAddress write FFullLivingAddress;
        //property FullDocumentAddress: string read FFullDocumentAddress write FFullDocumentAddress;
    }
}