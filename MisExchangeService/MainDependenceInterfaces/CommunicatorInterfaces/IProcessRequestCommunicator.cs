using ru.novolabs.SuperCore.Core;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.MainDependenceInterfaces.CommunicatorInterfaces
{
    public interface IProcessRequestCommunicator
    {
        Boolean CreateRequestXXX(CreateRequest3Request request, out Int32 id);
        String GetFreeNr(LimsUserSession userSession);
        List<Patient> PatientsByCode(string code);
        int PatientSave(Patient patient, out List<PatientCardId> patientCardIds);
        bool IsAblePatientSave { get; }
    }
}