using ru.novolabs.HL7;
using ru.novolabs.MisExchangeService.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore;
using MisExchangeAdapters.Parser.HL7;

namespace ru.novolabs.MisExchange.ExchangeHelpers.HL7
{
    /// <summary>
    /// http://lis50:8887/browse/LIS-6569
    /// </summary>
    [ExchangeHelperName("ExchangeHelper3_HL7")]
    class HL7ExchangeHelper3: ExchangeHelper3
    {
        HelperSettings helperSettings = new HelperSettings();
        TCPIPClientServerTransport server;
        public HL7ExchangeHelper3()
        {
            helperSettings = File.ReadAllText(Path.Combine(PathHelper.AssemblyDirectory, "exchangeHelperSettings.xml"), Encoding.UTF8).Deserialize<HelperSettings>(Encoding.UTF8);
        }

        protected override void SpecificProcessNewData()
        {
            //В силу специфики ТЗ, прием данных будет осуществляться в ином методе. Здесь же будет только первоначальный запуск сервера
            if (server == null)
            {
                server = new TCPIPClientServerTransport();
                server.Init(helperSettings);
                server.HL7QueryEvent += server_HL7QueryEvent;
            }
            //1 Получаем заявки
            //2 Преобразовываем их в ExchangeDTOs.Request
            //var requests = new List<ExchangeDTOs.Request>();
            //3 Сохраняем в БД
            //4 Считываем из БД ?
            //5 Отправляем в ЛИС
            //foreach (var requestDTO in requestDTOs)
            //{
                //var request = new ExchangeDTOs.Request();
                //requests.Add(request);
                //Обработка здесь
                //ProcessRequest(request);
                
            //}
        }

        private void server_HL7QueryEvent(object sender, Classes.HL7QueryEventArgs e)
        {
            ExchangeDTOs.Request request = e.Request;
            request.Patient.Code = helperSettings.PatientCodePrefix + request.Patient.Code;
            ProcessRequest(request);
        }



        protected override void SpecificFilterRequestResults(List<SuperCore.ObjectRef> requestIds)
        {
            throw new NotImplementedException();
        }

        public override void SpecificStoreResults(List<ExchangeDTOs.Result> results)
        {
            foreach (ExchangeDTOs.Result result in results)
            {
                //BrokenUseless: TCP isn't used, this code contain a strategy, which is spicific for HL7FileExchange, only after CacheInBase realization this code will may me used
                throw new NotImplementedException("Not used protocol");
                List<String> res = HL7MessageParser.BuildHL7Results(result, null );
                String strresult = "";
                foreach (String r in res)
                {
                    strresult += r + "\r\n";
                }
                server.SendBytes(server.Encoding.GetBytes(strresult));
            }
            //throw new NotImplementedException();
        }

       
    }
}
