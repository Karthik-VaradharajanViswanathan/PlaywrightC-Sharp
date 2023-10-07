using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace playwrightcs.utilities.helpers
{
    public static class ExtentReportsHelper
    {
        public static ExtentTest Test { get; set; }
        public static ExtentReports Reports { get; private set; }

        public static MediaEntityModelProvider CreateMediaEntityAync()
        {
            string filePath = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\files\screenshots\";
            string screenshotPath = $"{filePath + $"{TestContext.CurrentContext.Test.MethodName}.png"}";
            return MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build();
        }

        public static void ReportLineUp()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string reportPath = projectDirectory + "//index.html";

            var htmlReporter = new ExtentHtmlReporter(reportPath);

            Reports = new ExtentReports();
            Reports.AttachReporter(htmlReporter);
            Reports.AddSystemInfo("Host Name", "Localhost");
            Reports.AddSystemInfo("Environment", "QA");
            Reports.AddSystemInfo("Username", "Playwright C# Automation");
        }

        public static void ReportLineDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;

            if (status == TestStatus.Failed)
            {
                Test.Log(Status.Fail, "test failed with logtrace" + stackTrace);
                Test.Fail("Test failed", CreateMediaEntityAync());
            }
            else if (status == TestStatus.Passed)
            {
                Test.Log(Status.Pass, "Test Pass");
            }
        }
    }
}