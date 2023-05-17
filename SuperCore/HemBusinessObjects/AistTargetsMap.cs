using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    [Serializable]
    public class AistTargetsMap
    {
        private List<Stage> stages = new List<Stage>();

        [CSN("Stages")]
        public List<Stage> Stages
        {
            get { return stages; }
            set { stages = value; }
        }
    }

    [Serializable]
    public class Stage
    {
        private String sql = string.Empty;
        private List<DonorGroup> donorGroups = new List<DonorGroup>();

        [CSN("DonorGroups")]
        public List<DonorGroup> DonorGroups
        {
            get { return donorGroups; }
            set { donorGroups = value; }
        }

        [CSN("Sql")]
        public String Sql
        {
            get { return sql; }
            set { sql = value; }
        }


    }
    public class DonorGroup
    {
        private String donorType = string.Empty;
        private String donorTypeEx = string.Empty;

        [CSN("DonorTypeEx")]
        public String DonorTypeEx
        {
            get { return donorTypeEx; }
            set { donorTypeEx = value; }
        }

        [CSN("DonorType")]
        public String DonorType 
        {
            get { return donorType; }
            set { donorType = value; }
        }
        private List<Target> targets = new List<Target>();
        [CSN("Targets")]
        public List<Target> Targets
        {
            get { return targets; }
            set { targets = value; }
        }
    }

    [Serializable]
    public class Target
    {
        private String code = string.Empty;

        [CSN("Code")]
        public String Code
        {
            get { return code; }
            set { code = value; }
        }
    }
}
