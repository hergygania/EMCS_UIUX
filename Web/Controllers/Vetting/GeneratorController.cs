using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Web.App_Start;
using App.Web.Models;

namespace App.Web.Controllers.Vetting
{
    public partial class VettingController 
    {
        // GET: Generator
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "quick")]
        public ActionResult GeneratorVetting()
        {
            var emailList = Service.Master.EmailRecipients.GetList().Where(w => w.Purpose.ToLower() == "vettingprocessgenerator").ToList();
            if (emailList.Count() > 0)
            {
                ViewBag.EmailReceiver = emailList[0].EmailAddress;
            }
            return View(); 
        }

        //[HttpPost]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "quick")]
        public ActionResult GeneratorVettingPreview()
        {
            PartsOrderView crit = new PartsOrderView();
            var param = Request["params"];
            var target = Request["target"];
            if (!string.IsNullOrEmpty(param))
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                crit = ser.Deserialize<PartsOrderView>(param);
            }

            int freightId = (crit.Freight + "").ToLower() == "air" ? 2 : 1;

            crit.JCode = crit.selJCode == null ? null : string.Join("|", crit.selJCode.ToArray());
            crit.AgreementType = crit.selAgreementType == null ? null : string.Join("|", crit.selAgreementType.ToArray());
            //crit.FreightShippId=null;

            var list = Service.Vetting.PartsOrder.ListGeneratorExcel(freightId, crit.FreightShippId, crit.VettingRoute, crit.ShipmentMode, crit.InvoiceNo, crit.DateSta, crit.DateFin, crit.JCode, crit.AgreementType, crit.StoreNumber, crit.DANumber);


            if (target == "1")
            {

                var email = Request["email"];
                var subject = Request["subject"];
                var body = Request["body"];
                return SendToEmail(list, crit.DateSta, crit.DateFin, email, subject, body);
            }
            else
            {
                return PreviewExcel(list, crit.DateSta, crit.DateFin);
            }
        }

        private ActionResult PreviewExcel(List<Data.Domain.VettingProcess.GeneratorModel> data, DateTime? start, DateTime? end)
        {
            ViewBag.Title = "Summary_VettingProcess_Periode_" + start?.ToString("ddMMyyyy") + "-" + end?.ToString("ddMMyyyy");
            var sb = new System.Text.StringBuilder();

            if (data != null)
            {
                sb.Append("<table border=1 cellspacing='3'>");
                sb.Append("<tr>" +
                    "<td colspan=\"5\">CAT SPM DATA CARGO LIST "+ start? .ToString("dd MMM yyyy") + " : "+ end?.ToString("dd MMM yyyy") + "</td>" +
                    "</tr>");
                sb.Append("<tr>" +
                   "<td>InvoiceNo</td>" +
                   "<td>Remarks</td>" +
                   "<td>Action</td>" +
                   "<td>InvoiceDate</td>" +
                   "<td>GeneratedDate</td>" +
                   "</tr>");

                foreach (var e in data)
                {
                    sb.Append("<tr>" +
                    "<td>" + e.InvoiceNo+ "</td>" +
                    "<td>" + e.Remarks + "</td>" +
                    "<td>" + e.Action + "</td>" +
                    "<td>" + e.InvoiceDate + "</td>" +
                    "<td>" + e.GeneratedDate + "</td>" +
                    "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);
        }

        private ActionResult SendToEmail(List<Data.Domain.VettingProcess.GeneratorModel> data, DateTime? start, DateTime? end, string email, string subject, string body)
        {

            var sb = new System.Text.StringBuilder();
            string userName = Domain.SiteConfiguration.UserName;

            if (data != null)
            {
                sb.Append("<table border=1 cellspacing='3'>");
                sb.Append("<tr>" +
                    "<td colspan=\"5\">CAT SPM DATA CARGO LIST " + start?.ToString("dd MMM yyyy") + " : " + end?.ToString("dd MMM yyyy") + "</td>" +
                    "</tr>");
                sb.Append("<tr>" +
                   "<td>InvoiceNo</td>" +
                   "<td>Remarks</td>" +
                   "<td>Action</td>" +
                   "<td>InvoiceDate</td>" +
                   "<td>GeneratedDate</td>" +
                   "</tr>");

                foreach (var e in data)
                {
                    sb.Append("<tr>" +
                    "<td>" + e.InvoiceNo + "</td>" +
                    "<td>" + e.Remarks + "</td>" +
                    "<td>" + e.Action + "</td>" +
                    "<td>" + e.InvoiceDate + "</td>" +
                    "<td>" + e.GeneratedDate + "</td>" +
                    "</tr>");
                }
                sb.Append("</table>");
            }



            var mail = new MailMessage();
            var smtpClient = new SmtpClient();
            var basicCredential = new NetworkCredential("process.maker@trakindo.co.id", "trakindo");
            smtpClient.Host = "mail.tmt.co.id";
            smtpClient.Port = 25;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = basicCredential;

            mail.From = new MailAddress("noreply@trakindo.co.id");



            var emailList = email.Split(',').Select(i => i).ToList();
            foreach (var e in emailList)
            {
                if (!string.IsNullOrEmpty(e) && !isValidEmail(e.Trim()))
                {
                    return Json(new { success = false, Msg = "invalid email address for: " + e });
                }


                mail.To.Add(e);
            }


            mail.Subject = subject;
            //mail.IsBodyHtml = true;
            mail.Body = body+ "\n\n\n\n\n\n Sincerely, \n SCIS Application Team \n Generated by: "+ userName + "  \n\n\n\n\n\n";
            var attachment = Attachment.CreateAttachmentFromString(sb.ToString(), "Summary_VettingProcess_Periode_" + start?.ToString("ddMMyyyy ") + "- " + end?.ToString("ddMMyyyy") + ".xls");
            mail.Attachments.Add(attachment);
            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }


            return new EmptyResult();
        }

    }
}