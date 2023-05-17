using System;
using ru.novolabs.MisExchangeService;
using ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces;
using System.IO;
using System.Diagnostics;
using ru.novolabs.SuperCore.LimsDictionary;
using System.Text;

namespace ru.novolabs.MisExchange.Classes.Signature
{
    static class PDFSigner
    {
        private static IPDFSignerSettings Settings { get; set; }

        private static String detachedSignCommandTemplate = "/c {0} -signf -detached -dir {1} -thumbprint {2} -cert -cadesbes -cadestsa http://testca.cryptopro.ru/tsp/tsp.srf {3} -pin {4}";

        static PDFSigner()
        {
            Settings = GAP.PDFSignerSettings;
        }

        public static void getDetachedSigns(String reportBase64Data, ref String employeeDetachedSign, ref String organizationDetachedSign, EmployeeDictionaryItem employeeInfo = null,
            OrganizationDictionaryItem organizationInfo = null, String reportName = "report.pdf")
        {
            String reportFilePath = Path.Combine(Settings.DocumentsInDirPath, reportName);
            File.WriteAllBytes(reportFilePath, Convert.FromBase64String(reportBase64Data));

            String employeeCertificateSearchCriterion = Settings.EmployeeCertificateSearchCriterion;
            String employeeCertificatePin = Settings.EmployeeCertificatePin;

            if (employeeInfo != null)
            {
                employeeCertificateSearchCriterion = employeeInfo.CertificateSearchCriterion;
                employeeCertificatePin = Encoding.GetEncoding(1251).GetString(Convert.FromBase64String(employeeInfo.CertificatePin));
            }

            employeeDetachedSign = getDetachedSign("/c " + Settings.PdfSignerFullPath + " \"" + employeeCertificateSearchCriterion + "\" \"" + employeeCertificatePin + "\"", "employee", reportName);

            if (Settings.OrganizationSignatureRequired)
            {
                String organizationCertificateSearchCriterion = Settings.OrganizationCertificateSearchCriterion;
                String organizationCertificatePin = Settings.OrganizationCertificatePin;

                if (organizationInfo != null)
                {
                    organizationCertificateSearchCriterion = organizationInfo.CertificateSearchCriterion;
                    organizationCertificatePin = Encoding.GetEncoding(1251).GetString(Convert.FromBase64String(organizationInfo.CertificatePin));
                }

                organizationDetachedSign = getDetachedSign("/c " + Settings.PdfSignerFullPath + " \"" + organizationCertificateSearchCriterion + "\" \"" + organizationCertificatePin + "\"", "organization", reportName);
            }

            if (File.Exists(reportFilePath))
                File.Delete(reportFilePath);
        }

        private static String getDetachedSign(String arguments, String signatureSource, String reportName)
        {
            String result = String.Empty;

            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = "cmd.exe",
                    WindowStyle = ProcessWindowStyle.Normal,
                    Arguments = arguments
                };

                Process signProcess = Process.Start(processInfo);
                String errors = signProcess.StandardError.ReadToEnd();
                String outputInfo = signProcess.StandardOutput.ReadToEnd();

                Log.WriteText("Формирование " + signatureSource + " подписи документа...");
                Log.WriteText(outputInfo);
                Log.WriteText(errors);

                signProcess.WaitForExit();

                Log.WriteText("Выполняется поиск файла цифровой подписи - " + Path.Combine(Settings.SignaturesOutDirPath, reportName + ".sgn"));

                if (File.Exists(Path.Combine(Settings.SignaturesOutDirPath, reportName + ".sgn")))
                {
                    String[] signatureArray = File.ReadAllLines(Path.Combine(Settings.SignaturesOutDirPath, reportName + ".sgn"));
                    result = String.Concat(signatureArray);

                    File.Delete(Path.Combine(Settings.SignaturesOutDirPath, reportName + ".sgn"));
                }
                else
                    Log.WriteText("Файл цифровой подписи - " + Path.Combine(Settings.SignaturesOutDirPath, reportName + ".sgn") + " не найден!");
            }
            catch
            {
                //
            }

            return result;
        }

        //private static String getDetachedSign(String reportFilePath, String certificateSearchCriterion, String pin)
        //{
        //    String result = String.Empty;

        //    try
        //    {
        //        Process.Start("cmd.exe", String.Format(detachedSignCommandTemplate, Settings.CryptcpFullPath, Settings.SignaturesOutDirPath, certificateSearchCriterion, reportFilePath, pin)).WaitForExit();

        //        if (File.Exists(Path.Combine(Settings.SignaturesOutDirPath, defaultReportName + ".sgn")))
        //        {
        //            String[] signatureArray = File.ReadAllLines(Path.Combine(Settings.SignaturesOutDirPath, defaultReportName + ".sgn"));
        //            result = String.Concat(signatureArray);

        //            File.Delete(Path.Combine(Settings.SignaturesOutDirPath, defaultReportName + ".sgn"));
        //        }
        //    }
        //    catch
        //    {
        //        //
        //    }

        //    return result;
        //}
    }
}