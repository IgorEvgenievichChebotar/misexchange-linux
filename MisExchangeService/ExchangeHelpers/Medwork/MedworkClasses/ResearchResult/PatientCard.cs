using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.ResearchResult
{
    /// <summary>
    /// Класс описывает карту пациента
    /// </summary>     
    public class PatientCard
    {
        public PatientCard()
        {
            UserFields = new List<UserField>();
            CardNr = String.Empty;
        }
        [XmlIgnore]
        public long Id { get; set; }
        /// <summary>
        /// Номер карты
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public String CardNr { get; set; }
        /// <summary>
        /// Пользовательские поля
        /// </summary>
        [FieldProps()]
        public List<UserField> UserFields { get; set; }
    }
}