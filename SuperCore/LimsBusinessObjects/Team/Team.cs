using System;
using System.Collections.Generic;
using System.Linq;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    /// <summary>
    /// Класс, описывающий бригаду (смену) сотрудников
    /// </summary>
    public class Team : BaseObject
    {
        public Team()
        {
            Template = new TeamTemplateDictionaryItem();
            Members = new List<TeamMember>();
        }

        /// <summary>
        /// Cсылка на элемент справочника "Бригады сотрудников", на основании которго заполнена коллекция "Члены бригады"
        /// </summary>
        [CSN("Template")]
        public TeamTemplateDictionaryItem Template { get; set; }
        /// <summary>
        /// Время открытия смены 
        /// </summary>
        [CSN("Date")]
        public DateTime Date { get; set; }
        /// <summary>
        /// Коллекция, описывающая членов бригады
        /// </summary>
        [CSN("Members")]
        public List<TeamMember> Members { get; set; }


        // Вычисляемые свойства. Отображаются в журнале
        [SendToServer(false)]
        [CSN("TemplateName")]
        public string TemplateName { get { return Template != null ? Template.Name : String.Empty; } }
        [SendToServer(false)]
        [CSN("MemberNames")]
        public string MemberNames
        {
            get
            {
                var names =
                    from member in this.Members
                    let employeeName = member.Employee != null ? member.Employee.Name : String.Empty
                    orderby employeeName
                    select employeeName;

                return String.Join(", ", names);
            }
        }
    }
}