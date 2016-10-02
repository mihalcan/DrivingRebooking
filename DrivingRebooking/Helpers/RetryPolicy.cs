using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DrivingRebooking
{
    public static class RetryPolicy
    {
        public static T Linear<T>(Func<T> functionToExecute, TimeSpan interval, int maxTries = 3)
        {
            for (int i = 0; i <= maxTries; i++)
            {
                try
                {
                    return functionToExecute();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Retry error " + ex.Message + ex.StackTrace);
                    Trace.TraceError("Retry error " + ex.Message + ex.StackTrace);

                    if (i >= maxTries)
                    {
                        throw;
                    }
                }

                Thread.Sleep(interval);
            }

            throw new ArgumentException("Function was executed more times than expected.");
        }
    }
}
