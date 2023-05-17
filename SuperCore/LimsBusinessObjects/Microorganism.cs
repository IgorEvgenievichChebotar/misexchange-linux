using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class Microorganism: BaseObject
    {
        public Microorganism()
        {
        }

        [CSN("MicroOrganism")]
        public MicroOrganismDictionaryItem MicroOrganism { get; set; }

        [CSN("Sample")]
        public ObjectRef Sample { get; set; }

        [CSN("Found")]
        public Boolean Found { get; set; }

        [CSN("Value")]
        public String Value { get; set; }

    }
}
