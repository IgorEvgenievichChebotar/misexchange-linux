using ru.novolabs.MisExchange.MainDependenceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.MainDependenceImplementation
{
    class TimeFileSubjectPerformer : ITimeFileSubjectPerformer
    {
        TimeFileSubject.TimeFileSubject _timeFileSubject = null;
        public TimeFileSubjectPerformer(ITimeFileSubjectSettings settings)
        {
            Settings = settings;        
        }
        ITimeFileSubjectSettings Settings { get; set; }

        public void Start()
        {
            _timeFileSubject = new TimeFileSubject.TimeFileSubject(Settings.TimeFileBreak, Settings.TimeFileName, Settings.TimeFileFormat);       
        }
        public void Stop()
        {
            if (_timeFileSubject != null)
                _timeFileSubject.Stop();        
        }
    }
}
