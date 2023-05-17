using MisExchangeAdapters.Parser.HL7.Classes;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ru.novolabs.HL7.Files;
using ru.novolabs.ExchangeDTOs;

namespace MisExchangeAdapters.Parser.HL7
{
    public static class HL7MessageParser
    {
       // public static HelperSettings HelperSetting { get; set; } 
        private static void ParsePid(List<String> data, ru.novolabs.ExchangeDTOs.Request requestInfo)
        {
            requestInfo.Patient.Code = data[3];

            //requestInfo.HospitalCode = data[3].Split(new char[] { '^' }).ToList()[0].Split(new char[] { '/' }).ToList()[0];
            

            List<String> patientInfo = data[5].Split(new char[] { '^' }).ToList();

            requestInfo.Patient.FirstName = patientInfo[1];
            requestInfo.Patient.LastName = patientInfo[0];
            requestInfo.Patient.MiddleName = patientInfo[2];
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            DateTime birthDate = DateTime.ParseExact(data[7], "yyyyMMdd", cultureInfo);

            requestInfo.Patient.BirthYear = birthDate.Year;
            requestInfo.Patient.BirthMonth = birthDate.Month;
            requestInfo.Patient.BirthDay = birthDate.Day;


            switch (data[8])
            {
                case HL7Sex.Male:
                    requestInfo.Patient.Sex = 1;
                    break;
                case HL7Sex.Female:
                    requestInfo.Patient.Sex = 2;
                    break;
                default:
                    requestInfo.Patient.Sex = 0;
                    break;
            }

            List<String> patientAddress = data[11].Split(new char[] { '^' }).ToList();
            if (patientAddress.Count() > 0)
            {
                requestInfo.Patient.Country = patientAddress[0];
                if (patientAddress.Count >= 2)
                {
                    requestInfo.Patient.City = patientAddress[3];
                    if (patientAddress.Count >= 3)
                    {
                        requestInfo.Patient.Street = patientAddress[4];
                        if (patientAddress.Count >= 4)
                        {
                            requestInfo.Patient.Building = patientAddress[5];
                        }
                        if (patientAddress.Count >= 6)
                            requestInfo.Patient.Flat = patientAddress[7];
                    }
                }
            }

            requestInfo.Patient.PolicyNumber = data[19];

        }

        private static void ParsePv1(List<String> data, ru.novolabs.ExchangeDTOs.Request requestInfo)
        {
            List<String> doctorInfo = data[8].Split(new char[] { '^' }).ToList();

            if (doctorInfo.Count >= 4)
            {
                requestInfo.DoctorCode = doctorInfo[0];
                requestInfo.DoctorName = doctorInfo[1] + " " + doctorInfo[2] + " " + doctorInfo[3];
            }
            else
            {
                requestInfo.DoctorCode = doctorInfo[0];
            }

            requestInfo.DepartmentCode = data[39];
            requestInfo.DepartmentName = "Code: " + data[39];
        }

        private static void ParseObrTests(List<String> data, ru.novolabs.ExchangeDTOs.Request request)
        {
            ru.novolabs.ExchangeDTOs.Sample newSample = null;
            ru.novolabs.ExchangeDTOs.Target newTarget = new ru.novolabs.ExchangeDTOs.Target();
            

            List<String> tests = data[4].Split(new char[] { '^' }).ToList();
            newTarget.Code = tests[3];
           
            if (data[5] == "")
                newTarget.Priority = 0;
            else
                newTarget.Priority = 1;

            TargetDictionaryItem target = (TargetDictionaryItem) ProgramContext.Dictionaries[LimsDictionaryNames.Target, newTarget.Code];
            if (target == null)
                throw new Exception(String.Format("Нет такого исследования с кодом '{0}'", newTarget.Code));

            BiomaterialDictionaryItem biomaterial = null;

            string[] biomaterialParts = data[15].Split(new char[] {'~'});
            if (biomaterialParts.Count() < 2)
                throw new Exception(String.Format("Биоматериал не найден в сегменте OBR поля 15, позиции 2: {0}", data[15]));
            string biomaterialCode = biomaterialParts[1];
            biomaterial = (BiomaterialDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.Biomaterial, biomaterialCode];
            if (biomaterial == null)
                throw new Exception(String.Format("Биоматериал по коду '{0}' не найден", biomaterialCode));
            //if (target.DefaultBiomaterial != null)
            //{
            //    biomaterial = target.DefaultBiomaterial;
            //}
            //else
            //{
            //    if (target.Biomaterials.Count > 0)
            //        biomaterial = target.Biomaterials[0];
            //}
            //if (biomaterial == null)
            //    throw new Exception(String.Format("Исследование {0} не имеет биоматериалов", target.Code));

            foreach (ru.novolabs.ExchangeDTOs.Sample sample in request.Samples)
            {
                if (sample.BiomaterialCode == biomaterial.Code)
                    newSample = sample;
            }
            if (newSample == null)
            {
                newSample = new ru.novolabs.ExchangeDTOs.Sample();
                newSample.BiomaterialCode = biomaterial.Code;
                List<String> sampleNrs = data[3].Split(new char[] { '^' }).ToList();
                newSample.Barcode = sampleNrs[0];
                request.Samples.Add(newSample);
            }
            newSample.Targets.Add(newTarget);
        }

        private static void ParseObr(List<String> data, ru.novolabs.ExchangeDTOs.Request request)
        {
            if (String.IsNullOrEmpty(request.RequestCode))
            {
                List<String> requestCode = data[3].Split(new char[] { '^' }).ToList();
                request.RequestCode = requestCode[0];

            }
            ParseObrTests(data, request);
        }

        private static void ParseOrc(List<String> data, ru.novolabs.ExchangeDTOs.Request requestInfo)
        {
            //
        }


        #region Spicific for HL7 over TCP-protocol exchange
        private static MessageHeader GenerateAnswerMessageHeader(List<String> data)
        {
            MessageHeader result = new MessageHeader();
            MessageHeader input = new MessageHeader();
            input.SegmentName = data[0];
            input.Delimeters = data[1];
            input.AppSender = data[2];
            input.EqSender = data[3];
            input.AppReceiver = data[4];
            input.EqReceiver = data[5];

            CultureInfo cult = CultureInfo.InvariantCulture;
            input.MessageDate = DateTime.ParseExact(data[6], "yyyyMMddHHmmss", cult);
            input.MessageDefence = data[7];
            input.MessageType = data[8];
            input.MessageId = data[9];
            input.ExecuteCode = data[10];
            input.Version = data[11];
            
            result = input.CopyViaSerialization();
            result.AppSender = input.AppReceiver;
            result.AppReceiver = input.AppSender;
            result.EqSender = input.EqReceiver;
            result.EqReceiver = input.EqSender;

            List<String> msgType = result.MessageType.Split(new char[] { '^' }).ToList();
            msgType[0] = "ACK";
            result.MessageType = String.Join("^", msgType[0], msgType[1]);
            return result;
        }

        private static EventType GenerateEventType(List<String> data, String Error)
        {
            EventType res = new EventType();
            if (Error == null)
                res.MessageType = MessageTypes.ApplicationAccept;
            else
                res.MessageType = MessageTypes.ApplicationError;
            res.MessageId = data[2];
            res.SegmentName = "MSA";
            res.TextMessage = Error;
            return res;
        }

        public static String BuildAck(List<String> data, String Error = null)
        {
            List<String> frames = data;
            MessageHeader header = null;
            EventType eventType = null;
            foreach (String frame in frames)
            { 
                List<String> currentFrameFields = frame.Split(new char[] { '|' }).ToList();
                switch (currentFrameFields[0])
                {
                    case HL7BlockConst.MSH:
                        header = GenerateAnswerMessageHeader(currentFrameFields);
                        break;
                    case HL7BlockConst.MSA:
                        eventType = GenerateEventType(currentFrameFields, Error);
                        break;
                    case HL7BlockConst.ERR:
                        break;
                }
            }

            List<String> result = new List<string>();
            if (header != null && eventType != null)
            {
                result.Add(header.ToString());
                result.Add(eventType.ToString());
            }
            String res = "";
            foreach (String str in result)
            {
                res += str + "\r\n";
            }
            return res;
            
        }
        #endregion

        public static ru.novolabs.ExchangeDTOs.Request BuildDTORequest(String data)
        {
            List<String> frames = data.Split(new String[] { "\r" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            ru.novolabs.ExchangeDTOs.Request requestInfo = ru.novolabs.ExchangeDTOs.DTOInitializer.BuildEmptyRequest();//new ru.novolabs.ExchangeDTOs.Request();
            
            foreach (String frame in frames)
            {
                List<String> currentFrameFields = frame.Split(new char[] { '|' }).ToList();
                switch (currentFrameFields[0])
                {
                    case HL7BlockConst.MSH:
                        break;
                    case HL7BlockConst.PID:
                        ParsePid(currentFrameFields, requestInfo);
                        break;
                    case HL7BlockConst.PV1:
                        ParsePv1(currentFrameFields, requestInfo);
                        break;
                    case HL7BlockConst.OBR:
                        ParseObr(currentFrameFields, requestInfo);
                        break;
                    case HL7BlockConst.ORC:
                        ParseOrc(currentFrameFields, requestInfo);
                        break;
                    default:
                        break;
                }
            }

            return requestInfo;
        }


        //=========================================

        private static String BuildPidFrame(ru.novolabs.ExchangeDTOs.Result Result, List<string> pidCache)
        {

            PatientInfo patientInfo = new PatientInfo();
            patientInfo.SegmentType = "PID";
            //patientInfo.PatientId = Request.Patient.Code;
           // patientInfo.FullPatientId = String.Join(HL7SeparatorConst.ComponentSeparator.ToString(), Request.HospitalName + "/" + Request.HospitalCode, "", "", "", "UR");
            //patientInfo.FullPatientId = Request.Patient.Code;);
            if (pidCache != null)
            {
                patientInfo.FullPatientId = pidCache[2];
            }
            patientInfo.AlterPatientId = Result.Patient.Code;
            patientInfo.PatientName = new PatientName()
            {
                FirstName = Result.Patient.FirstName,
                MiddleName = Result.Patient.MiddleName,
                LastName = Result.Patient.LastName
            };
            if (Result.Patient.BirthYear.HasValue &&
                Result.Patient.BirthMonth.HasValue &&
                Result.Patient.BirthDay.HasValue &&
                Result.Patient.BirthYear != 0 &&
                Result.Patient.BirthMonth != 0 &&
                Result.Patient.BirthDay != 0)
            {
                patientInfo.PatientBirthDate = new DateTime(
                Result.Patient.BirthYear.Value,
                Result.Patient.BirthMonth.Value,
                Result.Patient.BirthDay.Value
                );
            }
            switch (Result.Patient.Sex)
            {
                case 0:
                    patientInfo.PatientSex = HL7Sex.Other;
                    break;
                case 1:
                    patientInfo.PatientSex = HL7Sex.Male;
                    break;
                case 2:
                    patientInfo.PatientSex = HL7Sex.Female;
                    break;
            }

            patientInfo.PatientAddress = "";
            patientInfo.PatientInsurance = Result.Patient.InsuranceCompany;

            return patientInfo.ToString();
        }
        private static String BuildMshFrame(ru.novolabs.ExchangeDTOs.Result result, List<string> mshCache)
        {
            MessageHeader header = new MessageHeader();

            header.SegmentName = "MSH";
            header.Delimeters = new string(new char[] { '^', '~', '\\', '&' });
            //recombination of sender and receiving parameters
            if (mshCache != null)
            {
                header.AppSender = mshCache[4];
                header.EqSender = mshCache[5];
                header.AppReceiver = mshCache[2];
                header.EqReceiver = mshCache[3];
            }
            header.MessageDate = DateTime.Now;
            header.MessageType = "ORU^R01";
            header.MessageId = "6";
            header.ExecuteCode = "P";
            header.Version = "2.3";

            return header.ToString();
        }

        private static List<String> BuildWorkResultsFrames(ru.novolabs.ExchangeDTOs.Result resultData, Dictionary<string, List<string>> obrCache)
        {
            List<ru.novolabs.ExchangeDTOs.SampleResult> results = resultData.SampleResults;
            List<String> result = new List<string>();

            int index = 1;
            foreach (ru.novolabs.ExchangeDTOs.SampleResult sampleResult in results)
            {
                foreach(ru.novolabs.ExchangeDTOs.TargetResult targetResult in sampleResult.TargetResults)
                {
                    if (targetResult.Code == "default")
                        continue;

                    TargetRequest currentRequest = new TargetRequest();

                    currentRequest.SegmentType = "OBR";
                    currentRequest.SegmentId = index.ToString();
                    if (obrCache != null)
                    {
                        currentRequest.TaskIdMis = obrCache[targetResult.Code][2]; 

                        currentRequest.PlacerKeyTest = obrCache[targetResult.Code][18];
                    }

                    currentRequest.TaskIdLis = resultData.RequestCode + "^qLIS";
                    currentRequest.TestCodes = "^^^" + targetResult.Code + "^" + targetResult.Name;
                    currentRequest.Biomaterial = "~" + sampleResult.BiomaterialCode;
                    index++;
                    currentRequest.Date = targetResult.Works.Max(wR => wR.ApproveDate);
                    result.Add(currentRequest.ToString());
                    targetResult.Works.RemoveAll(w => w.State != WorkStateForMis.Approved);

                    for (Int32 i = 0; i < targetResult.Works.Count; i++)
                    {
                        ru.novolabs.ExchangeDTOs.Work workResult = targetResult.Works[i];
                      
                        MisExchangeAdapters.Parser.HL7.Classes.TargetResult currentResult = new MisExchangeAdapters.Parser.HL7.Classes.TargetResult();

                        currentResult.SegmentType = "OBX";
                        currentResult.SegmentID = (i + 1).ToString();
                        currentResult.ResultType = "NM";
                        currentResult.TestCode = workResult.Code + "^" + workResult.Name;
                        currentResult.Value = workResult.Value;
                        currentResult.UnitType = workResult.UnitName;
                        currentResult.ReferenceValue = workResult.Norm.Norms;
                        currentResult.ExecutionDate = workResult.ApproveDate;

                        result.Add(currentResult.ToString());
                    }

                   
                }
            }
            return result;
        }
        private static string BuildPV1Frame(ru.novolabs.ExchangeDTOs.Result resultData, List<string> pv1Cache)
        {
            PatientVisit visit = new PatientVisit();
            visit.SegmentType = "PV1";
            visit.SeqVisitId = "";
            visit.PatientClass = "O";
            visit.Hospital = resultData.HospitalCode;
            visit.Doctor = "";
            //visit.PatientHistoryNumber = resultData.Patient.PatientCard.CardNr;
            //visit.HospitalizationDate = resultData.SampleDeliveryDate == null ? DateTime.MaxValue : resultData.SampleDeliveryDate.Value;
            //visit.DischargeDate = DateTime.Now;
            if (pv1Cache != null)
            {
                visit.VisitNumber = pv1Cache[19];
                DateTime tempDt;
                visit.HospitalizationDate = DateTime.TryParseExact(pv1Cache[44], "yyyyMMddHHmmss", null, DateTimeStyles.None, out tempDt) ? tempDt
                    : (Nullable<DateTime>)null;
                visit.AlterVisitNumber = pv1Cache.Count >=51? pv1Cache[50]: null;
            }
            return visit.ToString();
        }

        public static List<String> BuildHL7Results(ru.novolabs.ExchangeDTOs.Result resultData, string dataCache)
        {
            CacheHL7FileParser parserCache = new CacheHL7FileParser();
            parserCache.Parse(dataCache);
            List<String> result = new List<string>();
            result.Add(BuildMshFrame(resultData, parserCache.MSHdata));
            result.Add(BuildPidFrame(resultData, parserCache.PIDdata));
            result.Add(BuildPV1Frame(resultData, parserCache.PV1data));
            result.AddRange(BuildWorkResultsFrames(resultData, parserCache.OBRdata));
            return result;
        }
    }
    class CacheHL7FileParser
    {
        public List<string> MSHdata;
        public List<string> PIDdata;
        public List<string> PV1data;
        public Dictionary<string, List<string>> OBRdata;
        public void Parse(string data)
        {
            if (data == null)
                return;
            List<String> frames = data.Split(new String[] { "\r" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            OBRdata = new Dictionary<string, List<string>>();        
            foreach (String frame in frames)
            {
                List<String> currentFrameFields = frame.Split(new char[] { '|' }).ToList();
                switch (currentFrameFields[0])
                {
                    case HL7BlockConst.MSH:
                        MSHdata = currentFrameFields.ToList();
                        break;
                    case HL7BlockConst.PID:
                        PIDdata = currentFrameFields.ToList();
                        break;
                    case HL7BlockConst.PV1:
                        PV1data = currentFrameFields.ToList();
                        break;
                    case HL7BlockConst.OBR:
                        string specimenSource = currentFrameFields[4].Split(new char[] { '^' })[3];
                        OBRdata.Add(specimenSource, currentFrameFields.ToList());
                        break;
                    default:
                        break;
                }
            }
        
        }
    
    }

}
