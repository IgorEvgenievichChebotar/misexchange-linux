using System;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    /// <summary>
    /// Класс описывает результат нормы для одного простого исследования.
    /// Нормы вычисляются с учетом пола и возраста пациента а так же прочих параметров
    /// таких как фаза цикла, срок беременности и тд.
    /// </summary>
    [XmlType(TypeName = "Norm")]
    public class ExternalNorm
    {
        public ExternalNorm()
        {
            Norms = "";
        }

        // Критические значения
        [XmlIgnore]
        [CSN("CriticalLowLimit")]
        public Double CriticalLowLimit
        {
            get { return CryticalLowLimit; }
            set { CryticalLowLimit = value; } 
        }
        [CSN("LowLimit")]
        public Double LowLimit { get; set; }
        [CSN("HighLimit")]
        public Double HighLimit { get; set; }
        [XmlIgnore]
        [CSN("CriticalHighLimit")]
        public Double CriticalHighLimit
        {
            get { return CryticalHighLimit; }
            set { CryticalHighLimit = value; } 
        }
        // Нормы
        [CSN("Norms")]
        public String Norms { get; set; }
        [CSN("NormComment")]
        public String NormComment { get; set; }
        [CSN("UnitName")]
        public String UnitName { get; set; }
        [CSN("NormName")]
        public String NormName { get; set; }

        // Свойства с некорректным именем "crYtical" вместо "crItical". Нужны для выгрузки согласно первой версии протокола интеграции
        [CSN("CryticalLowLimit")]
        public Double CryticalLowLimit { get; set; }
        [CSN("CryticalHighLimit")]
        public Double CryticalHighLimit { get; set; }
    }
}
