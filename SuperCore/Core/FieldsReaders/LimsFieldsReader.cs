using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.Core.FieldsReaders
{
    public class LimsFieldsReader : FieldsReader
    {
        public LimsFieldsReader(BaseDictionaryCache DictionaryCache)
            : base(DictionaryCache)
        {

        }

        public override void SetUserParameter(String FieldName, KeyValuePair<String, String> Element, Object Results, List<String> ErrorMsg)
        {
            if (((UserFieldDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.UserField]).Find(FieldName) != null)
            {
                UserFieldDictionaryItem userField = (UserFieldDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.UserField, FieldName);
                //UserDirectoryDictionaryItem userDirectory = (UserDirectoryDictionaryItem)ProgramContext.Dictionaries[LisDictionaryNames.UserDirectory, userField.UserDirectory.Id];
                UserValue userValue = new UserValue();
                if (userField.UserDirectory != null)
                {
                    UserDirectoryDictionaryItem userDirectory = userField.UserDirectory;
                    Int32 Id;
                    if (Int32.TryParse(Element.Value, out Id))
                    {
                        UserDictionaryValue userDictionaryValue = userDirectory.Find(Id);
                        if (userDictionaryValue != null)
                        {
                            
                            userValue.Reference = new ObjectRef(userDictionaryValue.Id);
                            //userValue.Value = Element.Value;
                            userValue.UserField = new ObjectRef(userField.Id);
                           
                        }
                    }
                }
                else
                {
                    
                    userValue.UserField = new ObjectRef(userField.Id);
                    userValue.Value = Element.Value;
                    if (userField.FieldType == (int)UserFieldTypes.DATETIME)
                    {
                        userValue.Value = userValue.Value.Replace(".", "");
                    }
                }
                Type t = Results.GetType();
                if (t.Equals(typeof(BaseRequest)))
                    ((BaseRequest)Results).UserValues.Add(userValue);
                if (t.Equals(typeof(CreateRequest3Request)))
                    ((CreateRequest3Request)Results).UserValues.Add(userValue);
            }
            else
                ErrorMsg.Add("Поля " + FieldName + " в объекте " + Results.ToString() + " не существует.");
        }
    }
}
