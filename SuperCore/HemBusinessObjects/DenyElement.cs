using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.HemDictionary;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{

    public class DenyElementClass
    {
        // ФИО

        public string lastName = string.Empty;
        public string firstName = string.Empty;
        public string middleName = string.Empty;
        public string FullName = string.Empty;


        //  Дата рождения
        public int birthDay = 0;
        public int birthMonth = 0;
        public int birthYear = 0;
        public string displayBirthday = string.Empty;

        //Адрес
        public AddressClass address = new AddressClass();
        public string displayAddress = string.Empty;


        // Отвод 
        public int findYear = 0;
        public int findMonth = 0;
        public int findDay = 0;
        public string displayDenyDate = string.Empty;

        //Диагноз
        public string diagnosis = string.Empty;


        //Отвод и источник отвода
        public ObjectRef source = new ObjectRef();
        public ObjectRef deny = new ObjectRef();
        public string displayDenyName = string.Empty;
        public string displayDenySource = string.Empty;

        public bool removed = false;
        public void Prepare()
        {
            //firstName = FormatName(firstName);
            //lastName = FormatName(lastName);
            //middleName = FormatName(middleName);

            //displayBirthday = GetDisplayDate(birthDay, birthMonth, birthYear);
            //displayDenyDate = GetDisplayDate(findDay, findMonth, findYear);
            //displayDenyName = GetDenyName();
            //displayDenySource = GetDenySorce();

            //if (displayBirthday.Length > 10)
            //{ displayBirthday = displayBirthday.Substring(0, 10); }

            //if (displayDenyDate.Length > 10)
            //{ displayDenyDate = displayDenyDate.Substring(0, 10); }

            //FullName = lastName + " " + firstName + " " + middleName;
            //displayAddress = address.displayName;


        }



        private string GetDisplayDate(int birthDay, int birthMonth, int birthYear)
        {


            string dd = string.Empty;
            string mm = string.Empty;
            string yyyy = string.Empty;

            try
            { dd = string.Format("{0:D2}", Convert.ToInt32(birthDay)); }
            catch
            { dd = birthDay.ToString(); }

            try
            { mm = string.Format("{0:D2}", Convert.ToInt32(birthMonth)); }
            catch
            { mm = birthMonth.ToString(); }


            yyyy = birthYear.ToString();

            return dd + "." + mm + "." + yyyy;
        }

        public string FormatName(string StrIn)
        {
            if (StrIn.Length == 0) return "-";
            return StrIn.Substring(0, 1).ToUpper() + StrIn.Substring(1, StrIn.Length - 1).ToLower();
        }

        //private string GetDenyName()
        //{
        //    HttpApplicationState Application = HttpContext.Current.Application;
        //    SettingsClass Settings = (SettingsClass)Application.Contents[ApplicationVariablesConsts.Settings];
        //    DictionaryCashClass DictionaryCash = (DictionaryCashClass)Application.Contents[ApplicationVariablesConsts.Dictionaries];
        //    DictionaryClass<DenyDictionaryItemClass> DenyDictionary = (DictionaryClass<DenyDictionaryItemClass>)DictionaryCash.Denies;


        //    DenyDictionaryItemClass donorDeny = (DenyDictionaryItemClass)DenyDictionary.Find(deny.GetRef());
        //    if (donorDeny != null)
        //    {
        //        return donorDeny.name;

        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        //private string GetDenySorce()
        //{
        //    HttpApplicationState Application = HttpContext.Current.Application;
        //    SettingsClass Settings = (SettingsClass)Application.Contents[ApplicationVariablesConsts.Settings];
        //    DictionaryCashClass DictionaryCash = (DictionaryCashClass)Application.Contents[ApplicationVariablesConsts.Dictionaries];
        //    DictionaryClass<DenySourceDictionaryItemClass> DenyDictionary = (DictionaryClass<DenySourceDictionaryItemClass>)DictionaryCash.DenySource;


        //    DenySourceDictionaryItemClass denySource = (DenySourceDictionaryItemClass)DenyDictionary.Find(source.GetRef());
        //    if (denySource != null)
        //    {
        //        return denySource.name;

        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}
    }


    public class DenyElementRequest
    {
        // ФИО
        public string lastName;
        public string firstName;
        public string middleName;
        public string birthYear;
    }

    public class DenyElementJournalItemSet
    {
        public List<DenyElementClass> result = new List<DenyElementClass>();
        public void Prepare()
        {

            result.RemoveAll(deny => deny.removed);
            foreach (DenyElementClass Deny in result)
            {

                Deny.Prepare();
            }
        }
    }


}
