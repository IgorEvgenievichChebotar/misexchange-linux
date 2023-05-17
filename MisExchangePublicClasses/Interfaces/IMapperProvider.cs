using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.Common
{
    public interface IMapperProvider
    {
        List<TestMapperItem> TestMapper { get;  }
        List<TargetMapperItem> TargetMapper { get;  }
        List<BiomaterialMapperItem> BioMapper { get;  }
        void ProvideMapping();
    }
}
