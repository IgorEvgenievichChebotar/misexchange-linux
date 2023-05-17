using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.HelperDependencies.SimpleRequestValidatorDependencies;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.CommunicatorInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.MisExchange.HelperDependencies.SimpleCreateRequestDependencies
{
    internal class SimpleCreateRequest3UserValuesFactory
    {
        private SimpleUserValueSearcher UserValueSearcher { get; set; }

        public SimpleCreateRequest3UserValuesFactory(SimpleUserValueSearcher userValueSearcher, IDictionaryCache dictionaryCache, IProcessRequestSettings settings, IProcessRequestCommunicator communicator)
        {
            UserValueSearcher = userValueSearcher;
            DictionaryCache = dictionaryCache;
            Settings = settings;
            LimsCommunicator = communicator;
        }

        private readonly object _syncObject = new object();

        protected IDictionaryCache DictionaryCache { get; set; }
        protected IProcessRequestSettings Settings { get; set; }
        protected IProcessRequestCommunicator LimsCommunicator { get; set; }

        public virtual List<UserValue> BuildUserValues(Request requestDTO)
        {
            List<UserValue> userValues = new List<UserValue>();
            foreach (UserField userField in requestDTO.UserFields)
            {
                UserValue userValue = BuildUserValue(userField);
                userValues.Add(userValue);
            }
            foreach (UserField userField in requestDTO.Patient.UserFields)
            {
                UserValue userValue = BuildUserValue(userField);
                userValue.Name = userField.Name;
                userValue.Code = userField.Code;
                userValues.Add(userValue);
            }
            if (requestDTO.Patient.PatientCard != null)
            {
                foreach (UserField userField in requestDTO.Patient.PatientCard.UserFields)
                {
                    UserValue userValue = BuildUserValue(userField);
                    userValue.Name = userField.Name;
                    userValue.Code = userField.Code;
                    userValues.Add(userValue);
                }
            }
            userValues.RemoveAll(uv => uv.UserField.Id == 0);

            return userValues;
        }
        public virtual UserValue BuildUserValue(UserField userField)
        {
            UserFieldDictionaryItem userFieldDictionaryItem = DictionaryCache.GetDictionaryItem<UserFieldDictionaryItem>(userField.Code);
            UserValue userValue = new UserValue();
            if (userFieldDictionaryItem == null)
            {
                throw new NlsServerException(400, "Пользовательского поля с кодом " + userField.Code + " не существует в справочнике");
            }
            else
            {
                userValue.UserField = new ObjectRef(userFieldDictionaryItem.Id);
            }

            UserFieldDictionaryItem userFieldDict = DictionaryCache.GetDictionaryItem<UserFieldDictionaryItem>(userValue.UserField.Id);
            if (userFieldDict == null)
            {
                return userValue;
            }
            UserDirectoryDictionaryItem userDictionary = userFieldDict.UserDirectory != null
                ? DictionaryCache.GetDictionaryItem<UserDirectoryDictionaryItem>(userFieldDict.UserDirectory.Id)
                : null;

            UserDictionaryValue dictionaryItem = null;
            switch (userFieldDict.FieldType)
            {
                case (int)UserFieldTypes.ENUMERATION:

                    if (userDictionary == null)
                    {
                        return userValue;
                    }
                    dictionaryItem = BuildUserDictionary(userField, userField.Value,
                        (uf, str) => BuildEnumaretionUserDictionary(uf, userDictionary), userDictionary);
                    if (dictionaryItem != null)
                    {
                        userValue.Reference.Id = dictionaryItem.Id;
                    }

                    break;
                case (int)UserFieldTypes.SET:
                    if (userDictionary == null)
                    {
                        return userValue;
                    }
                    string[] valueSet = userField.Value.Trim().Split(new char[] { ',' });
                    foreach (string value in valueSet)
                    {
                        if (value.Trim() != "")
                        {
                            dictionaryItem = BuildUserDictionary(userField, value,
                                (uf, str) => BuildSetUserDictionary(uf, str, userDictionary), userDictionary);
                            if (dictionaryItem != null)
                            {
                                ObjectRef reference = new ObjectRef
                                {
                                    Id = dictionaryItem.Id
                                };
                                userValue.Values.Add(reference);
                            }
                        }
                    }
                    break;
                default:
                    userValue.Value = userField.Value;
                    break;
            }
            return userValue;
        }
        protected virtual UserDictionaryValue BuildUserDictionary(UserField userField, string value, Func<UserField, string, UserDictionaryValue> findFunc, UserDirectoryDictionaryItem userDictionary)
        {
            UserDictionaryValue dictionaryItem = null;
            lock (_syncObject)
            {
                dictionaryItem = findFunc(userField, value);
                if (dictionaryItem == null)
                {
                    bool isAllowedToAddUserValues = Settings.IsAllowedToAddUserValues;
                    if (!isAllowedToAddUserValues)
                    {
                        throw new Exception(string.Format("Value {0} is not allowed for Enumeration|Set Type userField [UserFieldCode:{1}]", value, userField.Code));
                    }

                    string userValueCode = UserValueSearcher.GenerateUserValueCode(userField, value, userDictionary);
                    dictionaryItem = DictionaryCache.CreateItemNonCachedDictionary<UserDictionaryValue, UserDirectoryDictionaryItem>(userValueCode, value, userDictionary);
                }
            }
            return dictionaryItem;
        }
        protected virtual UserDictionaryValue BuildSetUserDictionary(UserField userField, string value, UserDirectoryDictionaryItem userDictionary)
        {
            //this may be some handling by userFieldCode
            return UserValueSearcher.FindSetUserValue(userField, value, userDictionary);
        }
        protected virtual UserDictionaryValue BuildEnumaretionUserDictionary(UserField userField, UserDirectoryDictionaryItem userDictionary)
        {
            return UserValueSearcher.FindEnumerationUserValue(userField, userDictionary);
        }
    }
}