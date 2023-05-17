using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class TableFormCellList<T1, T2> : List<T1>
        where T1 : TableFormCell<T2>
    {
        public T1 this[Int32 x, Int32 y]
        {
            get
            {
                foreach (T1 cell in this)
                {
                    if ((cell.X == x) && (cell.Y == y))
                        return cell;
                }
                return null;
            }
        }
    }

    public class TableFormRowList : List<TableFormRow>
    {
        public new TableFormRow this[Int32 y]
        {
            get
            {
                foreach (TableFormRow row in this)
                {
                    if (row.Y == y)
                        return row;
                }
                return null;
            }
        }
    }

    public class TableFormColumnList : List<TableFormColumn>
    {
        public new TableFormColumn this[Int32 x]
        {
            get
            {
                foreach (TableFormColumn column in this)
                {
                    if (column.X == x)
                        return column;
                }
                return null;
            }
        }
    }

    public class TableForm<T, U>
        where T : TableFormCell<U>
    {
        public TableForm()
        {
            Cells = new TableFormCellList<T, U>();
            Rows = new TableFormRowList();
            Columns = new TableFormColumnList();
            ColWidth = 150; // default width
            RowHeight = 25; // default height
        }

        [BrowsableAttribute(false)]
        [CSN("Id")]
        public Int32 Id { get; set; }
        [DisplayName("Имя")]
        [CSN("Name")]
        public String Name { get; set; }
        [DisplayName("Код")]
        [CSN("Code")]
        public String Code { get; set; }
        [DisplayName("Мнемоника")]
        [CSN("Mnemonics")]
        public String Mnemonics { get; set; }

        [BrowsableAttribute(false)]
        [CSN("Removed")]
        public Boolean Removed { get; set; }

        [Category("Колонки")]
        [DisplayName("Колонки")]
#if FIELDSEDITOR
        [Editor(typeof(RowsAndColumnsEditor), typeof(UITypeEditor))]
#endif
        [CSN("Columns")]
        public TableFormColumnList Columns { get; set; }

        [Category("Колонки")]
        [DisplayName("Кол-во колонок")]
        [CSN("ColCount")]
        public Int32 ColCount { get; set; }

        [Category("Строки")]
        [DisplayName("Кол-во строк")]
        [CSN("RowCount")]
        public Int32 RowCount { get; set; }

        [Category("Строки")]
#if FIELDSEDITOR
        [DisplayName("Строки")]
        [Editor(typeof(RowsAndColumnsEditor), typeof(UITypeEditor))]
#endif
        [CSN("Rows")]
        public TableFormRowList Rows { get; set; }

        [Category("Строки")]
        [DisplayName("Высота строк")]
        [CSN("RowHeight")]
        public Int32? RowHeight { get; set; }

        [Category("Колонки")]
        [DisplayName("Ширина колонок")]
        [CSN("ColWidth")]
        public Int32? ColWidth { get; set; }

        [BrowsableAttribute(false)]
        [CSN("BgColor")]
        public Int32? BgColor { get; set; }
        [BrowsableAttribute(false)]
        [CSN("LineColor")]
        public Int32? LineColor { get; set; }
        [BrowsableAttribute(false)]
        [CSN("Cells")]
        public TableFormCellList<T, U> Cells { get; set; }

        // Свойства нужны только для редактора propertyGrid
        [XmlIgnore]
        [Category("Внешность")]
        [DisplayName("Цвет фона")]
        [CSN("_BgColor")]
        public System.Drawing.Color _BgColor
        {
            get { return BgColor != null ? BgColor.Value.ToARgbColor() : SystemColors.Control; }
            set { BgColor = value.ToARgb(); }
        }

        [XmlIgnore]
        [Category("Внешность")]
        [DisplayName("Цвет обводки")]
        [CSN("_LineColor")]
        public System.Drawing.Color _LineColor
        {
            get { return LineColor != null ? LineColor.Value.ToRgbColor() : SystemColors.Control; }
            set { LineColor = value.ToRgb(); }
        }

        private void RefreshColumns()
        {
            if (this.ColCount > Columns.Count)
            {
                int delta = this.ColCount - Columns.Count;
                for (int i = 0; i < delta; i++)
                    Columns.Add(new TableFormColumn() { X = Columns.Count, Width = this.ColWidth });
            }
            else if (this.ColCount < Columns.Count)
                Columns.RemoveAll(item => item.X >= this.ColCount);
        }

        private void RefreshRows()
        {
            if (this.RowCount > Rows.Count)
            {
                int delta = this.RowCount - Rows.Count;
                for (int i = 0; i < delta; i++)
                    Rows.Add(new TableFormRow() { Y = Rows.Count, Height = this.RowHeight });
            }
            else if (this.RowCount < Rows.Count)
                Rows.RemoveAll(item => item.Y >= this.RowCount);
        }

        public void RefreshStructure()
        {
            RefreshColumns();
            RefreshRows();
            RefreshCells();
        }

        private void RefreshCells()
        {
            int newCellCount = this.RowCount * this.ColCount;
            if (newCellCount > this.Cells.Count)
            {
                for (int i = 0; i < this.ColCount; i++)
                {
                    for (int j = 0; j < this.RowCount; j++)
                    {
                        if (this.Cells[i, j] == null)
                        {
                            var cell = (T)Activator.CreateInstance(typeof(T));
                            cell.X = i;
                            cell.Y = j;
                            cell.Appearance.BgColor = this.BgColor;
                            cell.Appearance.LineColor = this.LineColor;
                            this.Cells.Add(cell);
                        }
                    }

                }
            }

            Cells.RemoveAll(c => c.X >= this.ColCount || c.Y >= this.RowCount);
        }

        public void RefreshAppearance()
        {
            foreach (T cell in this.Cells)
            {
                cell.Appearance.BgColor = this.BgColor;
                cell.Appearance.LineColor = this.LineColor;
            }
        }
    }

    public class TableFormRow
    {
        public TableFormRow()
        {
            Height = 25; /* default height */
        }

        [BrowsableAttribute(false)]
        [CSN("Id")]
        public Int32 Id { get; set; }
        //  [BrowsableAttribute(false)]
        [CSN("Y")]
        public Int32? Y { get; set; }
        [CSN("Height")]
        public Int32? Height { get; set; }

        public override string ToString()
        {
            return "Row " + (Y ?? 0).ToString();
        }
    }

    public class TableFormColumn
    {
        public TableFormColumn()
        {
            Width = 150; /* default width */
        }

        [BrowsableAttribute(false)]
        [CSN("Id")]
        public Int32 Id { get; set; }
        // [BrowsableAttribute(false)]
        [CSN("X")]
        public Int32? X { get; set; }
        [CSN("Width")]
        public Int32? Width { get; set; }

        public override string ToString()
        {
            return "Column " + (X ?? 0).ToString();
        }
    }

    public class TableFormCellAppearance
    {
        [BrowsableAttribute(false)]
        [CSN("Id")]
        public Int32 Id { get; set; }
        [BrowsableAttribute(false)]
        [CSN("BgColor")]
        public Int32? BgColor { get; set; }

        private Font font;
        [XmlIgnore]
        [CSN("Font")]
        public Font Font {
            get { return font; }
            set 
            { 
                font = value;
                FontFamily = font.FontFamily.Name;
                FontSize = (Int32)font.Size;
                Bold = font.Bold;
                Italic = font.Italic;
                Underline = font.Underline;
            }
        }


       // [DisplayName("Шрифт. Семейство")]
        [CSN("FontFamily")]
        
        public String FontFamily { get; set; }
        [BrowsableAttribute(false)]
        [CSN("LineColor")]
        public Int32? LineColor { get; set; }
        [BrowsableAttribute(false)]
        [CSN("FontColor")]
        public Int32? FontColor { get; set; }

        [Category("Шрифт")]
       // [DisplayName("Шрифт. Размер")]
        [CSN("FontSize")]
        public Int32? FontSize { get; set; }
        
      //  [DisplayName("Шрифт жирный")]
        [CSN("Bold")]
        public Boolean? Bold { get; set; }

       // [DisplayName("Шрифт наклонный")]
        [CSN("Italic")]
        public Boolean? Italic { get; set; }

      //  [DisplayName("Шрифт подчёркнутый")]
        [CSN("Underline")]
        public Boolean? Underline { get; set; }

        [CSN("Uppercase")]
        public Boolean? Uppercase { get; set; }

        [BrowsableAttribute(false)]
        [CSN("ImageIndex")]
        public Int32? ImageIndex { get; set; }

        public override string ToString()
        {
            return String.Empty;
        }

        public void Clear()
        {
            //Caption = FontFamily = String.Empty; // todo
            BgColor = LineColor = FontColor = FontSize = ImageIndex = null;
            Bold = Italic = Underline = null;
        }

        // Свойства нужны только для редактора propertyGrid
        [XmlIgnore]
        //[DisplayName("Цвет фона")]
        [DisplayName("BgColor")]
        [CSN("_BgColor")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public System.Drawing.Color _BgColor
        {
            get { return BgColor != null ? BgColor.Value.ToARgbColor() : SystemColors.Control; }
            set { BgColor = value.ToARgb(); }
        }

        [XmlIgnore]
        [DisplayName("BgColorTransparency")]
        [CSN("BgColorTransparency")]
        public Byte BgColorTransparency
        {
            get { return _BgColor.A; }
            set { _BgColor = Color.FromArgb(value, _BgColor); }
        }

        [XmlIgnore]
        //   [DisplayName("Цвет обводки")]
        [CSN("_LineColor")]
        [DisplayName("LineColor")]
        public System.Drawing.Color _LineColor
        {
            get { return LineColor != null ? LineColor.Value.ToRgbColor() : SystemColors.Control; }
            set { LineColor = value.ToRgb(); }
        }

        [XmlIgnore]
      //  [DisplayName("Цвет шрифта")]
        [CSN("_FontColor")]
        [DisplayName("FontColor")]
        public System.Drawing.Color _FontColor
        {
            get { return FontColor != null ? FontColor.Value.ToRgbColor() : SystemColors.ControlText; }
            set { FontColor = value.ToRgb(); }
        }
    }

    public class TableFormCell<T>
    {
        public TableFormCell()
        {
            Appearance = new TableFormCellAppearance();
        }

        [CSN("CellId")]
        public String CellId { get; set; }
        [BrowsableAttribute(false)]
        [CSN("Id")]
        public Int32 Id { get; set; }
        [BrowsableAttribute(false)]
        [CSN("X")]
        public Int32 X { get; set; }
        [BrowsableAttribute(false)]
        [CSN("Y")]
        public Int32 Y { get; set; }
        [CSN("ParentId")]
        public String ParentId { get; set; }
        [CSN("ParentX")]
        [BrowsableAttribute(false)]
        public Int32? ParentX { get; set; }
        [CSN("ParentY")]
        [BrowsableAttribute(false)]
        public Int32? ParentY { get; set; }
        [CategoryAttribute("Внешний вид")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [CSN("Appearance")]
        public TableFormCellAppearance Appearance { get; set; }
        [CategoryAttribute("Содержимое")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [CSN("Content")]
        public T Content { get; set; }
    }
}
