using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.LimsDictionary;

public struct WorklistState
{
    public const int New = 1;
    public const int Loaded = 2;
    public const int Done = 3;
    public const int Closed = 4;
}

public struct WorklistSendRemoteState
{
    public const Int32 ALL = 0;
    public const Int32 READY = 1;
    public const Int32 SENT = 2;
}

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{      
    public class WorkList : BaseObject
    {
        public WorkList()
        {
            Positions = new WorklistPositionSet();
            Tests = new List<WorklistTest>();
            Rack = new WorklistRackDef();
        }

        /// <summary>
        /// Тип рабочего списка
        /// </summary>
        /// 
        [CSN("WorklistDef")]
        public WorklistDefDictionaryItem WorklistDef { get; set; }
        /// <summary>
        /// Описание штатива/плашки
        /// </summary>
        /// 
        [CSN("Rack")]
        public WorklistRackDef Rack { get; set; }
        /// <summary>
        /// Позиции рабочего списка
        /// </summary>
        /// 
        [CSN("Positions")]
        public WorklistPositionSet Positions { get; set; }
        [CSN("Tests")]
        public List<WorklistTest> Tests { get; set; }
        /// <summary>
        /// Отфильтровывает из проб, находящихся в позициях рабочего списка, работы не входяшие в список tests и опционально отменённые работы
        /// </summary>
        public void FilterWorks(List<TestDictionaryItem> tests, Boolean removeCancelled)
        {
            foreach (var position in Positions)
                position.FilterWorks(tests, removeCancelled);
        }

        public void PrepareForDownload()
        {
            foreach (var p in Positions)
            {
                if (p.PositionDef == null)
                    throw new ArgumentException(String.Format("worklistPosition.PositionDef == null. worklistPosition [X = {0}], [Y = {1}]", p.X, p.Y));

                p.Sample.Works.RemoveAll(work => work.State == WorkState.Approved);
                if (!p.PositionDef.AllTests)
                    p.Sample.Works.RemoveAll(work => !p.PositionDef.Tests.Contains(work.Test));
            }
        }
        /// <summary>
        /// Состояние рабочего списка
        /// </summary>
        /// 
        [CSN("State")]
        public int State { get; set; }
        /// <summary>
        /// Код
        /// </summary>
        /// 
        [CSN("Code")]
        public string Code { get; set; }
        /// <summary>
        /// Статус отправки в "удалённый анализатор"
        /// </summary>
        /// 
        [CSN("SendRemote")]
        public int SendRemote { get; set; }
        /// <summary>
        /// Срок годности
        /// </summary>
        /// 
        [CSN("ExpireDate")]
        public DateTime ExpireDate { get; set; }        
    }

    internal class WorklistAddSamples
    {
        public WorklistAddSamples()
        {
            StartX = 0;
            StartY = 0;
            Samples = new List<ObjectRef>();
        }

        [CSN("StartX")]
        public int StartX { get; set; }
        [CSN("StartY")]
        public int StartY { get; set; }
        [CSN("Samples")]
        public List<ObjectRef> Samples { get; set; }
    }


    internal class WorklistDelta : BaseObject
    {
        public WorklistDelta()
        {
            Code = String.Empty;
            Positions = new WorklistPositionSet();
            AddSamples = new WorklistAddSamples();
            MethodName = String.Empty;
            //ExpireDate
            CalcResult1 = String.Empty;
            CalcResult2 = String.Empty;
            Comment1 = String.Empty;
            Comment2 = String.Empty;
            UserValues = new List<UserValue>();
            Tests = new List<WorklistTest>();
            SkipTests = true;
        }

        [CSN("WorklistDef")]
        public ObjectRef WorklistDef { get; set; }
        [CSN("Rack")]
        public ObjectRef Rack { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Positions")]
        public WorklistPositionSet Positions { get; set; }
        [CSN("AddSamples")]
        public WorklistAddSamples AddSamples { get; set; }
        [CSN("MethodName")]
        public String MethodName { get; set; }
        [CSN("ExpireDate")]
        public DateTime ExpireDate { get; set; }
        [CSN("CalcResult1")]
        public String CalcResult1 { get; set; }
        [CSN("CalcResult2")]
        public String CalcResult2 { get; set; }
        [CSN("Comment1")]
        public String Comment1 { get; set; }
        [CSN("Comment2")]
        public String Comment2 { get; set; }
        [CSN("UserValues")]
        public List<UserValue> UserValues {get; set; }
        [CSN("Tests")]
        public List<WorklistTest> Tests {get; set; }
        [CSN("SkipTests")]
        public bool SkipTests { get; set; }
        [CSN("SendRemote")]
        public int SendRemote { get; set; }
    }
}