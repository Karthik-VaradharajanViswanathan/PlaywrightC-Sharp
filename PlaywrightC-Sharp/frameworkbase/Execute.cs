using Microsoft.Playwright;
using playwrightcs.utilities;
using playwrightcs.utilities.helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Diagnostics;

namespace playwrightcs.frameworkbase;

public class Execute
{
    private IPage Page;
    private readonly List<IBrowserType> browserTypeList = new();
    private string _environment;

    private static Task<IPlaywright> GetCreateAsync()
    {
        var playwright = Playwright.CreateAsync();
        return playwright;
    }

    private async Task<IBrowser> BrowserConfig()
    {
        var browserType = await GetBrowserDetails();
        // Launch a new browser instance
        if (browserType != null && Globals.RunningInPipeline)
        {
            var result = await browserType.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                Args = new List<string> { "--start-maximized" }
            });
            return result;
        }
        if (browserType != null && !Globals.RunningInPipeline)
        {
            var result1 = await browserType.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                Args = new List<string> { "--start-maximized" }
            });
            return result1;
        }
        return null;
    }

    private async Task LaunchWithOptionAsync(Action<IPage> Testcase)
    {
        var browser = await BrowserConfig();
        var Context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = ViewportSize.NoViewport
        });

        await Context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
        });

        Page = await Context.NewPageAsync();
        Testcase(Page);
    }

    private async Task LaunchWithoutOptionAsync(Action<IPage> Testcase)
    {
        var browser = await BrowserConfig();

        if (!(browser == null && Globals.BROWSER == "All"))
        {
            // Create a new context
            var context = await browser.NewContextAsync();
            Page = await context.NewPageAsync();
            Testcase(Page);
        }
        else
        {
            var browserTasks = new Task[browserTypeList.Count];

            for (int i = 0; i < browserTypeList.Count; i++)
            {
                var result = await browserTypeList[i].LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = false,
                });

                browserTasks[i] = Task.Run(async () =>
                {
                    // Create a new context
                    var context = await result.NewContextAsync();
                    Page = await context.NewPageAsync();
                    Testcase(Page);
                });
            }
            await Task.WhenAll(browserTasks);
        }
    }

    private async Task<IBrowserType> GetBrowserDetails()
    {
        string _browser = Globals.BROWSER;

        var playwright = await GetCreateAsync();

        switch (_browser.ToLower())
        {
            case "chromium":
                return playwright.Chromium;

            case "firefox":
                return playwright.Firefox;

            case "webkit":
                return playwright.Webkit;

            case "all":
                browserTypeList.Add(playwright.Chromium);
                browserTypeList.Add(playwright.Webkit);
                break;

            default:
                Console.WriteLine($"Not a valid {_browser}");
                break;
        }
        return null;
    }

    public async Task RunAsync(Action<IPage> TestCase)
    {
        var playwright = Task.Run(async () => await GetCreateAsync()).Result;

        if (Globals.BROWSER != "All")
        {
            Console.WriteLine(Globals.EXECUTE_ON_DEVICES.Count);
            if (Globals.EXECUTE_ON_DEVICES.Count != 0)
            {
                foreach (var mobileDevice in Globals.EXECUTE_ON_DEVICES)
                {
                    var device = playwright.Devices[mobileDevice];

                    await LaunchWithOptionAsync(TestCase);
                }
            }
            else
            {
                Console.WriteLine($"Not a valid browser:  {Globals.BROWSER}");
            }
        }
        else
        {
            await LaunchWithoutOptionAsync(TestCase);
        }
    }

    private async Task Dispose()
    {
        //Tracing session of passed tests will end with the context's closure.
        if (Page != null)
        {
            // Close the page first
            await Page.CloseAsync();

            if (Page.Context != null)
            {
                // Close the context after the page is closed
                await Page.Context.CloseAsync();
            }
        }
    }

    private async Task EndTrace()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Passed)
        {
            // Stop tracing only if the test failed
            await Page.Context.Tracing.StopAsync(new TracingStopOptions
            {
                Path = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\Files\Traces\{TestContext.CurrentContext.Test.Name}_trace.zip"
            });
            ExtentReportsHelper.ReportLineDown();
        }

        ExtentReportsHelper.Reports.Flush();
    }

    [OneTimeSetUp]
    public void NUnitInitialize()
    {
        var exitCode = Program.Main(new[] { "install" });

        if (exitCode != 0)
        {
            Console.WriteLine("Failed to install browsers");
            Environment.Exit(exitCode);
        }
        Console.WriteLine("Browsers installed");
        TestParameters parameters = TestContext.Parameters;

        try
        {
            //Sets Parametes based on the runsettings file
            if (!Debugger.IsAttached)
            {
                string isPipeLine = parameters["runningInPipeline"].ToString();
                Globals.RunningInPipeline = Convert.ToBoolean(isPipeLine);
                Globals.TargetEnvironment = parameters["environment"].ToString();
                _environment = parameters["server"].ToString();
            }
        }
        catch (NullReferenceException) //Will fail if the runsettings file gets deselected.
        {
            TestContext.Progress.WriteLine("Run Settings file not found, setting RunningInPipeline = false");
            Globals.RunningInPipeline = false;
        }

        ReadConfig.ReadDevicesDetailsFromAppConfig();
        ReadConfig.AddBrowserToGlobal();
        ExtentReportsHelper.ReportLineUp();
    }

    [SetUp]
    public void InitializeExtentTest()
    {
        ExtentReportsHelper.Test = ExtentReportsHelper.Reports.CreateTest(TestContext.CurrentContext.Test.MethodName);
    }

    [TearDown]
    public void TearDownAction()
    {
        Task.Run(async () =>
        {
            await EndTrace();
            await Dispose();
        }).Wait();
    }
}