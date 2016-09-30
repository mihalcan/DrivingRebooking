﻿using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DrivingRebooking
{
    class Program
    {
        static void Main(string[] args)
        {
            var latestDate = DateTime.ParseExact(ConfigurationManager.AppSettings["LatestDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);

            try
            {
                Trace.TraceWarning("Job started");
                Console.WriteLine("job started");

                var closestExamDate = new BookingDateExtractor().GetClosestAvailableDate();
                if (closestExamDate < latestDate)
                {
                    ExamBooker.BookNewDate(closestExamDate);
                }
                else
                {
                    var message = string.Format("No earlier slot, as closest date is {0} which is later than {1}", closestExamDate, latestDate);
                    Trace.TraceWarning(message);
                    Console.WriteLine(message);
                }

                Console.WriteLine("job finished OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                Trace.TraceError(ex.Message + " " + ex.StackTrace);

                Notifier.SendNotification("Booking exception", ex.Message)
                    .ContinueWith(t => EmailStatus(t)).Wait();
                throw;
            }
            finally
            {
                Browser.Current.Dispose();
            }
        }

        private static void EmailStatus(Task t)
        {
            if (t.IsFaulted)
            {
                Console.WriteLine("Email Is faulted");
                Console.WriteLine(t.Exception.GetBaseException().Message);
            }
            else
            {
                Console.WriteLine("EmAIL status - " + t.Status.ToString());
            }
        }
    }
}
