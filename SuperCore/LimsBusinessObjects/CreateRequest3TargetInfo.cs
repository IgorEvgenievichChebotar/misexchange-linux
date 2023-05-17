using Newtonsoft.Json;
using ru.novolabs.ExchangeDTOs;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class CreateRequest3TargetInfo
    {
        public CreateRequest3TargetInfo()
        {
            Target = new TargetDictionaryItem();
            Parents = new List<TargetDictionaryItem>();
            Tests = new List<TestDictionaryItem>();
            Parents = new List<TargetDictionaryItem>();
            Priority = 0;
        }

        [CSN("Target")]
        [DTOv2(dictionaryName: LimsDictionaryNames.Target, codeField: "Code", canCreate: false)]
        public TargetDictionaryItem Target { get; set; }

        [CSN("Parents")]
        public List<TargetDictionaryItem> Parents { get; set; }

        [CSN("Cancel")]
        public Boolean? Cancel { get; set; }

        [CSN("Priority")]
        [DTOv2(field: "Priority")]
        public Int32? Priority { get; set; }

        [CSN("Readonly")]
        [DTOv2(field: "ReadOnly")]
        public Boolean? Readonly { get; set; }

        [CSN("Tests")]
        public List<TestDictionaryItem> Tests { get; set; }

        private bool p_cito { get; set; }

        [CSN("Cito")]
        public bool? Cito
        {
            get
            {
                try
                {
                    // Костыль, добавленный для обратной совместимости .NET-приложений со старыми серверами ЛИС. После внедрения версий ЛИС старше 2.21.23.0
                    // у всех заказчиков костыль можно ликвидировать
                    if (ProgramContext.Settings == null)
                        return null;
                    bool? enableCito = (bool?)ProgramContext.Settings["enableCitoPropertyForTargetAndSampleInfo", false] ?? false;
                    if (enableCito != null && enableCito.Value)
                    {
                        if (Priority == 1) return true;
                        else return false;
                    }
                    return null;
                }
                catch { return null; }
            }

            set
            {
                bool? enableCito = (bool?)ProgramContext.Settings["enableCitoPropertyForTargetAndSampleInfo", false] ?? false;
                if (enableCito != null && enableCito.Value)
                {
                    if (value.HasValue && value == true)
                        Priority = 1;
                    else
                        Priority = 0;
                }
            }
        }

        [CSN("OriginalPrice")]
        [SendToServer(false)]
        [JsonIgnore]
        public double? OriginalPrice { get; set; }

        [CSN("Price")]
        [SendToServer(false)]
        [JsonIgnore]
        public double? Price { get; set; }

        [CSN("DiscountInRub")]
        [SendToServer(false)]
        [JsonIgnore]
        public double? DiscountInRub { get; set; }

        [CSN("State")]
        [SendToServer(false)]
        [JsonIgnore]
        public int? State { get; set; }
    }
}