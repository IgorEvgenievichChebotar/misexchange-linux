using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore
{
    public interface IObjectEditor
    {
        void New();
        void Edit(int objectId);
        IBaseObject GetObject(int objectId);
    }
}
