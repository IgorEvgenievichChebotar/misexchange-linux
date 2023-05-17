using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.HemDictionary;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class VisitStage
    {
        public bool active = false;
        private DateTime endDate;
        public int state = 0;
        private string comment = string.Empty;
        public List<ParameterValue> values = new List<ParameterValue>();
        public int stageType = 0;
        private ObjectRef lisRequest = new ObjectRef();
        public int lisState = 0;

        [CSN("Active")]
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        [CSN("EndDate")]
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        [CSN("State")]
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        [CSN("Comment")]
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        [CSN("Values")]
        public List<ParameterValue> Values
        {
            get { return values; }
            set { values = value; }
        }

        [CSN("StageType")]
        public int StageType
        {
            get { return stageType; }
            set { stageType = value; }
        }

        [CSN("LisRequest")]
        public ObjectRef LisRequest
        {
            get { return lisRequest; }
            set { lisRequest = value; }
        }

        [CSN("LisState")]
        public int LisState
        {
            get { return lisState; }
            set { lisState = value; }
        }

        private void PrepareValues(List<ParameterValue> ValueList)
        {
           
        }

        public Parameter FindParameter(DictionaryClass<ParameterGroup> ParameterGroup, int Id, ref int ParmeterGroupId)
        {
            foreach (ParameterGroup ParameterSet in ParameterGroup.DictionaryElements)
            {
                Parameter Parameter = ParameterSet.Find(Id);
                if (Parameter != null)
                {
                    ParmeterGroupId = ParameterSet.Id;
                    return Parameter;
                }
            }
            return null;
        }
    }
}
