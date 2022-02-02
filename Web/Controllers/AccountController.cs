using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using App.Web.Helper;
using App.Web.Models;
using App.Data.Caching;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using App.Service.POST;

namespace App.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        [AllowAnonymous]
        [Route("sign-in")]
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.Message = "A fresh CAPTCHA image is being displayed!";
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.isMobileDevice = this.isMobileDevice();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("sign-in")]
        public async Task<ActionResult> SignIn(UserSignInViewModel model, string returnUrl)
        {
            ViewBag.isMobileDevice = this.isMobileDevice();

            var result = await CheckCaptchaAuthentication(model.Captcha);
            if (result > 0)
            {
                ModelState.AddModelError("", "Authentication failed!.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                try
                {
                    model.Password = Encrypt(model.Password);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error Authentication");
                }

                result = await UserCheckAuthentication(model);
                switch (result)
                {
                    case 0:
                        return RedirectToLocal(returnUrl);
                    case 1:
                        ModelState.AddModelError("", "Sorry, You do not have an authorize to access.");
                        return View(model);
                    case 2:
                        ModelState.AddModelError("", "Current Account has been locked, please call admin.");
                        return View(model);
                    case 3:
                        ModelState.AddModelError("", "Please login again after " + Domain.SiteConfiguration.LoginDelayTime + " minutes.");
                        return View(model);
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }

            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("sign-inVendor")]
        public async Task<ActionResult> SignInVendor(string user, string token)
        {
            var item = await Service.Master.UserAcces.GetPassword(user, "");
            var itemUserInternal = await Service.POST.IntegrasiEproc.GetUserIdPis(user);

            if (itemUserInternal != "")
            {
                item = await Service.Master.UserAcces.GetPassword(itemUserInternal, "");
                if (item == null)
                {
                    //throw new Exception("Maaf User tidak ditemukan : Username yang di passing adalah : " + (user ?? "Kosong") + " dan token yang diterima adalah : " + (token ?? "Kosong"));
                    return RedirectToAction("SignIn", "Account");
                }
            }

            UserSignInViewModel modelLog = new UserSignInViewModel();
            modelLog.UserID = user;
            try
            {
                modelLog.Password = Encrypt(item.Password);
                if (item != null)
                {
                    var isValidEproc = await IntegrasiEproc.CheckValidationEproc(token);
                    if (isValidEproc)
                    {
                        string isAdmin = ("" + item.RoleAccess).ToLower().Contains("administrator") ? "true" : "false";
                        Authentication.WriteCookie(System.Web.HttpContext.Current, item.UserID, item.UserID, item.RoleAccess, item.RoleAccessMode, isAdmin, item.Email, item.FullName, item.UserType.ToLower());
                        return RedirectToAction("HomeVendor", "POST");
                    }
                    else
                    {
                        //throw new Exception("Token tidak Valid : Username yang di passing adalah : " + (user ?? "Kosong") + " dan token yang diterima adalah : " + (token ?? "Kosong"));
                        return RedirectToAction("SignIn", "Account");
                    }
                }
                return RedirectToAction("SignIn", "Account");
            }
            catch (Exception ex)
            {
                //throw new Exception("Username yang di passing adalah : " + (user ?? "Kosong") + " dan token yang diterima adalah : " + (token ?? "Kosong"));
                return RedirectToAction("SignIn", "Account");
            }
        }

        [AllowAnonymous]
        [Route("sign-out")]
        public ActionResult SignOut()
        {
            _cacheManager.Clear();
            Web.Helper.Authentication.DeleteCookies();
            FormsAuthentication.SignOut();
            Session.Abandon();

            //if(this.isMobileDevice())
            //	return View("login.mobile");
            //else
            //	return View("login");

            return RedirectToAction("SignIn", "Account");
        }

        [Route("change-password")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Route("change-password")]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(AccountPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var oldPassword = model.OldPassword;
            var newPassword = model.NewPassword;
            if (oldPassword == newPassword)
            {
                ModelState.AddModelError("", "New Password must be different then current Password ...");
                return View(model);
            }
            if (User.Identity.GetUserId() == newPassword)
            {
                ModelState.AddModelError("", "New Password must be different then User name ...");
                return View(model);
            }


            if (App.Domain.SiteConfiguration.EncryptPassword)
            {
                oldPassword = CalculatedMD5Hash(model.OldPassword);
                newPassword = CalculatedMD5Hash(model.NewPassword);
            }

            var result = await Service.Master.UserAcces.ChangePassword(User.Identity.GetUserId(), oldPassword, newPassword);
            if (result > 0)
            {
                //	var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                //	if(user != null)
                //	{
                //		await SignInAsync(user, isPersistent: false);
                //	}
                return RedirectToAction("ChangePasswordSuccess");
            }

            if (result < 0)
                ModelState.AddModelError("", "Invalid User-Id or Current Password ...");
            else
                ModelState.AddModelError("", "Change password error ...");
            return View(model);
        }

        [AllowAnonymous]
        public CaptchaImageResult ShowCaptchaImage()
        {
            return new CaptchaImageResult();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [Route("access-denied")]
        public ActionResult AccessDenied()
        {
            string c = "You do not have an authorize ..!";
            return View("Error", (object)c);
        }

        [AllowAnonymous]
        [Route("change-password-success")]
        public ActionResult ChangePasswordSuccess()
        {
            string c = "Change Password Success ..!";
            return View("Error", (object)c);
        }

        [AllowAnonymous]
        public JsonResult forgotPassword(string email, string captcha)
        {
            var captchaString = "" + HttpContext.Session["captchastring"].ToString();

            if ((captcha + "").ToLower() != captchaString.ToLower())
            {
                ModelState.AddModelError("", "Authentication failed!.");
                HttpContext.Session.Remove("captchastring");
                return Json(new App.Framework.Mvc.JsonObject { Status = 1, Msg = "Authentication failed!. ..!" });
            }

            var item = Service.Master.UserAcces.GetPassword(email);
            if (item == null)
            {
                ModelState.AddModelError("", "Ooops something wrong, -Id or Current Password");
                return Json(new App.Framework.Mvc.JsonObject { Status = 1, Msg = "Ooops something wrong, please call admin ..!" });
            }
            else
            {
                var emailTo = email;
                var emailCC = "";
                var strUrlAddress = Request.Url.Scheme + "://" + Request.Url.Authority.ToString() + "/";

                string emailBody = "Dear Sir,<br><br>" +
                    "For your information : <br/><br/> " +
                    "Your password : " + item.Password + " <br/> <br/>" +
                    "<br/><br/><br/>Please click here " + strUrlAddress + " login the application.<br/><br/>Has been submitted at <b>" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm") + "</b> by: pis";


                try
                {
                    Framework.Email.SendAsync("Forgot Password", emailTo, emailCC, emailBody);
                }
                catch { }

                return Json(new App.Framework.Mvc.JsonObject { Status = 0, Msg = "Update password success" });
            }

            //return View("sign-in");
        }

        [AllowAnonymous]
        [Route("index-portal/{id}")]
        public ActionResult IndexPortal(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                string urlReferrer = "", urlPortal = "";
                try
                {
                    urlPortal = ("" + System.Configuration.ConfigurationManager.AppSettings["UrlPortal"]).ToLower();
                    Uri myReferrer = Request.UrlReferrer;
                    urlReferrer = "UrlReferrer: " + myReferrer.ToString();
                }
                catch { }

                if (string.IsNullOrEmpty(urlReferrer) || !urlReferrer.ToLower().Contains(urlPortal))
                {
                    return Redirect("~/access-denied");
                    //return View("../home/index");
                }

                this.Session["redirectportal"] = id;
                return IndexRedirect(id);
            }
        }

        [AllowAnonymous]
        public ActionResult IndexRedirect(string id)
        {
            if (string.IsNullOrEmpty(Session["redirectportal"] + ""))
            {
                Session.Remove("redirectportal");
                return Redirect("~/access-denied");
            }
            Session.Remove("redirectportal");

            bool err = false;
            var item = new Data.Domain.UserAccess();
            try
            {
                item = Service.Master.UserAcces.GetUserRoles(id);
            }
            catch { err = true; }

            if (item == null || string.IsNullOrEmpty(item.UserID) || err)
            {
                return Redirect("~/access-denied");
                //return View("../home/index");
            }

            if (item.Status.HasValue && item.Status.Value == 0)
            {
                return Redirect("~/access-denied");
                //return View("../home/index");
            }

            string isAdmin = ("" + item.RoleAccess).ToLower().Contains("administrator") ? "true" : "false";

            Authentication.WriteCookie(System.Web.HttpContext.Current, item.UserID, item.UserID,
                item.RoleAccess, item.RoleAccessMode, isAdmin, item.Email, item.FullName, item.UserType.ToLower());
            return RedirectToAction("index", "home");
        }

        public static string Encrypt(string clearText)
        {
            byte[] saltBytes = new byte[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(saltBytes, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }, 1000);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            byte[] saltBytes = new byte[] { 2, 3, 5, 7, 11, 13, 17, 19 };
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(saltBytes, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }, 1000);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        #region Authentication
        private async Task<int> UserCheckAuthentication(UserSignInViewModel model)
        {
            return await UserCheckAuthentication(model, false);
        }

        private async Task<int> UserCheckAuthentication(UserSignInViewModel model, bool isPortal)
        {
            try
            {
                model.Password = Decrypt(model.Password);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Authentication");
            }

            string userName = model.UserID, hashPassword = model.Password;

            //if (App.Domain.SiteConfiguration.EncryptPassword)
            //{
            //    hashPassword = CalculatedMD5Hash(model.Password);
            //}

            string pwd = "";
            bool err = false;
            var item = new Data.Domain.UserAccess();

            try
            {
                item = await Service.Master.UserAcces.GetPassword(userName, hashPassword);

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // ComputeHash - returns byte array  
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(item.Password));

                    // Convert byte array to a string   
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    pwd = builder.ToString();
                }
            }
<<<<<<< HEAD
            catch(Exception ex)
            {
                throw ex;
                err = true; 
            }
=======
            catch { err = true; }
>>>>>>> 639d8d0 (Intial commit)

            if (item == null || string.IsNullOrEmpty(item.UserID) || err)
            {
                return 1;
            }

            if (item.Status.HasValue && item.Status.Value == 0)
            {
                return 2;
            }
            else if (item.Status.HasValue && item.Status.Value == 4)
            {
                return 3;
            }

<<<<<<< HEAD
            //if (hashPassword != pwd && isPortal == false)
            //{
            //    var wrong = await Service.Master.UserAcces.SetPasswordWrong(userName);
            //    return 4;
            //}
            //else
            //{
=======
            if (hashPassword != pwd && isPortal == false)
            {
                var wrong = await Service.Master.UserAcces.SetPasswordWrong(userName);
                return 4;
            }
            else
            {
>>>>>>> 639d8d0 (Intial commit)
                string isAdmin = ("" + item.RoleAccess).ToLower().Contains("administrator") ? "true" : "false";

                Authentication.WriteCookie(System.Web.HttpContext.Current, item.UserID, item.UserID,
                    item.RoleAccess, item.RoleAccessMode, isAdmin, item.Email, item.FullName, item.UserType.ToLower());

                //string _url = Url.IsLocalUrl(returnUrl) ? returnUrl : "~/"; //default.aspx";

                if (item.Status.HasValue && item.Status.Value != 1)
                {
                    var clr = await Service.Master.UserAcces.SetPasswordWrongClear(userName);
                }

                return 0;//RedirectToLocal(_url);
<<<<<<< HEAD
            //}
=======
            }
>>>>>>> 639d8d0 (Intial commit)
        }

        private static string CalculatedMD5Hash(string strPassword)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputbytes = System.Text.Encoding.UTF8.GetBytes(strPassword);
            byte[] hash = md5.ComputeHash(inputbytes);

            string strHash = Convert.ToBase64String(hash);
            return strHash.ToString();
        }

        private async Task<int> CheckCaptchaAuthentication(string text)
        {
            return await CheckCaptchaAuthentication(text, false);
        }

        private async Task<int> CheckCaptchaAuthentication(string text, bool isPortal)
        {
            try
            {
                var captchaString = "" + HttpContext.Session["captchastring"].ToString();
                if ((text + "").ToLower() != captchaString.ToLower())
                {
                    HttpContext.Session.Remove("captchastring");
                    return 1;
                }
                else
                {
                    HttpContext.Session.Remove("captchastring");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Authentication");
            }
        }

        #endregion

    }
}