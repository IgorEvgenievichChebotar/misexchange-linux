﻿using ru.novolabs.ExchangeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Interfaces
{
    public interface IRequestValidator
    {
        void CheckData(Request request);
    }
}
