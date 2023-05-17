using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.Common
{
   public static  class SimpleMapper
    {

       public const string formatString = "Not find {0} [Value: {1}] in map dictionary collection [{2}]";
       public static class ByLis
       {
           public static  BaseMapperItem GetMapperItemByLisCode(string Code, IEnumerable<BaseMapperItem> mapperCollection, string mapperName)
           {
               BaseMapperItem item = mapperCollection.FirstOrDefault(m => m.LisCode == Code);
               if (item == null)
               {
                   throw new NotInMappingDictionaryException(String.Format(formatString, "LisCode",Code, mapperName));
               }
               return item;
           }
           public static BaseMapperItem GetMapperItemByLisName(string Name, IEnumerable<BaseMapperItem> mapperCollection, string mapperName)
           {
               BaseMapperItem item = mapperCollection.FirstOrDefault(m => m.LisName == Name);
               if (item == null)
               {
                   throw new NotInMappingDictionaryException(String.Format(formatString, "LisName", Name, mapperName));
               }
               return item;
           }
       }
       public static class ByMis
       {
           public static BaseMapperItem GetMapperItemByMisCode(string Code, IEnumerable<BaseMapperItem> mapperCollection,string mapperName)
           {
               BaseMapperItem item = mapperCollection.FirstOrDefault(m => m.MisCode == Code);
               if (item == null)
               {
                   throw new NotInMappingDictionaryException(String.Format(formatString, "MisCode", Code, mapperName));
               }
               return item;
           }
           public static BaseMapperItem GetMapperItemByMisName(string Name, IEnumerable<BaseMapperItem> mapperCollection,string mapperName)
           {
               BaseMapperItem item = mapperCollection.FirstOrDefault(m => m.MisName == Name);
               if (item == null)
               {
                   throw new NotInMappingDictionaryException(String.Format(formatString, "MisName", Name, mapperName));
               }
               return item;
           }
       }
    }
   public class NotInMappingDictionaryException : Exception 
   {
       public NotInMappingDictionaryException(string errorStr):base(errorStr)
       {  
    
       }   
   }
}
