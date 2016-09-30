using OpenQA.Selenium;
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
using Microsoft.Azure.WebJobs;

namespace DrivingRebooking
{
    class Program
    {
        static void Main(string[] args)
        {
            JobHost host = new JobHost();
            host.Call(typeof(RebookingEngine).GetMethod("Execute"));
        }
    }
}
