using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.MainDependenceInterfaces
{
    interface ITimeFileSubjectSettings
    {
        Int32 TimeFileBreak { get; }
        String TimeFileName { get; }
        String TimeFileFormat { get; }
    }
}
