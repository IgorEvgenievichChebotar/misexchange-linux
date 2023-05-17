using ru.novolabs.MisExchangeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ru.novolabs.MisExchange.ServiceClasses.RESTFull
{
    class SimpleRESTFullClient
    {
        public string SendAsPost(string serverAddress, Func<HttpContent> contentFactory, bool isLoggingRequest = true, Action<HttpClient> headersConfiguration = null)
        {
            using (HttpClient client = new HttpClient())
            using (HttpContent content = contentFactory())
            {
                if (headersConfiguration != null)
                    headersConfiguration(client);
                if (isLoggingRequest)
                    GAP.Logger.WriteText("Trying to send by Post to [Server: {0}]. MessageContent:\r\n{1}", serverAddress, content.ReadAsStringAsync().Result);
                try
                {
                    GAP.Logger.WriteText("Sending");
                    var response = client.PostAsync(serverAddress, content).Result;
                    string resultString = response.Content.ReadAsStringAsync().Result;
                    GAP.Logger.WriteText("Recieved response");
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorStr = String.Format("Service returned bad http request status code({0} {1}) for POST  method:\r\n{2}\r\n", response.StatusCode.GetHashCode(), response.ReasonPhrase, resultString ?? String.Empty);
                        GAP.Logger.WriteError(errorStr);
                        throw new RESTFullClientException(errorStr);
                    }
                    else
                    {
                        GAP.Logger.WriteText("Request was successfully completed. Response:\r\n{0}\r\n", resultString ?? String.Empty);
                    }
                    return resultString;
                }
                catch (Exception ex)
                {
                    GAP.Logger.WriteError("Cannot execute Http Post method to send message. Error:\r\n{0}", ex.ToString());
                    throw;
                }
            }
        }
        public string SendStringAsPost(string serverAddress, StringContent stringContent, bool isLoggingRequest = true, Action<HttpClient> headersConfiguration = null)
        {
            return SendAsPost(serverAddress, () => stringContent, isLoggingRequest, headersConfiguration);      
        }
    }
    class RESTFullClientException : Exception
    {
        public RESTFullClientException(string message)
            : base(message)
        { }   
    }
}
