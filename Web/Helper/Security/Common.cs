<<<<<<< HEAD
﻿using QRCoder;
using System;
=======
﻿using System;
>>>>>>> 639d8d0 (Intial commit)
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


namespace App.Web.Helper
{
    public class Common
    {
<<<<<<< HEAD
        public static string GenerateQrCode(long IdCipl, string doc)
        {

            try
            {
                // invoice = downloadInvoice
                //PL =DownloadPl

                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                Uri url1 = new Uri(url);
                string host = url1.GetLeftPart(UriPartial.Authority);

                string docUrl = host + "/download/" + doc + "/" + IdCipl;
                string imgDataURL = string.Empty;
                QRCodeGenerator ObjQr = new QRCodeGenerator();
                QRCodeData qrCodeData = ObjQr.CreateQrCode(docUrl, QRCodeGenerator.ECCLevel.Q);
                Bitmap bitMap = new QRCode(qrCodeData).GetGraphic(20);
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    //Convert byte arry to base64string   
                    string imreBase64Data = Convert.ToBase64String(byteImage);
                    imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
                    //Passing image data in viewbag to view  

                }

                return imgDataURL;
            }
            catch (Exception ex)
            {

                throw ex;
            }




        }
=======
>>>>>>> 639d8d0 (Intial commit)
        public static string UploadFile(HttpPostedFileBase file, string appName)
        {

            if (file != null && file.ContentLength > 0)
            {
                //check size
                int RequiredMax = Convert.ToInt32(ConfigurationManager.AppSettings["UploadFileSize"]);
                Image img = new Bitmap(file.InputStream);
                int MaxSize = RequiredMax * 1024 * 1024;
                if (file.ContentLength > MaxSize)
                {
                    img = ScaleImage(img);
                }
               
                var fileName = appName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
                
                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/" + appName + "/"), fileName);

                img.Save(path);
                //file.SaveAs(path);
                return fileName;
            }
            return "";
        }
 
        private static Bitmap ScaleImage(Image oldImage)
        {
            double resizeFactor = 1;

            if (oldImage.Width > 600 || oldImage.Height > 600)
            {
                double widthFactor = Convert.ToDouble(oldImage.Width) / 600;
                double heightFactor = Convert.ToDouble(oldImage.Height) / 600;
                resizeFactor = Math.Max(widthFactor, heightFactor);

            }
            int width = Convert.ToInt32(oldImage.Width / resizeFactor);
            int height = Convert.ToInt32(oldImage.Height / resizeFactor);
            Bitmap newImage = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(newImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(oldImage, 0, 0, newImage.Width, newImage.Height);
            return newImage;
        }

        public static void DeleteFile(string file, string appName)
        {
            if (!string.IsNullOrEmpty(file))
            {
                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/" + appName + "/"), file);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        public static string GetImage(string file, string appName)
        {
            string Result = "asdfdfdsfdf";
            if (!string.IsNullOrEmpty(file))
            {
                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/" + appName + "/"), file);
        
        if (File.Exists(path))
                {
                    Result = "/Images/" + appName + "/" + file;
                }
                else
                    Result = "/Images/Tusmart/" + "icon-upload-picture.png";
            }
            return Result;
        }

        public static string Sanitize(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return Regex.Replace(text, @"[^-A-Za-z0-9+&@#/%?=~_|!:,.;\(\) ]", "");
            }
            else
            {
                return text;
            }
        }

        public static string PasswordHash(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            var hashBytes = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}