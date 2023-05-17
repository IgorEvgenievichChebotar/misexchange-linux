using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore;
using System.Reflection;

namespace ru.novolabs.Common
{
    public class CommonXmlMapperProvider : IMapperProvider
    {
        public string PathToMapperFiles {get;set;}

        protected List<TestMapperItem> testMapper;
        protected List<TargetMapperItem> targetMapper;
        protected List<BiomaterialMapperItem> bioMapper;

        public List<TestMapperItem> TestMapper { get { return testMapper; } }
        public List<TargetMapperItem> TargetMapper { get { return targetMapper; } }
        public List<BiomaterialMapperItem> BioMapper { get { return bioMapper; } }
        public void ProvideMapping()
        {
            if (String.IsNullOrEmpty(PathToMapperFiles))
            {
                PathToMapperFiles = Assembly.GetExecutingAssembly().CodeBase;            
            }
            testMapper = File.ReadAllText(Path.Combine(PathToMapperFiles,"TestMapper.xml")).Deserialize<List<TestMapperItem>>(Encoding.UTF8);
            targetMapper = File.ReadAllText(Path.Combine(PathToMapperFiles,"TargetMapper.xml")).Deserialize<List<TargetMapperItem>>(Encoding.UTF8);
            bioMapper = File.ReadAllText(Path.Combine(PathToMapperFiles,"BioMapper.xml")).Deserialize<List<BiomaterialMapperItem>>(Encoding.UTF8);
        }

    }
}
