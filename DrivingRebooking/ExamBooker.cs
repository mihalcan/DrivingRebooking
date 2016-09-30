using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DrivingRebooking
{
    public class ExamBooker
    {
        public static void BookNewDate(DateTime appointment)
        {
            Browser.Current.FindElement(By.CssSelector("ul.button-board>li:first-child a")).Click();

            Browser.Current.FindElementById("slot-warning-continue").Click();
            Browser.Current.FindElementById("i-am-candidate").Click();

            if (ShouldContinueWithBooking(appointment))
            {
                Console.WriteLine("Sending email with a close date " + DateParser.ToUkString(appointment));
                Notifier.SendNotification(
                        "SLOT IS AVAILABLE!!!",
                        CreateEmailContent(appointment))
                    .ContinueWith(t => EmailStatus(t)).Wait();
            }                        
        }

        private static bool ShouldContinueWithBooking(DateTime appointment)
        {
            // "Tuesday 13 December 2016 8:57am\r\n[was Tuesday 6 December 2016 8:20am]"


            var updateText = Browser.Current.FindElementByCssSelector(".contents .update dd").Text;

            var regex = new Regex(@"(.*)\r\n\[was\s(.*)\]", RegexOptions.Singleline);
            var match = regex.Match(updateText);

            if (match.Success && match.Groups.Count == 3)
            {
                var newDate = DateParser.FromUkDate(match.Groups[1].Value);
                var prevDate = DateParser.FromUkDate(match.Groups[2].Value);
                if (newDate < prevDate)
                {
                    return true;
                }

                return false;
            }

            throw new Exception(string.Format("Cannot locate date within {0}", updateText));
        }

        private static void EmailStatus(Task t)
        {
            if (t.IsFaulted)
            {
                Console.WriteLine("Email Is faulted");
                Console.WriteLine(t.Exception.GetBaseException().Message);
                Trace.TraceWarning("Email Is faulted" + t.Exception.GetBaseException().Message);
            }
            else
            {
                Console.WriteLine("EmAIL status - " + t.Status.ToString());
            }
        }

        private static string CreateEmailContent(DateTime appointment)
        {
            var content = "New slot is available " + DateParser.ToUkString(appointment) + Environment.NewLine;
            content += ConfigurationManager.AppSettings["InitialLink"] + Environment.NewLine;
            content += ConfigurationManager.AppSettings["LicenceNo"] + Environment.NewLine;
            content += ConfigurationManager.AppSettings["ReferenceNo"] + Environment.NewLine;
            return content;
        }
    }
}
