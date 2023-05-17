using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ru.novolabs.ExchangeDTOs
{
    public class FileInfo
    {
        [Key]
        [XmlIgnore]
        public int Id { get; set; }
        public String Filename { get; set; }

        public String FileContent { get; set; }
    }
}