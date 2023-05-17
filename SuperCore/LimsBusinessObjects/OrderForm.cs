using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using ru.novolabs.SuperCore.LimsDictionary;
using System.Linq;
using System.Collections;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class OrderFormLayout
    {
        public OrderFormLayout()
        {
            Pages = new OrderFormLayoutPageList();
        }
        [CSN("Pages")]
        public OrderFormLayoutPageList Pages { get; set; }
    }

    public class OrderFormLayoutPageList : List<OrderFormLayoutPage>
    {
        public OrderFormLayoutPage AddPage()
        {
            var newPage = new OrderFormLayoutPage();
            base.Add(newPage);
            newPage.PageIndex = this.IndexOf(newPage);
            newPage.Caption = "Страница " + newPage.PageIndex.ToString();
            return newPage;
        }
    }

    public class OrderFormLayoutPage
    {
        public OrderFormLayoutPage()
        {
            OrderForm = new OrderForm();
        }

        [DisplayName("Порядковый номер")]
        [CSN("PageIndex")]
        public int PageIndex { get; set; }
        [DisplayName("Заголовок")]
        [CSN("Caption")]
        public String Caption { get; set; }
        [Browsable(false)]
        [CSN("OrderForm")]
        public OrderForm OrderForm { get; set; }
    }

    [XmlRoot("nlsOrderForm")]
    public class OrderForm : TableForm<OrderFormCell, OrderFormCellContent>
    {
        [CSN("ImageResource")]
        public String ImageResource { get; set; }
        [CSN("ImageWidth")]
        public Int32 ImageWidth { get; set; }
        [CSN("ImageHeight")]
        public Int32 ImageHeight { get; set; }
        [CSN("WriteUserCaptions")]
        public Boolean WriteUserCaptions { get; set; }
        [CSN("TextStyles")]
        public String TextStyles { get; set; }
        [CSN("ActiveCellCSS")]
        public String ActiveCellCSS { get; set; }
        [CSN("CaptionCellCSS")]
        public String CaptionCellCSS { get; set; }

        public void SortCells(Int32 delta)
        {
            Dictionary<Int32, List<OrderFormCell>> cols = new Dictionary<int, List<OrderFormCell>>();

            foreach (OrderFormCell cell in Cells)
            {
                Int32 key = cell.Left;
                List<OrderFormCell> col;
                try
                {  
                    col = cols.First(p => Math.Abs(p.Key - cell.Left) < delta).Value; 
                }
                catch
                { 
                    Log.WriteText(String.Format("Left = {0}; Key = {1}", cell.Left, key));
                    cols.Add(key, new List<OrderFormCell>());
                    col = cols[key];
                }

                
                col.Add(cell);                                
            }


            List<Int32> keys = new List<Int32>(cols.Keys);
            keys.Sort((a, b) => a.CompareTo(b));

            Int32 x = 0;
            RowCount = 0;

            foreach(List<OrderFormCell> col in cols.Values)
            {
                col.Sort((a, b) => a.Top.CompareTo(b.Top));
                for (Int32 y = 0; y < col.Count; y++)
                {
                    OrderFormCell cell = col[y];
                    cell.X = x;
                    cell.Y = y;
                    if (String.IsNullOrEmpty(cell.CellId))
                        cell.CellId = "_" + x.ToString() + "_" + y.ToString() + "_";

                    RowCount = y > RowCount ? y : RowCount;
                }
                x++;
            }
            ColCount = x;

            foreach (TableFormRow row in Rows)
                row.Height = 40;

            foreach (TableFormColumn col in Columns)
                col.Width = 800 / ColCount;
        }
    }

    public class OrderFormCell : TableFormCell<OrderFormCellContent> 
    {
        public OrderFormCell()
        {
            Content = new OrderFormCellContent();
        }

        [CSN("Left")]
        [CategoryAttribute("Координаты")]
        [SendToServer(false)]
        public Int32 Left { get; set; }
        [CSN("Top")]
        [SendToServer(false)]
        [CategoryAttribute("Координаты")]
        public Int32 Top { get; set; }
        [CSN("Right")]
        [SendToServer(false)]
        [CategoryAttribute("Координаты")]
        public Int32 Right { get; set; }
        [CSN("Bottom")]
        [SendToServer(false)]
        [CategoryAttribute("Координаты")]
        public Int32 Bottom { get; set; }
    }

    public class LinkedCell
    {
        [CSN("Id")]
        public Int32 Id { get; set; }
        [CSN("BiomaterialCode")]
        public String BiomaterialCode { get; set; }
        [CSN("TargetCode")]
        public String TargetCode { get; set; }
    }

    public class OrderFormCellContent
    {
        private List<BiomaterialDictionaryItem> biomaterials;
        public OrderFormCellContent()
        {
            LinkedCells = new List<LinkedCell>();
            Extentions = new Dictionary<string, string>();
            BiomaterialCodes = new List<string>();
            
        }

        [BrowsableAttribute(false)]
        [XmlIgnore]
        [CSN("Extentions")]
        public Dictionary<string, string> Extentions { get; set; }

        [BrowsableAttribute(false)]
        [CSN("Id")]
        public Int32 Id { get; set; }
        [CategoryAttribute("Внешний вид")]
        [CSN("Caption")]
        public String Caption { get; set; }
        [CSN("UserCaption")]
        public String UserCaption { get; set; }
        [CategoryAttribute("Внешний вид")]
        [CSN("CanCheck")]
        public Boolean CanCheck { get; set; }
        [Browsable(false)]
        [CSN("Biomaterials")]
        [XmlIgnore]
        public List<BiomaterialDictionaryItem> Biomaterials 
        {
            get { return biomaterials; }
            set 
            { 
                biomaterials = value;
                if (biomaterials != null)
                {
                    BiomaterialCodes =
                        (from biomaterial in biomaterials
                         select biomaterial.Code).ToList();
                }
            }
        }
        [BrowsableAttribute(false)]
        [CSN("BiomaterialCodes")]
        [XmlArrayItem(ElementName = "BiomaterialCode")]
        public List<String> BiomaterialCodes { get; set; }

        [CategoryAttribute("Служебные данные")]
        [BrowsableAttribute(true)]
        [CSN("ProtoString")]
        public String ProtoString { get; set; }

        [BrowsableAttribute(true)]
        [CSN("_BiomaterialCodes")]
        [XmlIgnore]
        public String _BiomaterialCodes { get { return ((BiomaterialCodes == null) || (BiomaterialCodes.Count < 1)) ? String.Empty : String.Join(",", BiomaterialCodes); } set { } }
        [BrowsableAttribute(true)]
        [CSN("TargetCode")]
        public String TargetCode { get; set; }
        [Browsable(false)]
        [CSN("DisplayTargetName")]
        [SendToServer(false)]
        public String DisplayTargetName { get; set; }

        [CSN("DisplayBiomaterialName")]
        [SendToServer(false)]
        public String DisplayBiomaterialName { get; set; }

        //Координаты списка
        [CSN("ListLeft")]
        public Int32 ListLeft { get; set; }
        [CSN("ListRight")]
        public Int32 ListRight { get; set; }
        [CSN("ListTop")]
        public Int32 ListTop { get; set; }
        [CSN("ListBottom")]
        public Int32 ListBottom { get; set; }

        //Координаты кнопки
        [CSN("ButtonLeft")]
        public Int32 ButtonLeft { get; set; }
        [CSN("ButtonRight")]
        public Int32 ButtonRight { get; set; }
        [CSN("ButtonTop")]
        public Int32 ButtonTop { get; set; }
        [CSN("ButtonBottom")]
        public Int32 ButtonBottom { get; set; }

        [BrowsableAttribute(false)]
        [CSN("LinkedCells")]
        public List<LinkedCell> LinkedCells { get; set; }
        [CSN("BiomaterialsDisplayInfo")]
        [XmlIgnore]
        [Browsable(true)]
        [ReadOnly(true)]
        [DisplayName("Биоматериалы")]
        public String BiomaterialsDisplayInfo
        {
            get
            {
                if ((biomaterials != null) && (biomaterials.Count > 0))
                {
                    List<String> bmInfos =
                        (from bm in biomaterials
                         select String.Format("{0} [{1}]", bm.Name, bm.Code)).ToList();

                    return String.Join(",", bmInfos);
                }
                else
                    return String.Join(",", BiomaterialCodes);
            }
        }

        public override string ToString()
        {
            return String.Empty;
        }
    }
}