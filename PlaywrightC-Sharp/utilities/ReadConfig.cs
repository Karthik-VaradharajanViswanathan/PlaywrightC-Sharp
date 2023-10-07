using playwrightcs.frameworkbase;
using System.Collections.Specialized;
using System.Configuration;

namespace playwrightcs.utilities
{
    public class ReadConfig
    {
        public static string browser;

        public static void ReadDevicesDetailsFromAppConfig()
        {
            browser = ConfigurationManager.AppSettings["Browser"];
            //browser = ConfigurationManager.AppSettings.Get("Browser");

            var section = ConfigurationManager.GetSection("Execution/Devices") as NameValueCollection;

            if (section != null)
            {
                if (browser == "Webkit" || browser == "Chromium" || browser == "Firefox")
                {
                    foreach (string device in section)
                    {
                        if (!Globals.EXECUTE_ON_DEVICES.Contains(section[device].ToString()))
                            Globals.EXECUTE_ON_DEVICES.Add(section[device].ToString());
                    }
                }
                else
                {
                    Console.WriteLine($"Devices cannot run in {browser}, please update the browser to WebKit");
                }
            }
        }

        public static void AddBrowserToGlobal()
        {
            Globals.BROWSER = browser;
        }
    }
}