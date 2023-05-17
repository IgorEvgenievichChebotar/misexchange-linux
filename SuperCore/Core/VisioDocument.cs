using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ru.novolabs.SuperCore.Core
{
    public class VisioShape
    {
        public Double PinX { get; set; }
        public Double PinY { get; set; }
        public Double LocPinX { get; set; }
        public Double LocPinY { get; set; }
        public Double Width { get; set; }
        public Double Height { get; set; }
        public Int32? BgColor { get; set; }
        public Int32? LineColor { get; set; }
        public String Caption { get; set; }
        public VisioPage Parent { get; set; }

        public VisioShape(XElement root,  VisioPage parent)
        {
            this.Parent = parent;

            PinX = Double.Parse(root.Descendants("XForm").First().Descendants("PinX").First().Value, CultureInfo.InvariantCulture);
            PinY = Double.Parse(root.Descendants("XForm").First().Descendants("PinY").First().Value, CultureInfo.InvariantCulture);

            LocPinX = Double.Parse(root.Descendants("XForm").First().Descendants("LocPinX").First().Value, CultureInfo.InvariantCulture);
            LocPinY = Double.Parse(root.Descendants("XForm").First().Descendants("LocPinY").First().Value, CultureInfo.InvariantCulture);

            Width = Double.Parse(root.Descendants("XForm").First().Descendants("Width").First().Value, CultureInfo.InvariantCulture);
            Height = Double.Parse(root.Descendants("XForm").First().Descendants("Height").First().Value, CultureInfo.InvariantCulture);

            GetBgColor(root);
            GetLineColor(root);
            GetCaption(root); 
            
        }

        private string NormalizeCaption(string s)
        {
            if (String.IsNullOrEmpty(s)) return String.Empty;
            return s.Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ').Trim();
        }

        private void GetBgColor(XElement root)
        {
            String hex = String.Empty;
            try
            {
                String fillPattern = root.Descendants("Fill").First().Descendants("FillPattern").First().Value;
                if (!String.IsNullOrEmpty(fillPattern) && (fillPattern != "0"))
                {
                    hex = root.Descendants("Fill").First().Descendants("FillForegnd").First().Value.Substring(1);
                    BgColor = Int32.Parse(hex, NumberStyles.HexNumber);
                }
            }
            catch
            {
                BgColor = 0xFFFFFF;
                Log.WriteError(String.Format("Can not parce string to color '{0}'", hex));
            }
        }

        private void GetLineColor(XElement root)
        {
            String hex = String.Empty;
            try
            {
                String linePattern = root.Descendants("Fill").First().Descendants("LinePattern").First().Value;
                if (!String.IsNullOrEmpty(linePattern) && (linePattern != "0"))
                {
                    hex = root.Descendants("Line").First().Descendants("LineColor").First().Value.Substring(1);
                    LineColor = Int32.Parse(hex, NumberStyles.HexNumber);
                }
                else
                    LineColor = 0x00000;
            }
            catch
            {
                LineColor = 0x000000;
                Log.WriteError(String.Format("Can not parce string to color '{0}'", hex));
            }
        }

        private void GetCaption(XElement root)
        {
            try { Caption = NormalizeCaption(root.Descendants("Text").First().Value); }
            catch { }
        }
    }

    public class VisioPage
    {
        public Double Width { get; set; }
        public Double Height { get; set; }
        public List<VisioShape> Shapes { get; set; }
        public VisioDocument Parent { get; set; }

        public VisioPage(XElement root, VisioDocument parent)
        {
            Shapes = new List<VisioShape>();
            this.Parent = parent;

            Width = Double.Parse(root.Descendants("PageProps").First().Descendants("PageWidth").First().Value, CultureInfo.InvariantCulture);
            Height = Double.Parse(root.Descendants("PageProps").First().Descendants("PageHeight").First().Value, CultureInfo.InvariantCulture);

            foreach (XElement xShapeRoot in root.Descendants("Shapes").First().Descendants("Shape"))
            {
                Shapes.Add(new VisioShape(xShapeRoot, this));
            }
        }
    }

    public class VisioDocument
    {
        public List<VisioPage> Pages { get; set; }

        public VisioDocument(XElement root)
        {
            Pages = new List<VisioPage>();

            RemoveNameSpace(root);
            foreach (XElement xPageRoot in root.Descendants("Pages").First().Descendants("Page"))
            {
                Pages.Add(new VisioPage(xPageRoot, this));
            }
        }
        private void RemoveNameSpace(XElement root)
        {
            foreach (XElement e in root.DescendantsAndSelf())
            {
                if (e.Name.Namespace != XNamespace.None)
                {
                    e.Name = XNamespace.None.GetName(e.Name.LocalName);
                }
                if (e.Attributes().Where(a => a.IsNamespaceDeclaration || a.Name.Namespace != XNamespace.None).Any())
                {
                    e.ReplaceAttributes(e.Attributes().Select(a => a.IsNamespaceDeclaration ? null : a.Name.Namespace != XNamespace.None ? new XAttribute(XNamespace.None.GetName(a.Name.LocalName), a.Value) : a));
                }
            }

        }
    }
}
