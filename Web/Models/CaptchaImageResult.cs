using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Models
{
	public class CaptchaImageResult : ActionResult
	{

		public string GetCaptchaString(int length)
		{
			string strCaptchaString = "";
			Random random = new Random(System.DateTime.Now.Millisecond);
			//int intZero = '0';
			//int intNine = '9';
			//int intA = 'A';
			//int intZ = 'z';
			//int intCount = 0;
			//int intRandomNumber = 0;

			//while(intCount < length)
			//{
			//	intRandomNumber = random.Next(intZero, intZ);
			//	if(((intRandomNumber >= intZero) && (intRandomNumber <= intNine) || (intRandomNumber >= intA) && (intRandomNumber <= intZ)))
			//	{
			//		strCaptchaString = strCaptchaString + (char)intRandomNumber;
			//		intCount = intCount + 1;
			//	}
			//}
			

			string combination = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
			System.Text.StringBuilder captcha = new System.Text.StringBuilder();

			for(int i = 0; i < length; i++)
				captcha.Append(combination[random.Next(combination.Length)]);

			strCaptchaString = captcha.ToString();
			return strCaptchaString;
		}


		public override void ExecuteResult(ControllerContext context)
		{
			Bitmap bmp = new Bitmap(90, 30);
			Graphics g = Graphics.FromImage(bmp);
			g.Clear(Color.White);// .Navy);
            
			string randomString = GetCaptchaString(6);
			context.HttpContext.Session["captchastring"] = randomString;
			g.DrawString(randomString, new Font("Courier", 14), new SolidBrush(Color.Black), 2, 2);
          
            HttpResponseBase response = context.HttpContext.Response;
			response.ContentType = "image/jpeg";
			bmp.Save(response.OutputStream, ImageFormat.Jpeg);
			bmp.Dispose();
		}
	}
}