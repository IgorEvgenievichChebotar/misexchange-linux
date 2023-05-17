using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    /// <summary>
    /// Summary description for dailyReport
    /// </summary>
    /// 

    // Объект  "Элемент отчета"
    public class DailyReportItem
    {
        public ObjectRef productClassification = new ObjectRef(); //ссылка на номенклатуру продукта
        public int quantity = 0; // количество единиц продукта на ОПК
        public List<BloodParameterValue> bloodParameters = new List<BloodParameterValue>();
    }

    // Объект "Ежедневный отчет"
    public class dailyReport
    {
        public int id = 0; //уникальный идентификатор
        public DateTime date; // дата создания
        public ObjectRef department = new ObjectRef(); // ОПК
        public List<DailyReportItem> stock = new List<DailyReportItem>(); //  список продуктов, находящихся на ОПК, состоит из объектов следующего вида.
    }

    // Запрос на поиск отчетов
    public class FindDailyReportRequest
    {
        public FindDailyReportRequest()
        {
        }

        public List<ObjectRef> department = new List<ObjectRef>(); //список ОПК
        public DateTime dateFrom; // начальная дата поиска
        public DateTime dateTill; // конечная дата поиска
    }

    // Запрос на чтение одного отчета
    public class GetDailyReportRequest
    {
        public int id = 0;
    }
}
