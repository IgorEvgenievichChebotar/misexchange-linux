using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Cachers
{
    public static class DBRequestsCacher
    {
        public static void Init()
        {
        }

        public static void SaveToError(ExchangeDTOs.Request request, Exception ex)
        {

        }

        public static List<ExchangeDTOs.Request> ReadFromCache()
        {
            List<ExchangeDTOs.Request> requests = new List<ExchangeDTOs.Request>();

            return requests;
        }

        public static void SaveToCache(ExchangeDTOs.Request request)
        {

        }

        public static void SaveToCache(List<ExchangeDTOs.Request> requests)
        {
            foreach (ExchangeDTOs.Request request in requests)
            {
                SaveToCache(request);
            }
        }

        public static List<ExchangeDTOs.Request> ExchangeWithCache(List<ExchangeDTOs.Request> requests)
        {
            SaveToCache(requests);
            requests = ReadFromCache();
            return requests;
        }
    }
}
