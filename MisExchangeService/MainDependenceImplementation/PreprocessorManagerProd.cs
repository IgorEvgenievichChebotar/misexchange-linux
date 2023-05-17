using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.MainDependenceImplementation
{
    class PreprocessorManagerProd : IPreprocessorManager
    {
        public void Start()
        {
            ProgramContext.ServerListener.TryRun();
        }

        public void Stop()
        {
            ProgramContext.ServerListener.Stop();
        }
    }
}
