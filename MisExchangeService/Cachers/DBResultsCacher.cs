using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Cachers
{
    public static class DBResultsCacher
    {
        public static void Init()
        {
        }

        public static void SaveToError(ExchangeDTOs.Result result, Exception ex)
        {

        }

        public static List<ExchangeDTOs.Result> ReadFromCache()
        {
            List<ExchangeDTOs.Result> results = new List<ExchangeDTOs.Result>();

            return results;
        }

        public static void SaveToCache(ExchangeDTOs.Result result)
        {

        }

        public static void SaveToCache(List<ExchangeDTOs.Result> results)
        {
            foreach (ExchangeDTOs.Result result in results)
            {
                SaveToCache(result);
            }
        }

        public static List<ExchangeDTOs.Result> ExchangeWithCache(List<ExchangeDTOs.Result> results)
        {
            SaveToCache(results);
            results = ReadFromCache();
            return results;
        }
    }
}
