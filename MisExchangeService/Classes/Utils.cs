using ru.novolabs.ExchangeDTOs;
using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ru.novolabs.MisExchangeService.Classes
{
    public static class Utils
    {
        public static UInt64 Base36ToUInt64(this String base36String)
        {
            String s = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            base36String = base36String.ToUpper();
            Int32 i = 0, pos = 0;
            UInt64 rv = 0;
            do
            {
                pos = s.IndexOf(base36String[i]);
                rv = (rv * 36) + (UInt32)pos;
                i++;
            }
            while (i < base36String.Length);
            return rv;
        }

        public static String UInt64ToBase36(this UInt64 uInt64)
        {
            String s = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            UInt32 i = 12;
            UInt64 r = 0;
            Char[] c = new char[i + 1];
            do
            {
                r = (uInt64 % 36);
                c[i] = s[(Int32)r];
                uInt64 = (uInt64 - r) / 36;
                i--;
            }
            while (uInt64 > 0);
            Char[] trimChars = { '\0' };
            return new String(c).Trim(trimChars);
        }
    }
    /// <summary>
    /// Consolidate SampleResults by pair (Barcode, BiomaterialCode). Delete double works in the same sampleResult by selection one in others by ApprovedDate.
    /// Because only one work must be done in case of pair (Barcode, BiomaterialCode)
    /// </summary>
    public static class SampleResultConsolidator
    {
        private class GroupKeySample
        {
            public string Barcode { get; set; }
            public string BiomaterialCode { get; set; }
        }
        private class TargetWorkPair
        {
            public TargetResult Target { get; set; }
            public Work Work { get; set; }        
        }
        /// <summary>
        /// Compare works by Approved status and ApprovedDate
        /// </summary>
        public class WorkComparer : IComparer<Work>
        {
            public int Compare(Work w1, Work w2)
            {
                if (w1.State != WorkStateForMis.Approved && w2.State == WorkStateForMis.Approved)
                    return -3;
                if (w1.State == WorkStateForMis.Approved && w2.State != WorkStateForMis.Approved)
                    return 3;
                if (w1.State != WorkStateForMis.Approved && w2.State != WorkStateForMis.Approved)
                    return 0;
                if (w1.ApproveDate > w2.ApproveDate)
                    return 1;
                if (w1.ApproveDate < w2.ApproveDate)
                    return -1;
                return w1.ApprovingDoctorCode.GetHashCode() - w2.ApprovingDoctorCode.GetHashCode();
            }
        }
        /// <summary>
        /// Consolidate List of SampleResults by Barcode and BiomaterialCode. 
        /// As a result not some SampleResults with same pair (Barcode,BiomaterialCode).
        /// </summary>
        /// <param name="resultSamples">Arbitrary List of SampleResults</param>
        /// <returns>List of SampleResults with no doubles in case of pair (Barcode, BiomaterailCode)</returns>
        public static List<SampleResult> Consolidate(List<SampleResult> resultSamples)
        {
            List<SampleResult> samples = resultSamples.CopyViaSerialization();
            List<SampleResult> result = new List<SampleResult>();

            Func<GroupKeySample, GroupKeySample, Boolean> groupComparer = (g1,g2)=> g1.Barcode == g2.Barcode && g1.BiomaterialCode== g2.BiomaterialCode;
            Func<GroupKeySample,Int32> groupGetHashCode = (g)=>g.Barcode.GetHashCode() ^ g.Barcode.GetHashCode();

            var samplesGroups = samples.GroupBy(s => new GroupKeySample { Barcode = s.Barcode, BiomaterialCode = s.BiomaterialCode },
                new ru.novolabs.SuperCore.EqualityComparer<GroupKeySample>(groupComparer, groupGetHashCode)).Select(g=>g.ToList()).ToList();
            
            samplesGroups.ForEach(sampleGroup=>result.Add(Concatinate(sampleGroup)));
            return result;
        }
        /// <summary>
        /// Concatinate List of SampleResult into result SampleResult with all union of all TargteResults, with no doubles Work distincted by last ApprovedDate as primary factor
        /// </summary>
        /// <param name="resultSamples">List of SampleResults with same pair (Barcode, BiomaterialCode)</param>
        /// <returns>SampleResult with all Targets</returns>
        public static SampleResult Concatinate(List<SampleResult> resultSamples)
        {
            List<SampleResult> samples = resultSamples.CopyViaSerialization();
            SampleResult sample = samples.First();
            List<SampleResult> tailSamples = samples.Except(new List<SampleResult> { sample }).ToList();
            var tailTargets = tailSamples.SelectMany(s => s.TargetResults);
            sample.TargetResults.AddRange(tailTargets);
            PrepairWorks(sample.TargetResults);
            return sample;
        }
        private static void PrepairWorks(List<TargetResult> targets)
        {
            var twPairs = targets.SelectMany(t => t.Works, (t, w) => new TargetWorkPair() { Target = t, Work = w });
            Func<TargetWorkPair,TargetWorkPair,Boolean> twComparer = (tw1,tw2)=>tw1.Work.Code == tw2.Work.Code;
            Func<TargetWorkPair,Int32> twGetHashCode = tw=>tw.Work.Code.GetHashCode();
            var twPairsGroups = twPairs.GroupBy(tw => tw.Work.Code);
            // if no doubles of Works
            if (twPairsGroups.All(g=>g.Count() < 2))
                return;
            foreach(var twPairsGroup in twPairsGroups.Where(tw=>tw.Count() > 1))
            {
                PrepairTargetResult(twPairsGroup);            
            }
        }
        private static void PrepairTargetResult(IGrouping<string,TargetWorkPair> twPairsGroup)
        {
            TargetWorkPair etalonTW = twPairsGroup.OrderBy(tw => tw.Work, new WorkComparer()).First();
            List<TargetWorkPair> tailTW = twPairsGroup.ToList();
            tailTW.Remove(etalonTW);
            tailTW.ForEach(tw => tw.Target.Works.RemoveAll(w => w.Code == tw.Work.Code));
        }
    }    
}