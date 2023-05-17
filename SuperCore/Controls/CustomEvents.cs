using System;
using System.Windows.Forms;

namespace ru.novolabs.SuperCore
{
    // Классы аргументов событий
    public class DataGridViewRowGetStyleEventArgs : EventArgs
    {
        public DataGridViewRowGetStyleEventArgs(DataGridViewCellStyle rowStyle, Object rowObject)
        {
            RowStyle = rowStyle;
            RowObject = rowObject;
        }

        public Object RowObject { get; set; }
        public DataGridViewCellStyle RowStyle { get; set; }
    }

    public class DataGridViewSelectedRowChangedEventArgs : EventArgs
    {
        public DataGridViewSelectedRowChangedEventArgs(Object selectedObject)
        {
            SelectedObject = selectedObject;
        }

        public Object SelectedObject { get; set; }
    }

    // Делегаты событий
    public delegate void DataGridViewRowGetStyleEventHandler(object sender, DataGridViewRowGetStyleEventArgs e);

    public delegate void DataGridViewSelectedRowChangedEventHandler(object sender, DataGridViewSelectedRowChangedEventArgs e);
}
