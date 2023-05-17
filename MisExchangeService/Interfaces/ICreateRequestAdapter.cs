using ru.novolabs.ExchangeDTOs;
using ru.novolabs.SuperCore.LimsBusinessObjects;

namespace ru.novolabs.MisExchange.Interfaces
{
    public interface ICreateRequestAdapter
    {
        CreateRequest3Request MakeRequest(Request requestDTO, int requestLisId, bool savePatient);
    }
}