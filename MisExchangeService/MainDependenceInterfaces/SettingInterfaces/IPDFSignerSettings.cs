using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces
{
    public interface IPDFSignerSettings
    {
        string CryptcpFullPath { get; }

        string PdfSignerFullPath { get; }

        bool OrganizationSignatureRequired { get; }

        string DocumentsInDirPath { get; }

        string SignaturesOutDirPath { get; }

        string EmployeeCertificateSearchCriterion { get; }

        string EmployeeCertificatePin { get; }

        string OrganizationCertificateSearchCriterion { get; }

        string OrganizationCertificatePin { get; }
    }
}