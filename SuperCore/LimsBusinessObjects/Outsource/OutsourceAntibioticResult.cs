using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Outsource
{
    public class OutsourceAntibioticResult
    {
        /// <summary>
        /// Результат воздействия
        /// </summary>
        [CSN("Value")]
        public String Value { get; set; }
        /// <summary>
        /// Код антибиотика
        /// </summary>
        /// 
        [SendAsRef(true)]
        [CSN("AntibioticTest")]
        public OutsourceTestMapping AntibioticTest { get; set; }

        [CSN("Comment")]
        public String Comment { get; set; }
    }
}
