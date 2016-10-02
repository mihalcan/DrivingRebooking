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
    public class RebookingEngine
    {
        public static void Execute()
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

                var body = Browser.Current.FindElementByTagName("body").Text;

                Notifier.SendNotification("Booking exception", ex.Message + "body - " + body)
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
