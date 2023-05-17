using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsDictionary;
using System.Collections.Generic;
using System.Linq;

namespace ru.novolabs.MisExchange.HelperDependencies.SimpleCreateRequestDependencies
{
    internal class SimpleCreateRequest3SampleFactory
    {
        public SimpleCreateRequest3SampleFactory(IDictionaryCache dictionaryCache)
        {
            DictionaryCache = dictionaryCache;
        }

        protected IDictionaryCache DictionaryCache { get; set; }

        public virtual void BuildCreateRequset3SampleInfo(Sample sample, List<CreateRequest3SampleInfo> sampleInfoList, string internalNr)
        {
            CreateRequest3SampleInfo sampleInfo = new CreateRequest3SampleInfo
            {
                InternalNr = string.IsNullOrEmpty(sample.Barcode) ? internalNr : sample.Barcode,
                Biomaterial = BuildBiomaterialDictionaryItem(sample.BiomaterialCode),
                Priority = sample.Priority
            };
            CreateRequest3SampleInfo newSampleInfo = sampleInfoList.Find(si => si.Biomaterial.Id == sampleInfo.Biomaterial.Id && si.InternalNr == sampleInfo.InternalNr);
            if (newSampleInfo == null)
            {
                sampleInfoList.Add(sampleInfo);
            }
            else
            {
                sampleInfo = newSampleInfo;
            }

            foreach (Target target in sample.Targets)
            {
                CreateRequest3TargetInfo targetInfo = BuildCreateRequest3TargetInfo(target);
                if (targetInfo.Target.IsSimple() && !sampleInfo.Targets.Exists(tar => tar.Target.Code == targetInfo.Target.Code))
                {
                    sampleInfo.Targets.Add(targetInfo);
                }
                else if (targetInfo.Target.IsGroup())
                {
                    List<CreateRequest3TargetInfo> targets = BuildCreateRequest3TargetInfoGroup(targetInfo);
                    targets.RemoveAll(ti => sampleInfo.Targets.Exists(tar => tar.Target.Code == ti.Target.Code));
                    sampleInfo.Targets.AddRange(targets);
                }
                else if (targetInfo.Target.IsProfile())
                {
                    List<CreateRequest3TargetInfo> targets = BuildCreateRequest3SampleInfoProfile(targetInfo, sampleInfo);
                    targets.RemoveAll(ti => sampleInfo.Targets.Exists(tar => tar.Target.Code == ti.Target.Code));
                    sampleInfo.Targets.AddRange(targets);
                }
            }
            sampleInfo.Priority = sample.Priority > 0 ? sample.Priority : sampleInfo.Targets.Max(t => t.Priority);

        }
        protected virtual List<CreateRequest3TargetInfo> BuildCreateRequest3TargetInfoGroup(CreateRequest3TargetInfo targetInfo)
        {
            List<CreateRequest3TargetInfo> targetInfoList = new List<CreateRequest3TargetInfo>();
            foreach (TargetDictionaryItem targetDictItem in targetInfo.Target.Targets)
            {
                CreateRequest3TargetInfo targetInfoTemp = new CreateRequest3TargetInfo
                {
                    Target = targetDictItem,
                    Priority = targetInfo.Priority,
                    Readonly = targetInfo.Readonly
                };
                foreach (TestDictionaryItem test in targetInfo.Tests)
                {
                    if (targetDictItem.Tests.Exists(t => t.Code == test.Code))
                    {
                        targetInfoTemp.Tests.Add(test);
                    }
                }
                targetInfoList.Add(targetInfoTemp);
            }
            return targetInfoList;

        }
        protected virtual List<CreateRequest3TargetInfo> BuildCreateRequest3SampleInfoProfile(CreateRequest3TargetInfo targetInfo, CreateRequest3SampleInfo currentSampeInfo)
        {
            List<CreateRequest3TargetInfo> targetInfoList = new List<CreateRequest3TargetInfo>();
            foreach (TargetDictionaryItem targetDictItem in targetInfo.Target.Samples.First(s => s.Biomaterial.Id == currentSampeInfo.Biomaterial.Id).Targets)
            {
                CreateRequest3TargetInfo targetInfoTemp = new CreateRequest3TargetInfo
                {
                    Target = targetDictItem,
                    Priority = targetInfo.Priority,
                    Readonly = targetInfo.Readonly
                };
                targetInfoTemp.Parents.Add(targetInfo.Target);
                foreach (TestDictionaryItem test in targetInfo.Tests)
                {
                    if (targetDictItem.Tests.Exists(t => t.Code == test.Code))
                    {
                        targetInfoTemp.Tests.Add(test);
                    }
                }
                targetInfoList.Add(targetInfoTemp);
            }
            return targetInfoList;
        }
        protected virtual BiomaterialDictionaryItem BuildBiomaterialDictionaryItem(string biomaterialCode)
        {
            BiomaterialDictionaryItem biomaterialDictionaryItem = DictionaryCache.GetDictionaryItem<BiomaterialDictionaryItem>(biomaterialCode);
            return biomaterialDictionaryItem;
        }

        protected virtual CreateRequest3TargetInfo BuildCreateRequest3TargetInfo(Target target)
        {
            CreateRequest3TargetInfo targetInfo = new CreateRequest3TargetInfo
            {
                Target = BuildTargetDictionaryItem(target.Code),
                Priority = target.Priority,
                Readonly = target.ReadOnly
            };
            foreach (Test test in target.Tests)
            {
                TestDictionaryItem testDictItem = BuildTestDictionaryItem(test.Code);
                if (testDictItem != null)
                {
                    targetInfo.Tests.Add(testDictItem);
                }
            }
            return targetInfo;
        }
        protected virtual TargetDictionaryItem BuildTargetDictionaryItem(string targetCode)
        {
            TargetDictionaryItem targetDictionaryItem = DictionaryCache.GetDictionaryItem<TargetDictionaryItem>(targetCode);
            return targetDictionaryItem;
        }
        protected virtual TestDictionaryItem BuildTestDictionaryItem(string testCode)
        {
            TestDictionaryItem testDictionaryItem = DictionaryCache.GetDictionaryItem<TestDictionaryItem>(testCode);
            return testDictionaryItem;
        }
    }
}