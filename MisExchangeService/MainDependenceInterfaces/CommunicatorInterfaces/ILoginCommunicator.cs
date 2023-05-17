using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.MainDependenceInterfaces.CommunicatorInterfaces
{
    public interface ILoginCommunicator:IDisposable
    {
        void Login();
    }
}
