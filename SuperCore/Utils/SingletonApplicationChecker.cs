using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ru.novolabs.SuperCore
{
    public class SingletonApplicationChecker
    {
        Mutex _mutex;
        public bool IsApplicationExisted(string applicationName = null)
        {
            if (String.IsNullOrEmpty(applicationName))
                applicationName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            _mutex = new Mutex(true, applicationName.Replace(Path.DirectorySeparatorChar, '|'));
            if (_mutex.WaitOne(TimeSpan.Zero, true))
                return false;
            return true;
        }
        public void Close()
        {
            if (_mutex != null)
                _mutex.Close();
        }
    }
}
