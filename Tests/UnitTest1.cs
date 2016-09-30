using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var text = "Tuesday 13 December 2016 8:57am\r\n[was Tuesday 6 December 2016 8:20am]";
            var regex = new Regex(@"(.*)\r\n\[was\s(.*)\]", RegexOptions.Singleline);
            var match = regex.Match(text);

            Assert.AreEqual(match.Groups.Count, 2);



            Assert.IsTrue(match.Success);
        }
    }
}
