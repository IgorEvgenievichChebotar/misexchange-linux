using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class Quota : BaseObject
    {
        public Quota()
        {
            Services = new List<QuotaService>();            
        }

        /// <summary>
        /// Ссылка на заказчика
        /// </summary>
        [SendAsRef(true)]
        [CSN("Hospital")]
        public HospitalDictionaryItem Hospital { get; set; }
        /// <summary>
        /// Дата начала 
        /// </summary>
        [CSN("StartDate")]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Дата окончания 
        /// </summary>
        [CSN("EndDate")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Номер договора с заказчиком
        /// </summary>
        [CSN("ContractNumber")]
        public String ContractNumber { get; set; }
        /// <summary>
        /// Максимальная сумма квоты (в рублях) 
        /// </summary>
        [CSN("MaxSum")]
        public double MaxSum { get; set; }
        /// <summary>
        /// От 0 до 100. Процент исчерпания квоты, при котором выдавать предупреждение регистратору 
        /// </summary>
        [CSN("NotifyPercentage")]
        public int NotifyPercentage { get; set; }
        /// <summary>
        /// Максимальный уровень расхода квоты по услугам (если расход любой из услуг превысит это значение, квота считается частично исчерпанной). Минимальное значение поля - 100 (%) 
        /// </summary>
        [CSN("MaxPercentage")]
        public int MaxPercentage { get; set; }
        /// <summary>
        /// Статус квоты (статический справочник): 0 - не исчерпана, 1 - приближается к пределу, 2 - исчерпана, 3 - просрочена
        /// </summary>
        [CSN("State")]
        public QuotaStateDictionaryItem State { get; set; }
        /// <summary>
        /// Детализация по услугам
        /// </summary>
        [CSN("Services")]
        public List<QuotaService> Services { get; set; }
        /// <summary>
        /// Комментарий
        /// </summary>
        [CSN("Comment")]
        public String Comment { get; set; }
        /// <summary>
        /// Признак удаления
        /// </summary>
        [CSN("Removed")]
        public bool Removed { get; set; }
        /// <summary>
        /// Признак "активна"
        /// </summary>
        [SendToServer(false)]
        [CSN("Active")]
        public bool Active { get; set; }
        /// <summary>
        /// Вид контроля (статический справочник): 1 - по количеству исследований, 2 - по максимальной сумме, 3 - по сумме и по номенклатуре услуг
        /// </summary>
        [CSN("ControlType")]
        public QuotaControlTypeDictionaryItem ControlType { get; set; }
        /// <summary>
        /// Израсходованная сумма (по всем услугам). Нередактируемое поле, вычисляется сервером как сумма всех spentSum подобъектов "service"
        /// </summary>
        [SendToServer(false)]
        [CSN("SpentSum")]
        public double SpentSum { get; set; }
        /// <summary>
        /// Остаток суммы. Нередактируемое поле, вычисляется сервером как разность между maxSum и spentSum
        /// </summary>
        [SendToServer(false)]
        [CSN("RestSum")]
        public double RestSum { get; set; }
        /// <summary>
        /// Есть расход по квоте (невидимое нередактируемое поле, вычислется сервером). Если есть хотя бы 1 резерв услуги, ссылающийся на квоту, то значение = true
        /// </summary>
        [SendToServer(false)]
        [CSN("HasExpense")]
        public bool HasExpense { get; set; }
        
        [SendToServer(false)]
        [CSN("ActiveState")]
        public string ActiveState
        {
            get
            {
                if (Active)
                    return "Активна";
                else
                    return "Неактивна";
            }
        }

        [SendToServer(false)]
        [CSN("RemoveState")]
        public string RemoveState
        {
            get
            {
                string result = String.Empty;
                if (Removed)
                    result = "Удалена";
                return result;
            }
        }

        [SendToServer(false)]
        [CSN("StateName")]
        public string StateName
        {
            get
            {
                string result = String.Empty;
                if (State != null)
                    result = State.Name;
                return result;
            }
        }

        [SendToServer(false)]
        [CSN("ControlTypeName")]
        public string ControlTypeName
        {
            get
            {
                string result = String.Empty;
                if (ControlType != null)
                    result = ControlType.Name;
                return result;                
            }
        }        
    }

    public class QuotaService : BaseObject
    {
        /// <summary>
        /// Ссылка на услугу
        /// </summary>
        [SendAsRef(true)]
        [CSN("Service")]
        public ServiceDictionaryItem Service { get; set; }
        /// <summary>
        /// Максимальное кол-во услуг 
        /// </summary>
        [CSN("MaxCount")]
        public int? MaxCount { get; set; }
        /// <summary>
        /// Кол-во израсходованных услуг !!! Вычисляется, хранится сервером, не редактируется в клиенте (и не отправляется при сохранении) 
        /// </summary>
        [SendToServer(false)]
        [CSN("SpentCount")]
        public int? SpentCount { get; set; }
        /// <summary>
        /// Израсходованная сумма !!! Вычисляется, хранится сервером, не редактируется в клиенте (и не отправляется при сохранении) 
        /// </summary>
        [SendToServer(false)] 
        [CSN("SpentSum")]
        public double SpentSum { get; set; }
        /// <summary>
        /// Искусственное свойство. Признак того, что услуга добавлена из бизнес-объекта Quota, а не взята из справочника.
        /// (необходимо для загрузки сетки услуг уже существующей квоты с типом контроля - по сумме и по номенклатуре услуг)
        /// </summary>
        [SendToServer(false)]
        [CSN("IsQuotaService")]
        public bool IsQuotaService { get; set; }
    }
}
