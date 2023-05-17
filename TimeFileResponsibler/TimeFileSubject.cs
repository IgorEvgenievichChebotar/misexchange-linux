using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace TimeFileSubject
{
    public class TimeFileSubject
    {
        public TimeFileSubject(Int32 Break = 0, String FileName = null, String Format = null)
        {
            if (Break != 0)
                timebreak = Break;
            if (!String.IsNullOrEmpty(FileName))
                fileName = FileName;
            if (!String.IsNullOrEmpty(Format))
                format = Format;
            ReadyToExit = false;
            thread = new Thread(StartWriting);
            thread.IsBackground = true;
            thread.Start();
        }

        private Thread thread;


        /// <summary>
        /// Перерыв в записи
        /// </summary>
        public Int32 timebreak = 20;
        
        /// <summary>
        /// Имя файла, куда будет вестись запись
        /// </summary>
        public String fileName = "timeFile.txt";

        
        /// <summary>
        /// Формат записи даты
        /// </summary>
        public String format = "dd.MM.yyyy HH:mm:ss";
        private Boolean ReadyToExit = false;
        public void StartWriting()
        {
            try
            {
                if (!Path.IsPathRooted(fileName))
                    fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                while (!ReadyToExit)
                {
                    try
                    {
                        File.WriteAllText(fileName, DateTime.Now.ToString(format));
                    }
                    catch { }
                    Thread.Sleep(timebreak * 1000);
                }
            }
            catch { }
        }

        public void Stop()
        {
            ReadyToExit = true;
        }

    }
}
