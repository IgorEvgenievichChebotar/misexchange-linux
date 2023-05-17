using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.Classes.HandleResultExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Classes
{
    /// <summary>
    /// Needs to inform ResultSaver is Exception in ExchangeHelper's method for sending result Handled 
    /// </summary>
    class HandledResultException : Exception
    {
        public HandledResultException(Dictionary<Result, string> errorDict, IProvideSuccessfullExternalResults succssfullExternalResultsProvider)
        {
            ErrorDict = errorDict;
            SuccssfullExternalResultsProvider = succssfullExternalResultsProvider;
        }
        public Dictionary<Result, string> ErrorDict { get; private set; }
        public IProvideSuccessfullExternalResults SuccssfullExternalResultsProvider { get; private set; }
    }   
}
