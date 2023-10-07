using Microsoft.Playwright;
using NUnit.Framework;

namespace playwrightcs.utilities.helpers
{
    public static class GenericHelper
    {
        public static async Task TakeScreenshotAsync(this IPage page)
        {
            try
            {
                string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                string filePath = $@"{dir}\files\screenshots\";
                string fileName = TestContext.CurrentContext.Test.MethodName;
                string fullPathAndFileName = $"{filePath + $"{fileName}.png"}";
                var screenshotStream = await page.ScreenshotAsync(new PageScreenshotOptions { Path = fullPathAndFileName });
                Console.WriteLine($"Screenshot {fileName} has been captured and saved.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}