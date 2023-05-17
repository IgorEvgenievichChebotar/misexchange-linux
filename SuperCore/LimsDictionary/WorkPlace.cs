using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.HemDictionary;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class WorkPlaceDictionaryItem : DictionaryItem
    {
        public WorkPlaceDictionaryItem()
        {
            Equipments = new List<EquipmentDictionaryItem>();
            Apparatuses = new List<ApparatusDictionaryItem>();
            ClientId = String.Empty;
        }

        [CSN("Organization")]
        public OrganizationDictionaryItem Organization { get; set; } // Организация рабочего места

        [CSN("ClientId")]
        public String ClientId { get; set; } // Зашифрованная информация о рабочем месте
        [CSN("Equipments")]
        public List<EquipmentDictionaryItem> Equipments { get; set; } // Анализаторы, подключенные к рабочему месту (LIMS)
        [CSN("Apparatuses")]
        public List<ApparatusDictionaryItem> Apparatuses { get; set; } // Анализаторы, подключенные к рабочему месту (HEM)

        /*
    property OrganizationId: Integer read FOrganizationId write FOrganizationId; // Ссылка на учреждение
    property UserGroups: TlisIdList read FUserGroups; // Группы пользоватетей, у которых есть доступ к рабочему месту
         */
    }

    public class WorkPlaceDictionary : DictionaryClass<WorkPlaceDictionaryItem>
    {
        public WorkPlaceDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("WorkPlace")]
        public List<WorkPlaceDictionaryItem> WorkPlace
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
