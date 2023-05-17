using ru.novolabs.MisExchange.MainDependenceInterfaces.CommunicatorInterfaces;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.MainDependenceImplementation.Communicators
{
    class LisCommunicatorProd : ILoginCommunicator, IProcessRequestCommunicator, IProcessResultCommunicator
    {
        public void Login()
        {
            ProgramContext.LisCommunicator = new MisExchangeServerCommunicator();
            ((LimsIntegrationService)ProgramContext.LisCommunicator).Login();
        }

        public void Dispose()
        {
            ProgramContext.LisCommunicator.Dispose();
        }
        public bool CreateRequestXXX(SuperCore.LimsBusinessObjects.CreateRequest3Request request, out int id)
        {
            return ProgramContext.LisCommunicator.CreateRequestXXX(request, out id, null);
        }

        public String GetFreeNr(LimsUserSession userSession)
        {
            return ProgramContext.LisCommunicator.GetFreeNr(userSession);
        }

        public List<SuperCore.LimsBusinessObjects.Patient> PatientsByCode(string code)
        {
            return ProgramContext.LisCommunicator.PatientsByCode(code, null);
        }

        public int PatientSave(SuperCore.LimsBusinessObjects.Patient patient, out List<SuperCore.LimsBusinessObjects.PatientCardId> patientCardIds)
        {
            return ProgramContext.LisCommunicator.PatientSave(patient, out patientCardIds, null);
        }

        public bool IsAblePatientSave
        {
            get
            {
                return ProgramContext.LisCommunicator.ServerOptions.GetServerOption(ConstServerOptions.EDIT_PATIENT_FROM_REQUEST) != null
                && ProgramContext.LisCommunicator.ServerOptions.GetServerOption(ConstServerOptions.EDIT_PATIENT_FROM_REQUEST).Value == "true";
            }
        }
        public List<SuperCore.LimsBusinessObjects.Exchange.ExternalResult> GetRequestsResults(List<SuperCore.ObjectRef> requestIds)
        {
            return ProgramContext.LisCommunicator.GetRequestsResults(requestIds, null);
        }

        public List<SuperCore.LimsBusinessObjects.Exchange.ExternalResult> ResendCloseEvents(List<SuperCore.ObjectRef> requestIds)
        {
            return ProgramContext.LisCommunicator.ResendCloseEvents(requestIds, null);
        }
    }
}