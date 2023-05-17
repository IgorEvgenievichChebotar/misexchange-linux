using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsBusinessObjects.Outsource;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore
{
    public static class UserFieldHelperOld
    {
        public static void PrepareUserFieldsValuesByCodes(List<UserValue> userValues, List<ErrorMessage> warnings)
        {
            lock (ProgramContext.Dictionaries)
            {
                UserFieldDictionary userFieldsDict = (UserFieldDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.UserField];
                UserDirectoryDictionary userDirectoryDict = (UserDirectoryDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.UserDirectory];

                foreach (UserValue userValue in userValues)
                {
                    // Ищем пользовательское поле по коду, указанному внешней системой
                    var userField = (UserFieldDictionaryItem)userFieldsDict.Find(userValue.Code);
                    if (userField != null)
                    {
                        userValue.UserField.Id = userField.Id;
                        var userDictionary = userField.UserDirectory != null ? (UserDirectoryDictionaryItem)userDirectoryDict.Find(userField.UserDirectory.Id) : null;
                        switch (userField.FieldType)
                        {
                            // Если пользовательское поле является перечислимым, то необходимо проинициализировать ссылку на значение пользовательского справочника
                            case (int)UserFieldTypes.ENUMERATION:
                                if (userDictionary != null)
                                {
                                    // Ищем значение пользовательского справочника по имени, указанному внешней системой
                                    var dictionaryItem = (UserDictionaryValue)userDictionary.FindValueByName(userValue.Value);
                                    if (dictionaryItem != null)
                                    {
                                        userValue.Reference.Id = dictionaryItem.Id;
                                    }
                                    else
                                    {
                                        ErrorMessage warning = new ErrorMessage();
                                        warning.Severity = ErrorMessageTypes.WARNING;
                                        warning.Message = (String.Format("Не найдено значение пользовательского поля. Пользовательский справочник \"{0}\". Значение \"{1}\"", userDictionary.Name, userValue.Value));
                                        warnings.Add(warning);
                                    }
                                }
                                else
                                {
                                    ErrorMessage warning = new ErrorMessage();
                                    warning.Severity = ErrorMessageTypes.WARNING;
                                    warning.Message = (String.Format("Пользовательский справочник не найден. ID справочника = {0}", userField.UserDirectory.Id));
                                    warnings.Add(warning);
                                }
                                break;
                            // Если пользовательское поле является множеством, то необходимо проинициализировать ссылки на значения пользовательских справочников
                            case (int)UserFieldTypes.SET:
                                if (userDictionary != null)
                                {
                                    String[] valueSet = userValue.Value.Trim().Split(new Char[] { ',' });
                                    foreach (String value in valueSet)
                                    {
                                        if (value.Trim() != "")
                                        {
                                            // Ищем значение пользовательского справочника по имени, указанному внешней системой
                                            var dictionaryItem = (UserDictionaryValue)userDictionary.FindValueByName(value);
                                            if (dictionaryItem != null)
                                            {
                                                ObjectRef reference = new ObjectRef();
                                                reference.Id = dictionaryItem.Id;
                                                userValue.Values.Add(reference);
                                            }
                                            else
                                            {
                                                ErrorMessage warning = new ErrorMessage();
                                                warning.Severity = ErrorMessageTypes.WARNING;
                                                warning.Message = (String.Format("Не найдено значение пользовательского поля. Пользовательский справочник \"{0}\". Значение \"{1}\"", userDictionary.Name, value));
                                                warnings.Add(warning);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ErrorMessage warning = new ErrorMessage();
                                    warning.Severity = ErrorMessageTypes.WARNING;
                                    warning.Message = (String.Format("Пользовательский справочник не найден. ID справочника = {0}", userField.UserDirectory.Id));
                                    warnings.Add(warning);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        ErrorMessage warning = new ErrorMessage();
                        warning.Severity = ErrorMessageTypes.WARNING;
                        warning.Message = (String.Format("Пользовательское поле не найдено по коду \"{0}\" ", userValue.Code));
                        warnings.Add(warning);
                    }
                }
                userValues.RemoveAll(uv => uv.UserField.Id == 0);
            }
        }

        public static void PrepareUserFieldsValuesByCodes(List<OutsourceUserField> userFieldsValues, List<ErrorMessage> messages)
        {
            lock (ProgramContext.Dictionaries)
            {
                UserFieldDictionary userFieldsDict = (UserFieldDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.UserField];

                foreach (OutsourceUserField userFieldValue in userFieldsValues)
                {
                    // Ищем пользовательское поле по коду, указанному внешней системой
                    var userField = (UserFieldDictionaryItem)userFieldsDict.Find(userFieldValue.Code);
                    if (userField != null)
                        userFieldValue.UserField.Id = userField.Id;
                    else
                    {
                        ErrorMessage warning = new ErrorMessage();
                        warning.Severity = ErrorMessageTypes.WARNING;
                        warning.Message = (String.Format("Пользовательское поле не найдено по коду \"{0}\" ", userFieldValue.Code));
                        messages.Add(warning);
                    }
                }
                userFieldsValues.RemoveAll(uv => uv.UserField.Id == 0);
            }
        }

        public static void PrepareOutputUserFieldsValuesByIds(List<UserValue> userValues)
        {
            lock (ProgramContext.Dictionaries)
            {
                UserFieldDictionary userFieldsDict = (UserFieldDictionary)ProgramContext.Dictionaries.GetDictionary(LimsDictionaryNames.UserField);
                UserDirectoryDictionary userDirectoryDict = (UserDirectoryDictionary)ProgramContext.Dictionaries.GetDictionary(LimsDictionaryNames.UserDirectory);

                foreach (UserValue userValue in userValues)
                {
                    // Ищем пользовательское поле по коду, указанному внешней системой
                    var userField = (UserFieldDictionaryItem)userFieldsDict.Find(userValue.Code);
                    if (userField != null)
                    {
                        userValue.UserField.Id = userField.Id;
                        var userDictionary = userField.UserDirectory != null ? (UserDirectoryDictionaryItem)userDirectoryDict.Find(userField.UserDirectory.Id) : null;
                        switch (userField.FieldType)
                        {
                            // Если пользовательское поле является перечислимым, то необходимо получить значение ползовательского справочника по ссылке
                            case (int)UserFieldTypes.ENUMERATION:
                                if (userDictionary != null)
                                {
                                    // Ищем значение пользовательского справочника по ссылке
                                    var dictionaryItem = (UserDictionaryValue)userDictionary.Find(userValue.Reference.Id);
                                    if (dictionaryItem != null)
                                    {
                                        userValue.Value = dictionaryItem.Name;
                                    }
                                    else
                                        Log.WriteError(String.Format("User value not found. User directory \"{0}\". DictionaryItemId = {1}", userDictionary.Name, userValue.Reference.Id));
                                }
                                else
                                    Log.WriteError(String.Format("User directory not found. DirectoryId = {0}", userField.UserDirectory.Id));
                                break;
                            // Если пользовательское поле является множеством,  то необходимо получить значения пользовательских справочников по ссылкам 
                            case (int)UserFieldTypes.SET:
                                if (userDictionary != null)
                                {
                                    List<String> sl = new List<string>();
                                    foreach (ObjectRef reference in userValue.Values)
                                    {
                                        if (reference.Id > 0)
                                        {
                                            // Ищем значение пользовательского справочника по ccылке
                                            var dictionaryItem = (UserDictionaryValue)userDictionary.Find(reference.Id);
                                            if (dictionaryItem != null)
                                                sl.Add(dictionaryItem.Name);
                                            else
                                                Log.WriteError(String.Format("User value not found. User directory \"{0}\". DictionaryItemId = {1}", userDictionary.Name, reference.Id));
                                        }
                                    }
                                    userValue.Value = String.Join(",", sl);
                                }
                                else
                                    Log.WriteError(String.Format("User directory not found. DirectoryId = {0}", userField.UserDirectory.Id));
                                break;
                            default:
                                break;
                        }
                    }
                    else
                        Log.WriteError(String.Format("UserField not found. UserField Code = \"{0}\" ", userValue.Code));
                }
            }
        }

        /*  public static void PrepareOutputUserFieldValues(List<UserValue> userValues)
          {
              lock (ServiceContext.DictionaryCash)
              {
                  UserFieldDictionary userFieldsDict = (UserFieldDictionary)ServiceContext.DictionaryCash.GetDictionary(LisDictionaryNames.UserField);
                  UserDirectoryDictionary userDirectoryDict = (UserDirectoryDictionary)ServiceContext.DictionaryCash.GetDictionary(LisDictionaryNames.UserDirectory);

                  foreach (UserValue userValue in userValues)
                  {
                      // Ищем пользовательское поле по коду, указанному внешней системой
                      var userField = (UserFieldDictionaryItem)userFieldsDict.Find(userValue.Code);
                      if (userField != null)
                      {
                          userValue.UserField.ID = userField.Id;
                          var userDictionary = (UserDirectoryDictionaryItem)userDirectoryDict.Find(userField.UserDirectory.ID);
                          switch (userField.FieldType)
                          {
                              // Если пользовательское поле является перечислимым, то необходимо получить значение ползовательского справочника по ссылке
                              case (int)UserFieldTypes.ENUMERATION:
                                  if (userDictionary != null)
                                  {
                                      // Ищем значение пользовательского справочника по ссылке
                                      var dictionaryItem = (UserDictionaryValueClass)userDictionary.Find(userValue.Reference.ID);
                                      if (dictionaryItem != null)
                                      {
                                          userValue.Value = dictionaryItem.Name;
                                      }
                                      else
                                          Log.WriteError(String.Format("User value not found. User directory \"{0}\". DictionaryItemId = {1}", userDictionary.Name, userValue.Reference.ID));
                                  }
                                  else
                                      Log.WriteError(String.Format("User directory not found. DirectoryId = {0}", userField.UserDirectory.ID));
                                  break;
                              // Если пользовательское поле является множеством,  то необходимо получить значения пользовательских справочников по ссылкам 
                              case (int)UserFieldTypes.SET:
                                  if (userDictionary != null)
                                  {
                                      List<String> sl = new List<string>();
                                      foreach (ObjectRef reference in userValue.Values)
                                      {
                                          if (reference.ID > 0)
                                          {
                                              // Ищем значение пользовательского справочника по ccылке
                                              var dictionaryItem = (UserDictionaryValueClass)userDictionary.Find(reference.ID);
                                              if (dictionaryItem != null)
                                                  sl.Add(dictionaryItem.Name);
                                              else
                                                  Log.WriteError(String.Format("User value not found. User directory \"{0}\". DictionaryItemId = {1}", userDictionary.Name, reference.ID));
                                          }
                                      }
                                      userValue.Value = String.Join(",", sl);
                                  }
                                  else
                                      Log.WriteError(String.Format("User directory not found. DirectoryId = {0}", userField.UserDirectory.ID));
                                  break;
                              default:
                                  break;
                          }
                      }
                      else
                          Log.WriteError(String.Format("UserField not found. UserField Code = \"{0}\" ", userValue.Code));
                  }

              }
          }
    */
    }
}