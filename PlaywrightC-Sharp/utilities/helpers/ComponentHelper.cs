using Microsoft.Playwright;
using Newtonsoft.Json;
using NUnit.Framework;

namespace playwrightcs.utilities.helpers
{
    public static class ComponentHelper
    {
        public static async Task EnterTextAsync(this IPage page, string name, string text)
        {
            await page.GetByRole(AriaRole.Textbox, new() { Name = name }).FillAsync(text);
        }

        public static async Task EnterTextUsingLocatorAsync(this IPage page, ILocator Locator, string text)
        {
            await Locator.ClearAsync();
            await Locator.FillAsync(text);
        }

        public static async Task<string> GetInputValue(this IPage page, string selector)
        {
            return await page.Locator(selector).InputValueAsync();
        }

        public static async Task WaitForNetworkIdleAsync(this IPage page)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
    }
}