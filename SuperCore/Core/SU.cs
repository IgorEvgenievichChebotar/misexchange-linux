//#define built

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ru.novolabs.SuperCore
{
    public static class SecurityUtils
    {
        internal static PropertyInfo GetCustomIdProperty(this Type type)
        {
            return type.GetProperties().FirstOrDefault(pi => pi.GetDecryptedPropName().Equals("Id"));
        }

        public static PropertyInfo GetCustomProperty(this Type type, string propName)
        {
            return type.GetProperties().FirstOrDefault(pi => pi.GetDecryptedPropName().Equals(propName));
        }
        public static PropertyInfo GetCustomPropertyApproximately(this Type type, string propName)
        {
            return type.GetProperties().FirstOrDefault(pi => propName.StartsWith(pi.GetDecryptedPropName()));
        }

        public static PropertyInfo GetCustomSubProperty(this Type type, string propName)
        {
            if (propName.Contains('.'))
            {
                List<String> propNames = propName.Split('.').ToList();
                PropertyInfo curProp = null;
                Type t = type;
                foreach (String name in propNames)
                {
                    curProp = t.GetProperties().FirstOrDefault(pi => pi.GetDecryptedPropName().Equals(name));
                    if (curProp == null)
                        return null;
                    t = curProp.PropertyType;
                }
                return curProp;
            }
            else
                return type.GetProperties().FirstOrDefault(pi => pi.GetDecryptedPropName().Equals(propName));
        }


        public static Object GetValue(this PropertyInfo propInfo, Object obj, string fullPropName, Object[] index)
        {
            if (fullPropName.Contains('.'))
            {
                List<String> propNames = fullPropName.Split('.').ToList();
                PropertyInfo curProp = null;
                Type t = obj.GetType();
                Object subobj = obj;
                foreach (String name in propNames)
                {
                    curProp = t.GetProperties().FirstOrDefault(pi => pi.GetDecryptedPropName().Equals(name));
                    if (curProp == null)
                        return null;
                    t = curProp.PropertyType;
                    subobj = curProp.GetValue(subobj, null);
                }
                return subobj;
            }
            else
                return propInfo.GetValue(obj, null);
        }



        public static PropertyInfo GetFieldByAttributeName(this Object obj, string propertyName)
        {
            foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
            {
                if (propInfo.IsDefined(typeof(CSN), false))
                    if (((CSN)propInfo.GetCustomAttributes(typeof(CSN), false)[0]).GetSp3().Equals(propertyName))
                    {
                        return propInfo;
                    }
            }
            return null;
        }

        internal static string EncryptPropertyName(string propName)
        {
            if (!String.IsNullOrEmpty(propName))
            {
                propName = propName.Substring(0, 1).ToUpper() + propName.Substring(1);
#if (built)
                int length = 32;
                int garbageLength = length - propName.Length - 1;
                if (propName.Length < length - 1)
                {
                    propName += '⻆';
                    int charIndex = -1;
                    for (int i = 0; i < garbageLength; i++)
                    {
                        charIndex = charIndex > propName.Length - 2 ? 0 : charIndex + 1;
                        char gc = (char)((int)propName[charIndex] + 1);
                        propName += gc;
                    }
                }
                else
                    length = propName.Length;

                char[] chArray = propName.ToCharArray();
                while (--length >= 0)
                    chArray[length] = (char)(chArray[length] ^ NumValues.CEE7989355E64B0CBEE0AD6B7C46B07E);

                Array.Reverse(chArray);
                propName = new string(chArray);
#endif
            }

            return propName;
        }

        internal static PropertyInfo GetObfuscatedProperty(this Type type, string encryptedPropName)
        {
            if (type.IsAnonymousType())
                return type.GetProperty(encryptedPropName);
            else
                return type.GetProperties().FirstOrDefault(pi =>
                    pi.IsDefined(typeof(CSN), false)
                    && (((CSN)pi.GetCustomAttributes(typeof(CSN), false)[0]).GetSp3() == encryptedPropName));
        }

        internal static string GetDecryptedPropName(this PropertyInfo propInfo)
        {
            if (propInfo.IsDefined(typeof(CSN), false))
            {
#if (built)
                return DecryptPropName(((CSN)propInfo.GetCustomAttributes(typeof(CSN), false)[0]).GetSp3());
#else
                return ((CSN)propInfo.GetCustomAttributes(typeof(CSN), false)[0]).GetSp3();
#endif
            }
            else
                return String.Empty;
        }

        private static string DecryptPropName(string encryptedPropName)
        {
            char[] chArray = encryptedPropName.ToCharArray();
            int length = chArray.Length;
            Array.Reverse(chArray);
            while (--length >= 0)
                chArray[length] = (char)(chArray[length] ^ NumValues.CEE7989355E64B0CBEE0AD6B7C46B07E);

            return new String(chArray).Split('⻆').First();
        }
    }

    public class BaseCSN : BaseBaseCSN
    {
        public BaseCSN(string typeId) : base(typeId) { }
    }

    public class BaseBaseCSN : BaseBaseBaseCSN
    {
        public BaseBaseCSN(string typeId) : base(typeId) { }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BaseBaseBaseCSN : Attribute
    {
        private int sp1;
        private string sp2;
        private string sp3;
        private int sp4;
        private bool sp5;
        private string sp6;

        internal BaseBaseBaseCSN(string p1)
        {
            sp6 = p1 != "" && p1 != null ? null : (string)CalcP1(p1);
            sp3 = sp6 != null ? String.Join("ผรฦ", new object[] { "ᘀᘹӓ", null }) : p1;
            sp1 = (sp6 != null ? sp6.Length : -1) + (sp3 != null ? sp3.Length : 0);
            sp4 = (sp1 + 17) / 2;
            sp5 = sp4 > sp1 ? !GetT() : sp1 > sp4;
            sp2 = sp5.ToString().ToString();
        }

        private object CalcP1(string z1)
        {
            if (!GetT())
                return (from z in new uint[] { 56378465, 8798223, 263432423, 7657467 }.ToList() select z.GetType().FullName).Union(from item in new List<String>(new string[] { z1, z1 + "ᘀᘹ𠘮ቕ" }) select item.ToString()).ToList().Find(weur => weur == "ᘀᘹ𠘮");
            else
                return CalcY(z1);
        }

        private object CalcY(string r1)
        {
            return new string[] { "Ōל۽۱ᘷᘀᘹ𠘮", r1, null, "򋈒ᘀᘹӓ" }.ToList().Find(dt => new string[] { "ผรฦ", "Ōל۽۱ᘷ" + r1 }.ToList().Contains(dt));
        }

        internal string GetSp3()
        {
            return sp3;
        }

        internal int GetSp1()
        {
            return sp1;
        }

        internal bool GetSp5()
        {
            return sp5;
        }

        private bool GetT()
        {
            return !sp5;
        }

        internal string GetSp6()
        {
            return sp6;
        }
    }
}
