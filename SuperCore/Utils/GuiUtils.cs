//using System.Drawing;
//using System.Windows.Forms;

namespace ru.novolabs.SuperCore
{
    /*public class NlsCustomToolStripRenderer : ToolStripSystemRenderer
    {
        // Высота нижней полоски-артефакта, цвет которой не меняется при изменении дефолтного цвета ToolStrip-а = 2px (проверял на двух дисплеях с разным разрешением)
        private const int StupidStripHeight = 2; 

        public NlsCustomToolStripRenderer() { }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBorder(e);
            Brush brush = new SolidBrush(e.ToolStrip.BackColor);
            e.Graphics.FillRectangle(brush, e.AffectedBounds.Left, e.AffectedBounds.Bottom - StupidStripHeight, e.AffectedBounds.Width, StupidStripHeight);
        }
    }*/
}