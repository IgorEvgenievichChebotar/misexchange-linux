using ru.novolabs.ExchangeDTOs;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.HelperDependencies.SimpleRequestValidatorDependencies
{
    class SimpleUserValueSearcher
    {
        private static Int64 maxGeneratedCode = 0;
        public virtual UserDictionaryValue FindEnumerationUserValue(UserField userField, UserDirectoryDictionaryItem userDictionary)
        {
            UserDictionaryValue dictionaryItem = (UserDictionaryValue)userDictionary.FindValueByName(userField.Value);
            return dictionaryItem;
        }
        public virtual UserDictionaryValue FindSetUserValue(UserField userField, string value, UserDirectoryDictionaryItem userDictionary)
        {
            UserDictionaryValue dictionaryItem = (UserDictionaryValue)userDictionary.FindValueByName(value);
            return dictionaryItem;
        }
        public virtual string GenerateUserValueCode(UserField userField,string value, UserDirectoryDictionaryItem userDictionary)
        { 
            if (maxGeneratedCode > 0)
                return (maxGeneratedCode++).ToString();

            Predicate<string> predicateIsNumeric = (string str)=>
            {
                long temp;
                return Int64.TryParse(str, out temp);            
            };
            maxGeneratedCode = userDictionary.Values.Where(v => predicateIsNumeric(v.Code)).Select(v => Int64.Parse(v.Code)).DefaultIfEmpty(0).Max() + 1;
            return maxGeneratedCode.ToString();
        }
    }
}
