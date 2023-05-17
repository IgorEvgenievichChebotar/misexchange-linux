using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.MainDependenceImplementation
{
    class TaskManagerProd : ITaskManager
    {
        public void Start()
        {
            ProgramContext.TaskManager.TryRunForeground();
        }

        public void Stop()
        {
            ProgramContext.TaskManager.Stop();
        }
    }
}
