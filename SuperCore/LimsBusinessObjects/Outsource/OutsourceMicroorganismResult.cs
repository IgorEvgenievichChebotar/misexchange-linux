using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Outsource
{
    public class OutsourceMicroorganismResult
    {

        public OutsourceMicroorganismResult()
        {
            AntibioticResults = new List<OutsourceAntibioticResult>();
        }

        /// <summary>
        /// Количественный результат поиска
        /// </summary>
        [CSN("Value")]
        public String Value { get; set; }
        /// <summary>
        /// Качественный результат поиска
        /// </summary>
        [CSN("Found")]
        public Boolean Found { get; set; }
        /// <summary>
        /// Результаты воздействия антибиотиков
        /// </summary>
        [CSN("AntibioticResults")]
        public List<OutsourceAntibioticResult> AntibioticResults { get; set; }
        /// <summary>
        /// Маппинг микроорганизма
        /// </summary>
        [SendAsRef(true)]
        [CSN("Microorganism")]
        public OutsourceMicroOrganismMapping Microorganism { get; set; }

        [SendToServer(false)]
        [CSN("MicroorganismCode")]
        public String MicroorganismCode { get; set; }
    }
}
