using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace App.Web.Controllers.POST
{
    public partial class PostController : Framework.Mvc.BaseController
    {
        private readonly string Key = "EMCS2019";
        public static readonly string CrudModeInsert = "I";
        public static readonly string CrudModeUpdate = "U";
        public static readonly string CrudModeDelete = "D";

        public string EncryptData(string textData)
        {
            RijndaelManaged objrij = new RijndaelManaged();
            objrij.Mode = CipherMode.CBC;
            objrij.Padding = PaddingMode.PKCS7;
            objrij.KeySize = 0x80;
            objrij.BlockSize = 0x80;
            byte[] passBytes = Encoding.UTF8.GetBytes(Key);
            byte[] encryptionkeyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            int len = passBytes.Length;
            if (len > encryptionkeyBytes.Length)
            {
                len = encryptionkeyBytes.Length;
            }
            Array.Copy(passBytes, encryptionkeyBytes, len);

            objrij.Key = encryptionkeyBytes;
            objrij.IV = encryptionkeyBytes;

            ICryptoTransform objtransform = objrij.CreateEncryptor();
            byte[] textDataByte = Encoding.UTF8.GetBytes(textData);
            return Convert.ToBase64String(objtransform.TransformFinalBlock(textDataByte, 0, textDataByte.Length));
        }

        public string DecryptData(string encryptedText)
        {
            encryptedText = encryptedText.Replace(" ", "+");
            RijndaelManaged objrij = new RijndaelManaged();
            objrij.Mode = CipherMode.CBC;
            objrij.Padding = PaddingMode.PKCS7;

            objrij.KeySize = 0x80;
            objrij.BlockSize = 0x80;
            byte[] encryptedTextByte = Convert.FromBase64String(encryptedText);
            byte[] passBytes = Encoding.UTF8.GetBytes(Key);
            byte[] encryptionkeyBytes = new byte[0x10];
            int len = passBytes.Length;
            if (len > encryptionkeyBytes.Length)
            {
                len = encryptionkeyBytes.Length;
            }
            Array.Copy(passBytes, encryptionkeyBytes, len);
            objrij.Key = encryptionkeyBytes;
            objrij.IV = encryptionkeyBytes;
            byte[] textByte = objrij.CreateDecryptor().TransformFinalBlock(encryptedTextByte, 0, encryptedTextByte.Length);
            return Encoding.UTF8.GetString(textByte);
        }

        public void ApplicationTitle()
        {

            var userID = User.Identity.GetUserId();
            var userType = User.Identity.GetUserType().ToLower();
            string RoleName = Service.DTS.DeliveryRequisition.GetRoleName(userID);
            string Sid = System.Web.HttpContext.Current.Session.SessionID;
            var authCookie = System.Web.HttpContext.Current.Request.Cookies.Get(".ASPXAUTH");
            var authcookievalue = authCookie.Value;

            var LogStatus = Service.POST.Transaction.SaveLogUser(userID, RoleName, userType, authcookievalue);

            var title = "Purchase Order Status Tracking";
            ViewBag.AppTitle = title;
            ViewBag.BaseUrl = BaseUrl();
        }
    }
}