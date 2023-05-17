using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MisExchangeAdapters.Parser.HL7.Classes
{
    /// <summary>
    /// Информация о пациента
    /// </summary>
    public class PatientInfo
    {

        public PatientInfo()
        {
            PatientName = new PatientName();
        }
        /// <summary>
        /// Тип сегмента (PID)
        /// </summary>
        //[DataMember(Order = 0)]
        public String SegmentType;
        /// <summary>
        /// ID пациента
        /// </summary>
        //[DataMember(Order = 1)]
        public String PatientId;
        /// <summary>
        /// Расширенный ID пациента
        /// </summary>
        //[DataMember(Order = 3)]
        public String FullPatientId;
        /// <summary>
        /// Альтернативный ID пациента (PID-4)
        /// </summary>
        public String AlterPatientId;
        /// <summary>
        /// ФИО пациента
        /// </summary>
        //[DataMember(Order = 5)]
        public PatientName PatientName;
        /// <summary>
        /// Дата рождения пациента
        /// </summary>
        //[DataMember(Order = 7)]
        public DateTime PatientBirthDate;
        /// <summary>
        /// Пол пациента
        /// </summary>
        //[DataMember(Order = 8)]
        public String PatientSex;

        public Int32 PatientSexInt
        {
            get
            {
                switch (PatientSex)
                {
                    case HL7Sex.Male:
                        return 1;
                    case HL7Sex.Female:
                        return 2;
                    case HL7Sex.Other:
                    case HL7Sex.Unknown:
                    default:
                        return 0;
                }
            }
        }
        /// <summary>
        /// Адрес пациента
        /// </summary>
        //[DataMember(Order = 11)]
        public String PatientAddress;
        /// <summary>
        /// Телефон пациента
        /// </summary>
        //[DataMember(Order = 13)]
        public String PatientPhone;
        /// <summary>
        /// Номер страховки пациента
        /// </summary>
        //[DataMember(Order = 19)]
        public String PatientInsurance;
        /// <summary>
        /// Национальность пациента
        /// </summary>
        //[DataMember(Order = 28)]
        public String PatientNationality;

        

        public override string ToString()
        {
            return String.Join(
                HL7SeparatorConst.DefaultFieldDelim.ToString(),
                SegmentType,
                PatientId,
                FullPatientId,
                AlterPatientId,

                "",
                PatientName.ToString(),
                "",
                PatientBirthDate.ToString("yyyyMMdd"),
                PatientSex,
                "", 
                "",
                PatientAddress,
                "",
                PatientPhone,
                new String('|', 5),
                PatientInsurance,
                new String('|', 8));
        }
    }

    public class PatientName
    {
        public String LastName { get; set; } // 1
        public String FirstName { get; set; } // 2
        public String MiddleName { get; set; } // 3

        public override string ToString()
        {
            return String.Join(HL7SeparatorConst.ComponentSeparator.ToString(), LastName, FirstName, MiddleName);
        }
    }
        
}
