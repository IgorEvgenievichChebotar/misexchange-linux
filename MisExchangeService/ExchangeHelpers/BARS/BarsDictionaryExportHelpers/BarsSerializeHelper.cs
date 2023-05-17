using ru.novolabs.MisExchange.ExchangeHelpers.BARS.Temp;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.BARS
{
    class BarsSerializeHelper
    {
        public BarsSerializeHelper(ExportDirectoryHelperSettings helperSettings)
        {
            string synchronizePath = helperSettings.SynchronizedPath;
            if (String.IsNullOrEmpty(synchronizePath))
                synchronizePath = AppDomain.CurrentDomain.BaseDirectory;
            if (!Path.IsPathRooted(synchronizePath))
                synchronizePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, synchronizePath);

            _fileSynchronizedName = Path.Combine(synchronizePath, _fileSynchronizedName);
            _fileVersionName = Path.Combine(synchronizePath, _fileVersionName);
            _fileDepartmentSyncName = Path.Combine(synchronizePath, _fileDepartmentSyncName);
            _fileDefaultTestsName = Path.Combine(synchronizePath, _fileDefaultTestsName);
            _fileDefaultBiomaterialsName = Path.Combine(synchronizePath, _fileDefaultBiomaterialsName);
            if (!Directory.Exists(synchronizePath))
                Directory.CreateDirectory(synchronizePath);

            //ru.novolabs.MisExchangeService.GAP.Logger.WriteText("synchronizePath: " + synchronizePath);
            //ru.novolabs.MisExchangeService.GAP.Logger.WriteText("_fileSynchronizedName: " + _fileSynchronizedName);
            //ru.novolabs.MisExchangeService.GAP.Logger.WriteText("_fileVersionName: " + _fileVersionName);
            //ru.novolabs.MisExchangeService.GAP.Logger.WriteText("_fileDepartmentSyncName: " + _fileDepartmentSyncName);
            //ru.novolabs.MisExchangeService.GAP.Logger.WriteText("_fileDefaultTestsName: " + _fileDefaultTestsName);
            //ru.novolabs.MisExchangeService.GAP.Logger.WriteText("_fileDefaultBiomaterialsName: " + _fileDefaultBiomaterialsName);
        }

        internal static String _fileVersionName = "dictionariesVersion.txt";
        internal static String _fileSynchronizedName = "synchronizedTargets.txt";
        internal static String _fileDepartmentSyncName = "synchronizedDepartments.txt";
        internal static String _fileDefaultTestsName = "synchronizedDefaultTests.txt";
        internal static String _fileDefaultBiomaterialsName = "synchronizedDefaultBiomaterials.txt";

        public List<DirectoryVersionInfo> DeserializeVersions()
        {
            Versions root = new Versions();
            List<DirectoryVersionInfo> versions = new List<DirectoryVersionInfo>();
            if (File.Exists(_fileVersionName))
            {
                try
                {
                    String xml = File.ReadAllText(_fileVersionName);
                    root = xml.Deserialize<Versions>(Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    Log.WriteError("Can't deserialize versions: {0}", ex.StackTrace);
                    throw ex;
                }
            }
            versions = root.Items;
            return versions;
        }

        public void SerializeVersions(List<DirectoryVersionInfo> versions)
        {
            try
            {
                Versions root = new Versions();
                root.Items = versions;
                XmlSerializer serializer = new XmlSerializer(typeof(Versions));
                StringWriter sww = new StringWriter();
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, root);
                    String xml = sww.ToString();
                    File.WriteAllText(_fileVersionName, xml);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("Can't serialize versions: {0}", ex.Message);
            }
        }

        public List<NomenclatureElement> DeserializeSynchronizedTargets()
        {
            List<NomenclatureElement> targetCodes = new List<NomenclatureElement>();
            if (File.Exists(_fileSynchronizedName))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<NomenclatureElement>));
                    //StreamReader reader = new StreamReader(_fileVersionName);
                    //reader.ReadToEnd();
                    String xml = File.ReadAllText(_fileSynchronizedName);
                    targetCodes = xml.Deserialize<List<NomenclatureElement>>(Encoding.UTF8);
                    //targetCodes = (List<String>)serializer.Deserialize(reader);
                    //reader.Close();
                }
                catch (Exception ex)
                {
                    Log.WriteError("Can't deserialize synchronized targets: {0}", ex.StackTrace);
                    throw ex;
                }
            }
            return targetCodes;
        }
        public List<NomenclatureElement> DeserializeSynchronizedDepartments()
        {
            List<NomenclatureElement> departmentCodes = new List<NomenclatureElement>();
            if (File.Exists(_fileDepartmentSyncName))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<NomenclatureElement>));
                    //StreamReader reader = new StreamReader(_fileVersionName);
                    //reader.ReadToEnd();
                    String xml = File.ReadAllText(_fileDepartmentSyncName);
                    departmentCodes = xml.Deserialize<List<NomenclatureElement>>(Encoding.UTF8);
                    //targetCodes = (List<String>)serializer.Deserialize(reader);
                    //reader.Close();
                }
                catch (Exception ex)
                {
                    Log.WriteError("Can't deserialize synchronized departments: {0}", ex.StackTrace);
                    throw ex;
                }
            }
            return departmentCodes;
        }
        public List<TestWrapper> DeserializeSynchronizedDefaultTests()
        {
            List<TestWrapper> testCodes = new List<TestWrapper>();
            if (File.Exists(_fileDefaultTestsName))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<TestWrapper>));
                    //StreamReader reader = new StreamReader(_fileVersionName);
                    //reader.ReadToEnd();
                    String xml = File.ReadAllText(_fileDefaultTestsName);
                    testCodes = xml.Deserialize<List<TestWrapper>>(Encoding.UTF8);
                    //targetCodes = (List<String>)serializer.Deserialize(reader);
                    //reader.Close();
                }
                catch (Exception ex)
                {
                    Log.WriteError("Can't deserialize synchronized tests: {0}", ex.StackTrace);
                    throw ex;
                }
            }
            return testCodes;
        }
        public List<BioWrapper> DeserializeSynchronizedDefaultBiomaterials()
        {
            List<BioWrapper> biomaterialCodes = new List<BioWrapper>();
            if (File.Exists(_fileDefaultBiomaterialsName))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<BioWrapper>));
                    //StreamReader reader = new StreamReader(_fileVersionName);
                    //reader.ReadToEnd();
                    String xml = File.ReadAllText(_fileDefaultBiomaterialsName);
                    biomaterialCodes = xml.Deserialize<List<BioWrapper>>(Encoding.UTF8);
                    //targetCodes = (List<String>)serializer.Deserialize(reader);
                    //reader.Close();
                }
                catch (Exception ex)
                {
                    Log.WriteError("Can't deserialize synchronized tests: {0}", ex.StackTrace);
                    throw ex;
                }
            }
            return biomaterialCodes;
        }

        public void SerializeSynchronizedTargets(List<NomenclatureElement> targetCodes)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<NomenclatureElement>));
                StringWriter sww = new StringWriter();
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, targetCodes);
                    String xml = sww.ToString();
                    File.WriteAllText(_fileSynchronizedName, xml);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("Can't serialize synchronized targets: {0}", ex.Message);
            }
        }
        public void SerializeSynchronizedDepartmets(List<NomenclatureElement> departmentCodes)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<NomenclatureElement>));
                StringWriter sww = new StringWriter();
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, departmentCodes);
                    String xml = sww.ToString();
                    File.WriteAllText(_fileDepartmentSyncName, xml);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("Can't serialize synchronized departments: {0}", ex.Message);
            }
        }
        public void SerializeSynchronizedDefaultTests(List<TestWrapper> testCodes)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<TestWrapper>));
                StringWriter sww = new StringWriter();
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, testCodes);
                    String xml = sww.ToString();
                    File.WriteAllText(_fileDefaultTestsName, xml);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("Can't serialize synchronized tests: {0}", ex.Message);
            }
        }
        public void SerializeSynchronizedDefaultBiomaterials(List<BioWrapper> biomaterialCodes)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<BioWrapper>));
                StringWriter sww = new StringWriter();
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, biomaterialCodes);
                    String xml = sww.ToString();
                    File.WriteAllText(_fileDefaultBiomaterialsName, xml);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("Can't serialize synchronized biomaterialCodes: {0}", ex.Message);
            }
        }
    }
}
