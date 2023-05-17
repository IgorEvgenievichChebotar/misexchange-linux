using ru.novolabs.MisExchange.HelperDependencies.SimpleRequestValidatorDependencies;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.HelperDependencies.SimpleRequestValidatorDependencies
{
    class SimpleUserValueSearcherByCode : SimpleUserValueSearcher
    {
        public override SuperCore.LimsDictionary.UserDictionaryValue FindEnumerationUserValue(ExchangeDTOs.UserField userField, SuperCore.LimsDictionary.UserDirectoryDictionaryItem userDictionary)
        {
            UserDictionaryValue dictionaryItem = (UserDictionaryValue)userDictionary.FindValueByCode(userField.Value);
            return dictionaryItem;
        }
        public override SuperCore.LimsDictionary.UserDictionaryValue FindSetUserValue(ExchangeDTOs.UserField userField, string value, SuperCore.LimsDictionary.UserDirectoryDictionaryItem userDictionary)
        {
            UserDictionaryValue dictionaryItem = (UserDictionaryValue)userDictionary.FindValueByCode(value);
            return dictionaryItem;
        }
        public override string GenerateUserValueCode(ExchangeDTOs.UserField userField,string value, UserDirectoryDictionaryItem userDictionary)
        {
            return value;
        }
    }
}
