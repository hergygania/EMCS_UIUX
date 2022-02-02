using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using App.Data.Domain.POST;
using App.Web.App_Start;
using Newtonsoft.Json;

namespace App.Web.Controllers.POST
{
    public partial class PostController
    {
        #region View
        public ActionResult ListParameter()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();

        }
        #endregion

        #region Get Data
        public JsonResult GetDataParameter()
        {
            try
            {
                var data = Service.POST.StParameter.GetData().OrderBy(a => a.Group).ThenBy(a => a.Sort).ToList();
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult GetParamByGroup(string groupName)
        {
            try
            {
                var data = Service.POST.StParameter.GetParamByGroup(groupName);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }

        }



        public JsonResult GetParameterById(long id)
        {
            try
            {
                var data = Service.POST.StParameter.GetParamById(id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult GetParamById(string groupName, string name)
        {
            try
            {
                var data = Service.POST.StParameter.GetParamByName(groupName, name);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult GetParamByValue(string groupName, string value)
        {
            try
            {
                var data = Service.POST.StParameter.GetParamByValue(groupName, value);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }


        }
        #endregion

        #region CRUD Master Parameter
        [HttpPost]
        public JsonResult MasterParameterCreate(StParameter items)
        {
            try
            {
                ViewBag.crudMode = CrudModeInsert;
                Service.POST.StParameter.Crud(items, ViewBag.crudMode);
                return Json(new { status = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult MasterParameterEdit(StParameter items)
        {
            try
            {
                ViewBag.crudMode = CrudModeUpdate;
                StParameter parameter = Service.POST.StParameter.GetParamById(items.Id);
                parameter.Id = items.Id;
                parameter.Name = items.Name;
                parameter.Group = items.Group;
                parameter.Description = items.Description ?? "";
                parameter.Sort = items.Sort;
                parameter.Public = items.Public;
                parameter.Value = items.Value;
                Service.POST.StParameter.Crud(parameter, ViewBag.crudMode);
                return Json(new { status = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult MasterParameterDeleteById(int Id)
        {
            try
            {
                StParameter item = Service.POST.StParameter.GetParamById(Id);
                Service.POST.StParameter.Crud(
                    item,
                    CrudModeDelete);
                return Json(new { status = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        string Baseurl = "http://10.2.14.174:8081/";
        public async Task<ActionResult> TestingIntegrasi(string token = "")
        {
            token = "baWwPWG4yPqDwWeYKKVO0iS5ZFdjBLB1pqVj6zgR3ykuaWcpaQ3Gl9HQ7hmCjANyyqaEl4hDaUl8vjv5VuxR3Hwn44aHtuiRs5T6djHS5qoBqYcWuMfUlyKqBqrjLATEFSmI-VGuHmR6PNZtmSGhCkDp9Swtw18fqaz7AaY7APZ0-xFZfYZjxVDuQyFi51-d4XsH4d_wkCpl-9rmjeeW7DKDUzpbPu0U1vSzP5rdF1reps-l-3VQjVyrKY7bQ29niu2Am8wAlYxNByKYWSeZshu3QwTMCl5JRCEfX3YPT35tPMf4MjQWgEbOgXDdYgy0e1kucb1O3oTChWP355BfZXTTlhRYkM5CNuNK49PAo1E#1611650978618%231611643779618";
            //List<Employee> EmpInfo = new List<Employee>();
            //var content = new HttpContent();
            //content.Headers 

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                var buffer = System.Text.Encoding.UTF8.GetBytes("");
                var byteContent = new ByteArrayContent(buffer);

                var tokenEproc = "CookieToken=" + token;
                //byteContent.Headers.Add("Cookie", tokenEproc);
                //client.DefaultRequestHeaders.Add("Cookie", tokenEproc);

                HttpResponseMessage Res = await client.PostAsync("api/User/validasiPost", byteContent);
                Res.Headers.Add("Cookie", tokenEproc);
                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                }
                //returning the employee list to view  
                return View();
            }
        }
    }
}