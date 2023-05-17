using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.Reporting
{
    [XmlInclude(typeof(HtmlExportSettings)), XmlInclude(typeof(PdfExportSettings))]
    [XmlInclude(typeof(Excel2007ExportSettings)), XmlInclude(typeof(Word2007ExportSettings)), XmlInclude(typeof(RtfExportSettings))]
    [XmlInclude(typeof(JpegExportSettings)), XmlInclude(typeof(PngExportSettings))]
    public abstract class ExportSettings
    {
        public override string ToString()
        {
            return null;
        }
    }

    public class ImageExportSettings : ExportSettings
    {
        public ImageExportSettings()
        {
            Resolution = 96;
        }

        public bool SeparateFiles { get; set; }
        public int Resolution { get; set; }
    }

    public class JpegExportSettings : ImageExportSettings
    {
        public JpegExportSettings()
            : base()
        {
            JpegQuality = 100;

            object resolution = ProgramContext.Settings["jpegDefaultResolution", false];
            if(resolution != null)
            {
                Resolution = (int)resolution;
            }
            object quality = ProgramContext.Settings["jpegDefaultQuality", false];
            if (quality != null)
            {
                JpegQuality = (int)quality;
            }
        }

        public int JpegQuality { get; set; }
    }

    public class PngExportSettings : ImageExportSettings
    {
        public PngExportSettings()
            : base()
        { }
    }

    public class HtmlExportSettings : ExportSettings
    {
        public HtmlExportSettings()
        {
            Wysiwyg = true;
            Pictures = true;
            SinglePage = true;
            EmbedPictures = true;
        }
        public bool Wysiwyg { get; set; }
        public bool Pictures { get; set; }
        public bool SinglePage { get; set; }
        public bool EmbedPictures { get; set; }
    }

    public class PdfExportSettings : ExportSettings
    {
        public PdfExportSettings()
        {
            EmbeddingFonts = true;
            Background = true;
            PrintOptimized = true;
        }

        public bool EmbeddingFonts { get; set; }
        public bool Background { get; set; }
        public bool PrintOptimized { get; set; }
    }

    public class OpenOfficeExportBase : ExportSettings
    {
        public bool Wysiwyg { get; set; }
    }


    public class Excel2007ExportSettings : OpenOfficeExportBase
    {
        public Excel2007ExportSettings()
        {
            PageBreaks = true;
        }

        public bool PageBreaks { get; set; }
        public bool DataOnly { get; set; }
    }

    public class Word2007ExportSettings : OpenOfficeExportBase
    {
    }

    public enum RTFImageFormat
    {
        Png,
        Jpeg,
        Metafile
    }

    public class RtfExportSettings : OpenOfficeExportBase
    {
        public RtfExportSettings()
        {
            Wysiwyg = true;
            PageBreaks = true;
            ImageFormat = RTFImageFormat.Metafile;
        }

        public bool PageBreaks { get; set; }
        public RTFImageFormat ImageFormat { get; set; }
    }
}