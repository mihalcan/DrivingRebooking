using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrivingRebooking
{
    public class BookingDateExtractor
    {
        private readonly string licenseNo = ConfigurationManager.AppSettings["LicenceNo"];
        private readonly string referenceNo = ConfigurationManager.AppSettings["ReferenceNo"];

        public DateTime GetClosestAvailableDate()
        {
            Console.WriteLine("jopening browser");
            string earliestDateString = RetryPolicy.Linear(() => GetEarliestDate(), TimeSpan.FromSeconds(2), 5);

            Trace.TraceWarning("Closest date " + earliestDateString);
            Console.WriteLine("Closest date " + earliestDateString);

            return DateParser.FromUkDate(earliestDateString);
        }

        private string GetEarliestDate()
        {
            Browser.Current.Navigate().GoToUrl(ConfigurationManager.AppSettings["InitialLink"]);
            Trace.TraceWarning("Browser.Currentpracticaltest is loaded");

            Browser.Current.FindElementById("driving-licence-number").SendKeys(licenseNo);
            Browser.Current.FindElementById("application-reference-number").SendKeys(referenceNo);
            Browser.Current.FindElementById("booking-login").Click();
            Trace.TraceWarning("driving licence details have been entered");

            Browser.Current.FindElementById("test-centre-change").Click();
            Trace.TraceWarning("selected to change test centre");
            Browser.Current.FindElementById("test-centres-input").Clear();
            Browser.Current.FindElementById("test-centres-input").SendKeys("Reading");
            Browser.Current.FindElementById("test-centres-submit").Click();
            Trace.TraceWarning("looking for reading slots");

            Browser.Current.FindElementByClassName("test-centre-details-link").Click();
            Browser.Current.FindElementById("load-earlier-availability").Click();
            Trace.TraceWarning("looking for reading slots");

            var earliestDateString = Browser.Current.FindElementById("availability-results").FindElement(By.CssSelector("span.slotDateTime")).Text;
            return earliestDateString;
        }
    }
}
