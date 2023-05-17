using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Classes
{
    [Obsolete("This class is not supported. Please, use SimpleRequestValidator or your own custom validator")]
    class OldGeneralRequestValidator: IRequestValidator
    {
        public void CheckData(Request requestDTO)
        {
            RequestDTOValidator.CheckData(requestDTO);       
        }
    }
}
