using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;

public class WorklistPositionState
{
    public const int Reserved = 1;
    public const int New = 2;
    public const int Done = 3;
    public const int Approved = 4;
    public const int Empty = 5;
    public const int Cancelled = 6;
    public const int Control = 7;
}

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class WorklistTest : BaseObject
    {
        [CSN("Rank")]
        public Int32 Rank { get; set; }
        [CSN("Test")]
        public TestDictionaryItem Test { get; set; }
    }

    public class WorklistSample : BaseSample { }

    public class WorklistPositionDef : BaseObject
    {
        public WorklistPositionDef()
        {
            Tests = new List<TestDictionaryItem>();
        }

        [CSN("X")]
        public Int32 X { get; set; }
        [CSN("Y")]
        public Int32 Y { get; set; }
        [CSN("Reserved")]
        public Boolean Reserved { get; set; }
        [CSN("AllTests")]
        public Boolean AllTests { get; set; }
        [CSN("ControlLotNr")]
        public String ControlLotNr { get; set; }
        [CSN("Tests")]
        public List<TestDictionaryItem> Tests { get; set; }

        public Boolean IsControl()
        {
            return Reserved && !String.IsNullOrEmpty(ControlLotNr);
        }
    }
    
    public class WorklistPosition : BaseObject
    {
        public WorklistPosition()
        {
            Sample = new WorklistSample();
            PositionDef = new WorklistPositionDef();
        }

        [CSN("X")]
        public Int32 X { get; set; }
        [CSN("Y")]
        public Int32 Y { get; set; }
        [CSN("Reserved")]
        public Boolean Reserved { get; set; }
        [CSN("Sample")]
        public WorklistSample Sample { get; set; }
        [CSN("PositionDef")]
        public WorklistPositionDef PositionDef { get; set; }
        [CSN("ControlLotNr")]
        public String ControlLotNr { get; set; }
        [CSN("State")]
        public Int32 State { get; set; }        
        /// <summary>
        /// тип нумерации: числовая(1), буквенная(2) или римская (3). LIS_NUMERATION_TYPE_xxx
        /// </summary>
        /// 
        [CSN("XNumberingType")]
        public Int32 XNumberingType { get; set; }
        /// <summary>
        /// тип нумерации: числовая(1), буквенная(2) или римская (3). LIS_NUMERATION_TYPE_xxx
        /// </summary>
        /// 
        [CSN("YNumberingType")]
        public Int32 YNumberingType { get; set; }
        [CSN("AbsIndex")]
        public Int32 AbsIndex { get; set; }

        public void FilterWorks(List<TestDictionaryItem> tests, Boolean removeCancelled)
        {
            Sample.Works.RemoveAll(work => !tests.Contains(work.Test));
            if (removeCancelled)
                Sample.Works.RemoveAll(work => work.State == WorkState.Cancelled);
        }
    }

    public class WorklistPositionSet : List<WorklistPosition>
    {
        private const int RACK_FILL_MODE_UP_DOWN = 1;
        private const int RACK_FILL_MODE_LEFT_RIGHT = 2;

        public WorklistPositionSet() : base() { }

        public WorklistPositionSet(int capacity) : base(capacity) { }

        public void Init(WorklistRackDef rack)
        {
            CheckAllPositions(rack);
            UpdatePositionDef(rack);
            UpdateStates();
           // UpdateNorms(); Вряд ли нужно выставлять нормы работам перед отправкой рабочего списка в анализатор. Если всё-таки нужно, то реализовать в будущем
        }

        /// <summary>
        /// В случае отсутствия позиций рабочего списка добавляем их на основе описания позиций штатива
        /// </summary>
        private void CheckAllPositions(WorklistRackDef rack)
        {
            for (int i = 0; i < rack.Width; i++)
            {
                for (int j = 0; i < rack.Height; j++)
                {
                    if (Find(p => (p.X == i) && (p.Y == j)) == null)
                    {
                        var position = new WorklistPosition();
                        position.X = i;
                        position.Y = j;
                        WorklistPositionDef rackPosition = rack.Positions.Find(p => (p.X == i) && (p.Y == j));
                        if (rackPosition != null)
                            position.Reserved = rackPosition.Reserved;

                        Add(position);
                    }
                }
            }
        }

        /// <summary>
        /// Копируем информацию из описания позиций штатива в позиции рабочего списка
        /// </summary>
        private void UpdatePositionDef(WorklistRackDef rack)
        {
            ForEach(item =>
                {
                    WorklistPositionDef rackPositionDef = rack.Positions.Find(p => (p.X == item.X) && (p.Y == item.Y));
                    if (rackPositionDef != null)
                    {
                        if (rackPositionDef.IsControl())
                        {
                            item.ControlLotNr = rackPositionDef.ControlLotNr;
                        }
                    }

                    item.XNumberingType = rack.XNumberingType;
                    item.YNumberingType = rack.YNumberingType;
                });
        }

        private void UpdateStates()
        {
            ForEach(item =>
                {
                    if (item.Reserved)
                    {
                        if ((item.PositionDef != null) && (!String.IsNullOrEmpty(item.PositionDef.ControlLotNr)))
                            item.State = WorklistPositionState.Control;
                        else
                            item.State = WorklistPositionState.Reserved;
                    }
                    else if (item.Sample.Id == 0)
                        item.State = WorklistPositionState.Empty;
                    else if (item.Sample.Works.Count == 0)
                        item.State = WorklistPositionState.Cancelled;
                    else if (item.Sample.Works.Find(w => w.State.Equals(WorkState.New)) != null)
                        item.State = WorklistPositionState.New;


                    else if (item.Sample.Works.Find(w => w.State.Equals(WorkState.Loaded)) != null)
                        item.State = WorklistPositionState.New;
                    else if (item.Sample.Works.Find(w => w.State.Equals(WorkState.Done)) != null)
                        item.State = WorklistPositionState.Done; 
                    else if (item.Sample.Works.Find(w => w.State.Equals(WorkState.Approved)) != null)
                        item.State = WorklistPositionState.Approved;
                    else
                        item.State = WorklistPositionState.Cancelled;
                });
        }

        private int GetAbsPosition(int x, int y, int width, int height, int fillMode)
        {
              if (fillMode == RACK_FILL_MODE_UP_DOWN)
                  return x * height + y;
              else
                  return y * width + x;
        }      
        
        public void UpdateAbsIndex(WorklistRackDef rack)
        {
            ForEach(item => item.AbsIndex = GetAbsPosition(item.X, item.Y, rack.Width, rack.Height, rack.FillMode) + 1);
        }

        public void SortByAbsIndex()
        {
            Sort((WorklistPosition x, WorklistPosition y) => { return x.AbsIndex.CompareTo(y.AbsIndex); });
        }
    }


    public class WorklistRackDef : BaseObject
    {
        public WorklistRackDef()
        {
            Positions = new List<WorklistPositionDef>();
        }


        /// <summary>
        /// количество позиций по горизонтали
        /// </summary>
        /// 
        [CSN("Width")]
        public Int32 Width { get; set; }
        /// <summary>
        /// количество позиций по вертикали
        /// </summary>
        /// 
        [CSN("Height")]
        public Int32 Height { get; set; }
        /// <summary>
        /// позиции штатива
        /// </summary>
        /// 
        [CSN("Positions")]
        public List<WorklistPositionDef> Positions { get; set; }
        /// <summary>
        /// тип нумерации: числовая(1), буквенная(2) или римская (3). LIS_NUMERATION_TYPE_xxx
        /// </summary>
        /// 
        [CSN("XNumberingType")]
        public Int32 XNumberingType { get; set; }
        /// <summary>
        /// тип нумерации: числовая(1), буквенная(2) или римская (3). LIS_NUMERATION_TYPE_xxx
        /// </summary>
        /// 
        [CSN("YNumberingType")]
        public Int32 YNumberingType { get; set; }
        /// <summary>
        /// способ заполнения штатива: сверху-вниз(1) или слева-направо(2). LIS_RACK_FILL_MODE_xxx
        /// </summary>
        /// 
        [CSN("FillMode")]
        public Int32 FillMode { get; set; }
        /// <summary>
        /// код анализатораx
        /// </summary>
        /// 
        [CSN("EquipmentCode")]
        public String EquipmentCode { get; set; }
    }

    [OldSaveMethod]
    public class WorklistDefDictionaryItem : DictionaryItem
    {
        public WorklistDefDictionaryItem()
        {
            Tests = new List<WorklistTest>();
        }
        [CSN("Equipment")]
        public EquipmentDictionaryItem Equipment { get; set; } // Анализатор, c которым связан тип рабочего списка
        [CSN("SendToEquipment")]
        public Boolean SendToEquipment { get; set; } // Опция "Отправка списка на анализатор"
        [CSN("PcrMode")]
        public Boolean PcrMode { get; set; } // Режим ПЦР
        [CSN("Tests")]
        public List<WorklistTest> Tests { get; set; }
    }

    public class WorklistDefDictionary : DictionaryClass<WorklistDefDictionaryItem>
    {
        public WorklistDefDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("WorklistDef")]
        public List<WorklistDefDictionaryItem> WorklistDef
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
