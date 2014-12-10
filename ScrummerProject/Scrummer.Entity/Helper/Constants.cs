using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrummer.Entity.Helper
{
   public static class Constants
    {
       internal const string EncryptKey = "ENCRYPT PIN";
       public const string ValidateUserUrl = "User/ValidateUser";
       public const string ApplicationType = "application/json";
       public const string ScrummerWebApiUrl = "ScrummerWebApiUrl";
       public const string ExportFileName = "Exceptions";
       public const string ExcelExtension = ".xls";
       public const string CsvExtension = ".csv";
       public const string Overview = "Overview";
       public const int Zero = 0;
       public const int One = 1;
       public const string ExportError = "<b><div style='color:red'>Exception occurred while exporting as Excel.</div></b>";
       public const string ExceptionFormat = "Details:{0}";
       public const string UserScrumFileName = "UserScrumerDetails";
       public const string ScrumerViewUrl = "Scrum/ExportScrumStatusCSVExport";
       public const string GetUserUrl = "Login/GetUser";
       
    }
}
