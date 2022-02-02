using System;
using System.Web.Mvc;
using App.Data.Domain.POST;

namespace App.Web.Controllers.POST
{
    public partial class PostController
    {
        public string BaseUrl()
        {
            if (Request.Url != null)
            {
                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                return baseUrl;
            }

            return null;
        }

        public JsonResult ReadData(long RequestId)
        {
            try
            {
                var data = Service.POST.Comment.GetByReqId(RequestId);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetTotalCommentById(long RequestId)
        {
            try
            {
                var data = Service.POST.Comment.GetTotalCommentByRequest(RequestId);
                return Json(new { status = "SUCCESS", result = data.TotalComment }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetTotalUnread(long[] RequestId)
        {
            try
            {
                var data = Service.POST.Comment.GetTotalUnread(RequestId);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetTotalComment(long[] RequestId)
        {
            try
            {
                var data = Service.POST.Comment.GetTotalCommentList(RequestId);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CreateComment(string Comment = "", long RequestId = 0)
        {
            try
            {
                var comment = new TrRequestComment();
                comment.Comment = Comment;
                comment.RequestId = RequestId;
                Service.POST.Comment.CreateComment(comment);
                return Json(new { status = "SUCCESS", result = "Submit Comment Successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}