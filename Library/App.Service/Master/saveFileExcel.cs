using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Service.Master
{
    public class saveFileExcel
    {
        public static bool InsertHistoryUpload(HttpPostedFileBase upload, ref Data.Domain.DocumentUpload file, ref Data.Domain.LogImport logImport, string ModuleName, ref string filePathName, ref string msg)
        {
            try
            {
                var excel = new Framework.Infrastructure.Documents();
                var ret = excel.UploadExcel(upload, ref filePathName, ref msg);

                if (ret)
                {
                    file.ModulName = ModuleName;
                    file.FileName = System.IO.Path.GetFileName(filePathName);
                    file.URL = System.IO.Path.Combine("/Upload/Doc/", file.FileName);
                    file.Status = 0; //in Progress
                    Service.Master.DocumentUpload.crud(file, "I");
                    logImport.FileName = file.FileName.ToString();
                    logImport.Modul = file.ModulName;
                }

                return ret;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
    }
}
