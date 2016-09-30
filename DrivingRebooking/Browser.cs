using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrivingRebooking
{
    public class Browser
    {
        private static RemoteWebDriver current;
        public static RemoteWebDriver Current
        {
            get
            {
                if (current != null)
                {
                    return current;
                }

                var options = new PhantomJSOptions();
                options.AddAdditionalCapability("phantomjs.page.customHeaders.Accept-Language", "en,en;q=0.5");
                current = new PhantomJSDriver(options);

                return current;
            }
        }
    }
}
