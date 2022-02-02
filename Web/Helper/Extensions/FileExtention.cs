using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace App.Web.Helper.Extensions
{
    public class FileExtention
    {
        public static Boolean isExcelFile(String fileName)
        {

            string[] fileextention = { ".jpg", ".png", ".gif", ".docx", ".doc", ".xlsx", ".xls", ".pdf" };

            string Extension = System.IO.Path.GetExtension(fileName);

            return Array.Exists(fileextention, element => element.Contains(Extension.ToLower()));
        }

        public static Boolean isImageFile(String fileName)
        {

            string[] fileextention = { ".png", ".jpg" };

            string Extension = System.IO.Path.GetExtension(fileName);

            return Array.Exists(fileextention, element => element.Contains(Extension.ToLower())); ;
        }

        public static Boolean isVideoFile(String fileName)
        {

            string[] fileextention = { ".mp4" };

            string Extension = System.IO.Path.GetExtension(fileName);

            return Array.Exists(fileextention, element => element.Contains(Extension.ToLower())); ;
        }

        public static Boolean isDRFormFile(String fileName)
        {

            string[] fileextention = { ".jpg", ".png", ".gif", ".docx", ".doc", ".xlsx", ".xls", ".pdf" };

            string Extension = System.IO.Path.GetExtension(fileName);

            return Array.Exists(fileextention, element => element.Contains(Extension.ToLower())); ;
        }

        public static Boolean isSurveyDocumentFile(String fileName)
        {

            string[] fileextention = { ".jpg", ".png", ".gif",".zip", ".docx", ".doc", ".xlsx", ".xls", ".pdf" };

            string Extension = System.IO.Path.GetExtension(fileName);

            return Array.Exists(fileextention, element => element.Contains(Extension.ToLower())); ;
        }

        public static Boolean isValidLogProcessFile(String fileName)
        {

            string[] fileextention = { ".jpg", ".png", ".gif", ".bmp", ".docx", ".doc", ".xlsx", ".xls", ".pdf" };

            string Extension = System.IO.Path.GetExtension(fileName);

            return Array.Exists(fileextention, element => element.Contains(Extension.ToLower())); ;
        }

        public static Boolean hasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>,";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
    }
}