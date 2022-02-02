using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController : Framework.Mvc.BaseController
    {
        private string Key = "EMCS2019";
        public EmcsController() {
            UpdateUserLog();
        }

        // GET: /DTS/
        public ActionResult Index()
        {
            return View();
        }

        public string EncryptData(string textData)
        {
            RijndaelManaged objrij = new RijndaelManaged();
            //set the mode for operation of the algorithm   
            objrij.Mode = CipherMode.CBC;
            //set the padding mode used in the algorithm.   
            objrij.Padding = PaddingMode.PKCS7;
            //set the size, in bits, for the secret key.   
            objrij.KeySize = 0x80;
            //set the block size in bits for the cryptographic operation.    
            objrij.BlockSize = 0x80;
            //set the symmetric key that is used for encryption & decryption.    
            byte[] passBytes = Encoding.UTF8.GetBytes(Key);
            //set the initialization vector (IV) for the symmetric algorithm    
            byte[] encryptionkeyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            int len = passBytes.Length;
            if (len > encryptionkeyBytes.Length)
            {
                len = encryptionkeyBytes.Length;
            }
            Array.Copy(passBytes, encryptionkeyBytes, len);

            objrij.Key = encryptionkeyBytes;
            objrij.IV = encryptionkeyBytes;

            //Creates symmetric AES object with the current key and initialization vector IV.    
            ICryptoTransform objtransform = objrij.CreateEncryptor();
            byte[] textDataByte = Encoding.UTF8.GetBytes(textData);
            //Final transform the test string.  
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
            return Encoding.UTF8.GetString(textByte);  //it will return readable string  
        }

        public void ApplicationTitle()
        {
            var title = "Export Monitoring & Control System";
            ViewBag.AppTitle = title;
        }

        public void UpdateUserLog()
        {
            Service.EMCS.SvcUserLog.Crud();
        }               
    }
}