
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class DirectoryVesionInfoSet
    {
        private List<DirectoryVersionInfo> versions = new List<DirectoryVersionInfo>();

        [CSN("Versions")]
        public List<DirectoryVersionInfo> Versions
        {
            get { return versions; }
            set { versions = value; }
        }
    }    
    
    public class DirectoryVersionInfo
    {
        [CSN("Name")]
        public string Name { get; set; }
        [CSN("Version")]
        public int Version { get; set; }
    }
}
