using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class TargetMap: BaseObject
    {
        /// <summary>
        /// Время создания записи
        /// </summary>
        [CSN("Date")]
        public DateTime Date { get; set; }

        [CSN("TargetLocations")]
        public List<TargetLocation> TargetLocations { get; set; }
        
    }

    public class TargetLocation : BaseObject
    {
        /// <summary>
        /// Ссылка на исследование
        /// </summary>
        [CSN("Target")]
        public TargetDictionaryItem Target { get; set; }

        /// <summary>
        /// Ссылка на аутсорсера
        /// </summary>
        [CSN("Outsourcer")]
        public OutsourcerDictionaryItem Outsourcer { get; set; }

        /// <summary>
        /// Ссылка на подразделение
        /// </summary>
        [CSN("Department")]
        public DepartmentDictionaryItem Department { get; set; }
    }

    public class TargetMapVersionResponse
    {
        /// <summary>
        /// Максимально возможное значение
        /// </summary>
        [CSN("Id")]
        public Int32 Id { get; set; }
    }
}
