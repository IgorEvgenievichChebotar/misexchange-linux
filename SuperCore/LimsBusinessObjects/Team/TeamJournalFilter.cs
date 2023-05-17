using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class TeamJournalFilter : BaseJournalFilter
    {
        public TeamJournalFilter()
        {
            DateFrom = DateTime.Now.StartOfTheDay();
            DateTill = DateTime.Now.EndOfTheDay();
            Roles = new List<ObjectRef>();
            Templates = new List<ObjectRef>();
        }

        /// <summary>
        /// Дата открытия смены "с"
        /// </summary>
        [CSN("DateFrom")]
        public DateTime? DateFrom { get; set; }
        /// <summary>
        /// Дата открытия смены "по"
        /// </summary>
        [CSN("DateTill")]
        public DateTime? DateTill { get; set; }
        /// <summary>
        /// Имя сотрудника
        /// </summary>
        [CSN("FirstName")]
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия сотрудника
        /// </summary>
        [CSN("LastName")]
        public string LastName { get; set; }
        /// <summary>
        /// Отчество сотрудника
        /// </summary>
        [CSN("MiddleName")]
        public string MiddleName { get; set; }
        /// <summary>
        /// Множество ссылок на роль сотрудника
        /// </summary>
        [CSN("Roles")]
        public List<ObjectRef> Roles { get; set; }
        /// <summary>
        /// Множество ссылок на роль сотрудника
        /// </summary>
        [CSN("Templates")]
        public List<ObjectRef> Templates { get; set; }

        public override void Clear()
        {
            DateFrom = DateTime.Now.StartOfTheDay();
            DateTill = DateTime.Now.EndOfTheDay();
            FirstName = LastName = MiddleName = String.Empty;
            Roles.Clear();
            Templates.Clear();
        }

        public override void PrepareToSend()
        {
            base.PrepareToSend();
        }
    }
}
