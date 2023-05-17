using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class FileInfo : BaseObject
    {
        private String pathFilename;

        [CSN("PathFilename")]
        public String PathFilename
        {
            get { return pathFilename; }
            set { pathFilename = value; }
        }

        private String pathFileContent;

        [CSN("PathFileContent")]
        public String PathFileContent
        {
            get { return pathFileContent; }
            set { pathFileContent = value; }
        }

        [CSN("Id")]
        public int Id { get; set; }
    }
}
