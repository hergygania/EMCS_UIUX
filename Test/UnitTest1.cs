using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowService.Library;

namespace Test
{
    [TestClass]
    public class EmailServiceTest
    {
        [TestMethod]
        public void ReadEmailTes()
        {
          Library.ReadEmailExchange();

        }

    }
}
