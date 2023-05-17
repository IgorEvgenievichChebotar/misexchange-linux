using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{    
    public class PcrTestRef : BaseObject
    {
        [CSN("Rank")]
        public int Rank { get; set; }
      //  public Object Object { get; set; } // ObjectId
    }

    public class PcrTest : BaseObject
    {
        /// <summary>
        /// Ранг. Соответствует индексу теста в рабочем списке
        /// </summary>
        /// 
        [CSN("Rank")]
        public int Rank { get; set; }
        // property TestId: Integer read GetTestId write SetTestId; Мэп на Id
    }

    /// <summary>
    /// Страница в ПЦР штативе расстановки
    /// </summary>
    public class PcrPage
    {
        public PcrPage()
        {
            Samples = new List<PcrSample>();
            Tests = new List<PcrTest>();
            Positions = new PcrPositionSet();
        }

        [CSN("Rank")]
        public int Rank { get; set; }
        [CSN("Samples")]
        public List<PcrSample> Samples { get; set; }
        [CSN("Tests")]
        public List<PcrTest> Tests { get; set; }
        [CSN("Positions")]
        public PcrPositionSet Positions {get; set;}
    }

    public class PcrPageSet : List<PcrPage>
    {
        public PcrPageSet() : base() { }

        public PcrPageSet(int capacity) : base(capacity) { }
    }
    
    
    /// <summary>
    /// Проба/контроль в ПЦР-штативе расстановки
    /// </summary>
    public class PcrSample : BaseObject
    {
        public PcrSample()
        {
            Sample = new BaseSample();
        }
        [CSN("Rank")]
        public int Rank { get; set; }
        [CSN("Sample")]
        public BaseSample Sample { get; set; }

       /* public Boolean IsControl()
        {
            return false; 
        }

        public Boolean IsEmpty()
        {
            return false;
        }*/
    }    


    /// <summary>
    /// Позиция в ПЦР-штативе раскапывания
    /// </summary>
    public class PcrPosition : BaseObject
    {
        public PcrPosition()
        {
            Sample = new PcrSample();
        }

        /// <summary>
        /// Координата по горизонтали
        /// </summary>
        /// 
        [CSN("X")]
        public int X { get; set; }
        /// <summary>
        /// Координата по вертикали
        /// </summary>
        /// 
        [CSN("Y")]
        public int Y { get; set; }
        /// <summary>
        /// Индекс в плашке
        /// </summary>
        /// 
        [CSN("AbsIndex")]
        public int AbsIndex { get; set; }
        /// <summary>
        /// Проба, находящаяся в данной позиции
        /// </summary>
        /// 
        [CSN("Sample")]
        public PcrSample Sample { get; set; }
    }


    public class PcrPositionSet : List<PcrPosition>
    {
        public PcrPositionSet() : base() { }

        public PcrPositionSet(int capacity) : base(capacity) { }
    }

    /// <summary>
    /// Все данные о штативе ПЦР
    /// </summary>
    public class PcrWorklist : BaseObject
    {
        public PcrWorklist()
        {
            WorklistPositions = new WorklistPositionSet();
            Tests = new List<PcrTestRef>();
            Pages = new PcrPageSet();  
        }

        public void Clear()
        {
            Tests.Clear();
            Pages.Clear();
            WorklistPositions.Clear();
        }
        [CSN("Tests")]
        public List<PcrTestRef> Tests { get; set; }
        [CSN("Pages")]
        public PcrPageSet Pages { get; set; }
        [CSN("WorklistPositions")]
        public WorklistPositionSet WorklistPositions { get; set; }

        public void FillPositions(PcrPositionSet positions)
        {
            throw new NotImplementedException();
        }

        public void Prepare(WorkList worklist)
        {
            throw new NotImplementedException();
           // WorklistDefDictionaryItem worklistDef = 

//====================================================================================================
            /*

procedure TlisPcrWorklistData.Prepare(Worklist: TlisWorklistData);
var
  WorklistDef: TlisWorklistDef;
  TestRegions: TlisPcrTestSet;
begin
  Clear;

  WorklistDef := Worklist.WorklistDef;
  FWorklistDefId := Worklist.WorklistDefId;
  FRackDefId := Worklist.RackId;
  SamplesPerPage := WorklistDef.PatientsCount;
  TestsPerRack := WorklistDef.DividerPosition;
  ControlsCount := Worklist.Rack.PcrControls.CalcExisitngItemCount;

  // 0. Сохраняем имеющиеся позиции.
  WorklistPositions.CloneFrom(Worklist.Positions);
  TlisPcrTestSet.PreparePositions(WorklistDef, Worklist.Rack, WorklistPositions);

  TestRegions := TlisPcrTestSet.Create;
  try
    // 1. Разбиваем рабочий список на регионы, соответствующие одному тесту. Граница региона - либо разные тесты,
    //    либо контроль - не контроль.
    TestRegions.Prepare(Worklist);
    // 2. Разносим регионы по страницам.
    Pages.Prepare(TestRegions, SamplesPerPage, ControlsCount);
  finally
    TestRegions.Free;
  end;

  // 3. Инициализируем все тесты, как в рабочем списке, так и на каждой странице. На всех страницах одинаковый набор
  //    тестов.
  PrepareTests(Worklist.Tests, Pages);
  // 4. Инициализируем пробы для заголовков на всех страницах.
  PrepareSamples(WorklistDef, Worklist.Rack, Pages);
  // 5. Инициализируем позиции на всех страницах. X и Y для позиции всегда в рамках одной позиции.
  Pages.PreparePositions(WorklistDef, WorklistDef.PatientsCount);

  UpdateWidth;
  Height := WorklistDef.TestsCount;
end;

*/
        }
    }
}