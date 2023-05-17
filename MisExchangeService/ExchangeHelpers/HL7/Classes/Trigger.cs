using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ru.novolabs.MisExchange.ExchangeHelpers.HL7.Classes
{
    public class Trigger
    {
        public Trigger(String template, Int32 id)
        {
            RegexTemplate = template;
            Id = id;
        }

        public Trigger(Int32 id, String template, Int32 length = 0, Byte[] sequence = null)
        {
            Id = id;
            RegexTemplate = template;
            Length = length;
            Sequence = sequence;
        }


        public Int32 Id;
        public String RegexTemplate = "";
        public Int32 Length = 0;
        public byte[] Sequence;
    }

    public class TriggerList : List<Trigger>
    {
        //public TriggerResult CheckString(String str)
        //{
        //    TriggerResult result = null;
        //    foreach (Trigger trigger in this)
        //    {
        //        if (!String.IsNullOrEmpty(trigger.RegexTemplate))
        //        {
        //            Regex reg = new Regex(trigger.RegexTemplate);
        //            Match match = reg.Match(trigger.RegexTemplate);
        //            if (match.Success && match.Index == 0)
        //            {
        //                result = new TriggerResult(trigger.Id, match.Value);
        //                break;
        //            }
        //        }
        //        if (trigger.Length != 0)
        //        {
        //            if (str.Length >= trigger.Length)
        //            {
        //                result = new TriggerResult(trigger.Id, str);

        //            }
        //        }
        //    }
        //    return result;
        //}

        public TriggerResult CheckString(Byte[] bytes, Encoding encoding)
        {
            TriggerResult result = null;
            foreach (Trigger trigger in this)
            {
                Boolean Length = false;
                Boolean RegExp = false;
                Boolean Sequence = false;
                if (trigger.Length != 0)
                {
                    if (trigger.Length <= bytes.Length)
                    {
                        Length = true;
                    }
                    else
                        continue;
                }
                else
                {
                    Length = true;
                }

                Match match = null;
                if (!String.IsNullOrEmpty(trigger.RegexTemplate))
                {
                    Regex reg = new Regex(trigger.RegexTemplate);
                    match = reg.Match(encoding.GetString(bytes));

                    if (match.Success && match.Index == 0)
                    {
                        RegExp = true;
                    }
                }
                else
                {
                    RegExp = true;
                }

                Int32 seqIndex = -1;
                if (trigger.Sequence != null && trigger.Sequence.Length != 0)
                {
                    for (Int32 i = 0; i < bytes.Length; i++)
                    {
                        Boolean found = true;
                        for (Int32 j = 0; j < trigger.Sequence.Length; j++)
                        {
                            if (trigger.Sequence[j] != bytes[i + j])
                            {
                                found = false;
                                break;
                            }
                        }

                        if (found)
                        {
                            seqIndex = i;
                            Sequence = true;
                            break;
                        }
                    }
                }
                else
                {
                    Sequence = true;
                }

                if (Length && RegExp && Sequence)
                {
                    Int32 Max = Math.Max(trigger.Length, Math.Max(match == null ? 0 : match.Length, trigger.Sequence == null ? 0 : seqIndex + trigger.Sequence.Length));
                    result = new TriggerResult(trigger.Id, Max);
                    break;
                }
            }
            return result;
        }
    }

    public class TriggerResult
    {
        public TriggerResult(Int32 id, Int32 index)
        {
            Id = id;
            Index = index;
        }
        public Int32 Id;
        public Int32 Index;

    }

    public class TriggerTypes
    {
        internal const int Enquiry = 1;
        internal const int DataFrame = 2;
        internal const int EndOfTransmission = 3;
        internal const int Acknowledge = 4;
        internal const int NotAcknowledge = 5;
    }
}
