using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class AddressClass : BaseObject
    {
        private string manualCityIndex = string.Empty;
        private string manualStreetIndex = string.Empty;
        private string manualCityName = string.Empty;
        private string cityIndex = string.Empty;
        private string manualStreetName = string.Empty;
        private string streetIndex = string.Empty;
        private string streetName = string.Empty;
        public string building = string.Empty;
        public string code = string.Empty;

        public AddressClass()
        {
            DisplayName = "";
            District = "";
        }

        [CSN("Code")]
        public string Code { get; set; }

        [CSN("Version")]
        public int Version { get; set; }

        [CSN("DisplayName")]
        public string DisplayName { get; set; }

        [CSN("ManualCityName")]
        public string ManualCityName { get; set; }

        [CSN("ManualStreetName")]
        public string ManualStreetName { get; set; }

        [CSN("Building")]
        public string Building { get; set; }

        [CSN("CityIndex")]
        public string CityIndex { get; set; }

        [CSN("StreetIndex")]
        public string StreetIndex { get; set; }

        [CSN("ManualCityIndex")]
        public string ManualCityIndex { get; set; }

        [CSN("ManualStreetIndex")]
        public string ManualStreetIndex { get; set; }

        [CSN("District")]
        public string District { get; set; }

        [CSN("RegistrationExpireDate")]
        public DateTime? RegistrationExpireDate { get; set; }


        public override string ToString()
        {
            return this.DisplayName;
        }
        
      
        [SendToServer(false)]
        [CSN("StreetName")]
        public string StreetName { get; set; }

        [SendToServer(false)]
        [CSN("IsManualTown")]
        public bool IsManualTown
        { get {return (manualCityIndex != string.Empty); } }

        [SendToServer(false)]
        [CSN("IsManualStreet")]
        public bool IsManualStreet
        { get { return (manualStreetIndex != string.Empty); } }

        public void BuildIndexes()
        {
            if (manualCityName != string.Empty)
            {
                manualCityIndex = MakeAdressIndex(manualCityName);
            }
            else
            {
                cityIndex = MakeAdressIndex(cityIndex);
            }

            if (manualStreetName != string.Empty)
            {
                manualStreetIndex = MakeAdressIndex(manualStreetName);
            }
            else
            {
                streetIndex = MakeAdressIndex(streetName);
            }
        }

        private string MakeAdressIndex(string Name)
        {
            string Result = string.Empty;
            Result = "|" + Name + "|";
            Result = Result.Replace(' ', '|');
            return Result;
        }

        public string GetHouse()
        {
            return GetBuildingElement(0);
        }

        public string GetBuilding()
        {
            return GetBuildingElement(1);
        }

        public string GetBuild()
        {
            return GetBuildingElement(2);
        }

        public string GetFlat()
        {
            return GetBuildingElement(3);
        }

        private string GetBuildingElement(int Index)
        {
            List<string> Values = new List<string>(Building.Split(new char[] { '|' }));
            if (Values.Count != 4)
            { return string.Empty; }
            return Values[Index];
        }

        public long GetRegion()
        {
            if ((Code != null) && (Code.Length > 2))
            {
                return Convert.ToInt64(Code.Substring(0, 2))*100000000000;
            }
            else
            {
                return 0;
            }
        }

        public long GetArea()
        {
            if ((Code != null) && (Code.Length > 5))
            {
                return Convert.ToInt64(Code.Substring(0, 5)) * 100000000;
            }
            else
            {
                return 0;
            }
        }

        public long GetCity()
        {
            if ((Code != null) && (Code.Length > 8))
            {
                return Convert.ToInt64(Code.Substring(0, 8)) * 100000;
            }
            else
            {
                return 0; 
            }
        }

        public long GetTown()
        {
            if ((Code != null) && (Code.Length > 11))
            {
                return Convert.ToInt64(Code.Substring(0, 11)) * 100;
            }
            else
            {
                return 0;
            }
        }

        public long GetStreet()
        {
            if ((Code != null) && (Code.Length > 11))
            {
                return Convert.ToInt64(Code.PadRight(17,'0'));
            }
            else
            {
                return 0;
            }
        }
    }
}