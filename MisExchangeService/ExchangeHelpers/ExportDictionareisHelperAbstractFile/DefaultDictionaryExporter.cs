using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.ExchangeHelpers
{
    static class DefaultDictionaryExporter
    {
        public static void ExportTestDictionary(String exportFileName)
        {
            var tests = (TestDictionary)ProgramContext.Dictionaries.GetDictionary(LimsDictionaryNames.Test);

            List<ExternalTestInfo> extTests = new List<ExternalTestInfo>();
            foreach (TestDictionaryItem test in tests.Elements)
            {
                if (!test.Removed)
                {
                    extTests.Add(new ExternalTestInfo() { TestRef = new ObjectRef(test.Id) });
                }
            }

            File.WriteAllText(exportFileName, extTests.Serialize(Encoding.UTF8));
        }

        public static void ExportTargetDictionary(String exportFileName)
        {
            var targets = (TargetDictionary)ProgramContext.Dictionaries.GetDictionary(LimsDictionaryNames.Target);
            List<ExternalTargetInfo> extTargets = new List<ExternalTargetInfo>();

            foreach (TargetDictionaryItem target in targets.Elements)
            {
                if (!target.Removed)
                {
                    extTargets.Add(new ExternalTargetInfo() { TargetRef = new ObjectRef(target.Id) });
                }
            }

            File.WriteAllText(exportFileName, extTargets.Serialize(Encoding.UTF8));
        }

        public static void ExportBiomaterialDictionary(String exportFileName)
        {
            var biomaterials = (BiomaterialDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.Biomaterial];

            List<ExternalBiomaterialInfo> extBiomaterials = new List<ExternalBiomaterialInfo>();
            foreach (BiomaterialDictionaryItem biomaterial in biomaterials.Elements)
            {
                if (!biomaterial.Removed)
                {
                    extBiomaterials.Add(new ExternalBiomaterialInfo() { BiomaterialRef = new ObjectRef(biomaterial.Id) });
                }
            }

            File.WriteAllText(exportFileName, extBiomaterials.Serialize(Encoding.UTF8));
        }
        public static void ExportAsCodeMnemonic(String dictionary, String exportFileName)
        {
            DictionariesExportHelper.ExportDictionaryAsNameCodeMnemonicsOld(dictionary, exportFileName);
        }
    }
}
