using log4net.Repository.Hierarchy;
using System;
using System.IO;

namespace App.Web.Helper
{
    public class ErrorHelper
    {
        private static log4net.ILog Log { get; set; }

        public ErrorHelper()
        {
            Log = log4net.LogManager.GetLogger(typeof(Logger));
        }

        public void Error(object msg)
        {
            if (!Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/ErrorLog/")))
            {
                Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/ErrorLog/"));
            }
            string fPath = System.Web.Hosting.HostingEnvironment.MapPath("~/ErrorLog/" + DateTime.Today.ToString("dd-MM-yyyy") + ".txt");

            if (!File.Exists(fPath))
            {
                var myFile = File.Create(fPath);
                myFile.Close();
            }

            using (StreamWriter writer = new StreamWriter(fPath, true))
            {
                writer.WriteLine("###############################################################");
                writer.WriteLine("DateTime: " + System.DateTime.Now.ToString());
                writer.WriteLine("Message: " + msg);
                writer.WriteLine("###############################################################");
                writer.Close();
            }
        }

        public void Error(object msg, Exception ex)
        {
            Log.Error(msg, ex);
        }

        public void Error(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }

        public void Info(object msg)
        {
            Log.Info(msg);
        }
    }
}