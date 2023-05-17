using System;
using System.Collections.Generic;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class BatchElementClass
    {
        public int rank = 1;
        public string nrValue = string.Empty;
        public string firstName = string.Empty;
        public string lastName = string.Empty;
        public string middleName = string.Empty;
        public int sex = 0;
        public ObjectRef donationTemplate = new ObjectRef();
        public List<BloodParameterValue> bloodParameters = new List<BloodParameterValue>();
        public int volume = 0;
        public ObjectRef donor = new ObjectRef();
        public ObjectRef donation = new ObjectRef();
    }

    public class Batch : BaseObject
    {
        public DateTime date = DateTime.Now;
        public ObjectRef department = new ObjectRef();
        public int mode = 2;
        public bool checkDonorInfo = false;
        public bool checkNr = false;
        public string comment = string.Empty;
        public List<BatchElementClass> elements = new List<BatchElementClass>();
        public List<BloodParameterValue> bloodParameters = new List<BloodParameterValue>();
    }

    public class BatchActivateRequest
    {
        public List<ObjectRef> ids = new List<ObjectRef>();
        public int state = 2;
    }

    public class BatchSaveResult : BaseSaveResult
    {

    }
}
