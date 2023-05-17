//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using FicExchange.Core;
//using FicExchange.Dictionary;
//using FicExchange.External;
//using System.Reflection;

//namespace FicExchange.BusinessObjects
//{
//    public class MainLab: BaseObject
//    {
//        public MainLab(Donation donation)
//        {
//            this.Id = donation.Id;
//            this.donation = new ObjectRef(donation);
//            this.donor = donation.Donor.ID;
//            this.department = new ObjectRef(donation.Department);
//            this.Date = donation.Date;
//            GetParamValues(donation);
//        }


//        private int donor = 0;
//        private ObjectRef donation = new ObjectRef();
//        private ObjectRef department = new ObjectRef();
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


//        private void GetParamValues(Donation donation)
//        {

//            foreach (ParameterValueClass paramValue in donation.MainLabStage.Values)
//            {
//                if ((paramValue.State == ParameterValueState.STATE_APPROVED) && paramValue.Parameter.isValidObject())
//                {
//                    String paramName = paramValue.Parameter.ExternalCode;
//                    PropertyInfo propInfo = GetType().GetProperty(paramName);
//                    if (propInfo != null)
//                    {
//                        String value = string.Empty;
//                        if (paramValue.Reference.isValidObject())
//                        {
//                            value = paramValue.Reference.ExternalCode;
//                        }
//                        else
//                        {
//                            value = paramValue.Value;
//                        }
//                        if (!String.Empty.Equals(value))
//                        {
//                            SetParamValue(propInfo, value);
//                        }
//                    }
//                }
//            }
//        }

//        private void SetParamValue(PropertyInfo propInfo, string value)
//        {
//            try
//            {
//                Type pType = propInfo.PropertyType;
//                // Если свойство имеет тип дженерика
//                if (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(Nullable<>))
//                {
//                    pType = pType.GetGenericArguments()[0];
//                }


//                if (pType.Equals(typeof(Int32)))
//                    {
//                        propInfo.SetValue(this, Int32.Parse(value), null);
//                    }
//                else if (pType.Equals(typeof(float)))
//                    {
//                        propInfo.SetValue(this, float.Parse(value), null);
//                    }
//                    else
//                    {
//                        propInfo.SetValue(this, value, null);
//                    }
                
//            }
//            catch
//            {
//             Log.WriteError(String.Format("Не удалось преобразовать значение {0} к типу {1} для параметра {2}", 
//                 value, propInfo.PropertyType.Name, propInfo.Name));
//             }
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
//        public ObjectRef Department
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
