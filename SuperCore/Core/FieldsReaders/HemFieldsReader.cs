using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.HemBusinessObjects;
using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ru.novolabs.SuperCore.Core.FieldsReaders
{
    public class HemFieldsReader: FieldsReader
    {
        public HemFieldsReader(BaseDictionaryCache DictionaryCache) : base(DictionaryCache)
        {

        }

        public override void SetUserParameter(String FieldName, KeyValuePair<String, String> Element, Object Results, List<String> ErrorMsg)
        {
            BloodParameterItem param = ((BloodParameterGroupDictionaryClass)ProgramContext.Dictionaries[HemDictionaryNames.BloodParameterGroup]).GetBloodParameterByCode(FieldName);
            if (param != null)
            {
                BloodParameterValue paramValue = new BloodParameterValue();
                if (param.UserDirectory != null)
                {
                    Int32 Id;

                    UserDirectoryDictionaryItem userDirectory = param.UserDirectory;
                    userDirectory = (UserDirectoryDictionaryItem)ProgramContext.Dictionaries[HemDictionaryNames.UserDirectory, userDirectory.Id];
                    switch (param.FieldType)
                    {
                        case BloodParameterFieldType.HEM_FIELD_TYPE_ENUMERATION:


                            if (Int32.TryParse(Element.Value, out Id))
                            {
                                UserDictionaryValue userDictionaryValue = userDirectory.Find(Id);
                                if (userDictionaryValue != null)
                                {

                                    paramValue.Reference = userDictionaryValue;
                                        ///new ObjectRef(userDictionaryValue.Id);
                                    //paramValue.Parameter = new ObjectRef(param.Id);
                                    paramValue.Parameter = new Parameter() { Id = param.Id };

                                }
                            }
                            break;
                        case BloodParameterFieldType.HEM_FIELD_TYPE_SET:
                            List<String> Ids = Element.Value.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (String id in Ids)
                            {
                                if (Int32.TryParse(id, out Id))
                                {
                                    UserDictionaryValue userDictionaryValue = userDirectory.Find(Id);
                                    if (userDictionaryValue != null)
                                    {

                                        paramValue.Reference = userDictionaryValue;
                                        paramValue.Values.Add(new ObjectRef(userDictionaryValue.Id));

                                    }
                                }
                            }
                            break;
                    }
                    PropertyInfo userParams = Results.GetType().GetCustomProperty(UserParamNames.BloodParameters);
                    List<BloodParameterValue> bloodParams = (List<BloodParameterValue>)userParams.GetValue(Results, null);
                    bloodParams.Add(paramValue);
                    userParams.SetValue(Results, bloodParams, null);
                }
                else
                {
                    //paramValue.Parameter = new ObjectRef(param.Id);
                    paramValue.Parameter = new Parameter() { Id = param.Id };
                    paramValue.Value = Element.Value;
                    //if (param.FieldType == BloodParameterFieldType.HEM_FIELD_TYPE_DATETIME)
                    //{
                    //    paramValue.Value = paramValue.Value.Replace(".", "");
                    //}
                }
            }
            else
                ErrorMsg.Add("Поля " + FieldName + " в объекте " + Results.ToString() + " не существует.");
        }
    }
}
