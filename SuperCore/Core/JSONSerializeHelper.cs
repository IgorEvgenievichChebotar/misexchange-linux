using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ru.novolabs.SuperCore
{
    public static class JSONSerializeHelper
    {
        public static string ToJSON(this object obj, Encoding encoding)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                serializer.WriteObject(ms, obj);
                return encoding.GetString(ms.ToArray());
            }                    
        }

        public static T FromJSON<T>(this string json, Encoding encoding)
        {
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}
