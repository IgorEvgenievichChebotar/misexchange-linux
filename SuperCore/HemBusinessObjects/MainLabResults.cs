//using System;
//using ru.novolabs.SuperCore.HemDictionary;
//using ru.novolabs.SuperCore.DictionaryCore;

//namespace ru.novolabs.SuperCore.HemBusinessObjects
//{
//    public class MainLabResults : StageResults
//    {
//        public MainLabResults()
//        {
//        }
        
//        public MainLabResults(Donation donation)
//        {
//            this.Id = donation.Id;
//            this.donation = new ObjectRef(donation);
//            this.donor = donation.Donor.Id;
//            this.department = donation.Department;
//            this.Date = donation.Date;
//            GetMainLabValues(donation);
//        }


//        private int donor = 0;
//        private ObjectRef donation = new ObjectRef();
//        private DepartmentDictionaryItem department = null;
//        private DateTime date = DateTime.MinValue;
//        private Int32? hb;
//        private float? erit;
//        private Int32? ret;
//        private Int32? belok;
//        private Int32? gemat;
//        private float? leyk;
//        private Int32? soe;
//        private Int32? mchc;
//        private Int32? trom;
//        private Int32? abo;
//        private Int32? mcv;
//        private Int32? alt;
//        private float? mch;
//        private String kell;
//        private String rh;

//        private void GetEpressLabValues(Visit visit)
//        {
//            foreach (VisitStage stage in visit.Stages)
//            {
//                if (stage.StageType == StageTypes.STAGE_TYPE_EXPRESS_LAB)
//                {
//                    GetVisitStageValues(stage);
//                }
//            }
//        }

//        private void GetMainLabValues(Donation donation)
//        {
//            GetVisitStageValues(donation.MainLabStage);            
//        }

//        public int Donor
//        {
//            get { return donor; }
//            set { donor = value; }
//        }
//        public ObjectRef Donation
//        {
//            get { return donation; }
//            set { donation = value; }
//        }

//        [LinkedDictionary("department", "ExternalCode")]
//        public DepartmentDictionaryItem Department
//        {
//            get { return department; }
//            set { department = value; }
//        }
//        public DateTime Date
//        {
//            get { return date; }
//            set { date = value; }
//        }
//        public Int32? Hb
//        {
//            get { return hb; }
//            set { hb = value; }
//        }
//        public float? Erit
//        {
//            get { return erit; }
//            set { erit = value; }
//        }
//        public Int32? Ret
//        {
//            get { return ret; }
//            set { ret = value; }
//        }
//        public Int32? Trom
//        {
//            get { return trom; }
//            set { trom = value; }
//        }

//        public float? Leyk
//        {
//            get { return leyk; }
//            set { leyk = value; }
//        }
//        public Int32? Soe
//        {
//            get { return soe; }
//            set { soe = value; }
//        }
//        public Int32? Gemat
//        {
//            get { return gemat; }
//            set { gemat = value; }
//        }

//        public Int32? Belok
//        {
//            get { return belok; }
//            set { belok = value; }
//        }
//        public Int32? Abo
//        {
//            get { return abo; }
//            set { abo = value; }
//        }

//        public String Rh
//        {
//            get { return rh; }
//            set { rh = value; }
//        }


//        public String Kell
//        {
//            get { return kell; }
//            set { kell = value; }
//        }

//        public float? Mch
//        {
//            get { return mch; }
//            set { mch = value; }
//        }
//        public Int32? Mchc
//        {
//            get { return mchc; }
//            set { mchc = value; }
//        }
//        public Int32? Mcv
//        {
//            get { return mcv; }
//            set { mcv = value; }
//        }
//        public Int32? Alt
//        {
//            get { return alt; }
//            set { alt = value; }
//        }


//    }
//}
