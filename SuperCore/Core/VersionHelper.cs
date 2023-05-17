
namespace ru.novolabs.SuperCore
{
    public static class VersionHelper
    {
        public static string GetVersionString()
        {
             return GetMajorVersion() + "." + GetMinorVersion() + "." + GetBuildNumber() + "." + GetRevisionNumber();
        }

        private static string GetMajorVersion()
        {
            return GetNumberByPosition(0);
        }

        private static string GetMinorVersion()
        {
            return GetNumberByPosition(1);
        }

        private static string GetBuildNumber()
        {
            return GetNumberByPosition(2);
        }

        private static string GetRevisionNumber()
        {
            return GetNumberByPosition(3);
        }

        private static string GetNumberByPosition(byte position)
        {
            string[] versionParts = XMLConst.XML_Request_BuildNumberId.Split(new char[] { '.' });
            if (versionParts.Length == 4)
                return versionParts[position];
            else
                return "?";
        }
    }}