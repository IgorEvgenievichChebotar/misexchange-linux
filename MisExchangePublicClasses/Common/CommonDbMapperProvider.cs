using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.Common
{
    public class CommonDbMapperProvider: IMapperProvider
    {
        public CommonDbMapperProvider(string connectionStr)
        {
            ConnectionStr = connectionStr;       
        }
        protected string ConnectionStr { get; set; }

        public List<TestMapperItem> TestMapper { get; protected set; }
        public List<TargetMapperItem> TargetMapper { get; protected set; }
        public List<BiomaterialMapperItem> BioMapper { get; protected set; }
        public virtual void ProvideMapping()
        {
            using (var context = new MapperContext(ConnectionStr))
            {
                TestMapper = context.TestMapping.ToList();
                TargetMapper = context.TargetMapping.ToList();
                BioMapper = context.BioMapping.ToList();
            }
        }
        protected void ProvideInheritMapping(MapperContext context)
        {
            TestMapper = context.TestMapping.ToList();
            TargetMapper = context.TargetMapping.ToList();
            BioMapper = context.BioMapping.ToList();        
        }


    }
}
