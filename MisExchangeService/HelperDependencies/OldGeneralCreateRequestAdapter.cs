using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.Interfaces;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Classes
{
    [Obsolete("This class is not supported. It not support OrganizationCode field and other newer fields. Please, use SimpleCreateRequestAdapter or your own custome adapter")]
    class OldGeneralCreateRequestAdapter: ICreateRequestAdapter
    {
        public CreateRequest3Request MakeRequest(Request requestDTO, int requestLisId, bool savePatient)
        {
            return CreateRequest3Adapter.MakeRequest(requestDTO, requestLisId);
        }
    }
}
