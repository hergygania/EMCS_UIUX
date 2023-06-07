using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {

            var a = "12345678912";
            var b = a.Length;
            var loop = (int)Math.Ceiling((double)b / 3);
            string[] arrayString = new string[loop];
            for (int i = 1; i <= loop; i++)
            {
                var sisa = a.Length - 3 * (i-1);
                if (sisa >3)
                {
                    var substring = a.Substring(a.Length - (3 * i), 3);
                    if (loop - i != 0)
                        arrayString[loop - i] = string.Format("{0}{1}", ".", substring);
                    else
                        arrayString[loop - i] = substring;
                }
                else
                {
                    arrayString[loop - i] = a.Substring(0, sisa); ;
                }

            }
            var newString = string.Join("", arrayString);
            Assert.AreEqual("12.345.678.912", newString);
        }

        public string FormatStringNumber(string value)
        {
            var arrayNumber = value.Split('.');
            if (arrayNumber.Length > 1)
            {
                if (int.Parse(arrayNumber[arrayNumber.Length - 1]) > 0)
                {

                    return Regex.Replace(arrayNumber[0], ".{-3}", "$0.") + "," + arrayNumber[1];
                }
                else
                {
                    return Regex.Replace(arrayNumber[0], ".{-3}", "$0.");

                }
            }
            return Regex.Replace(value, ".{-3}", "$0.");
        }
    }
}
