using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Classes
{
    class GeneralRequestValidator: IRequestValidator
    {
        public void CheckData(Request requestDTO)
        {
            RequestDTOValidator.CheckData(requestDTO);
        
        }
    }
}
