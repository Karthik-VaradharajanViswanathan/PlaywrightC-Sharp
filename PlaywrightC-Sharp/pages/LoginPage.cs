using Microsoft.Playwright;
using playwrightcs.utilities.helpers;
using static playwrightcs.models.UserRolesItem;

namespace playwrightcs.pages
{
    public class LoginPage
    {
        private readonly IPage _page;
        private readonly CartPage _cartPage;

        public LoginPage(IPage page)
        {
            _page = page;
            _cartPage = new CartPage(page);
        }

        #region Locators

        private ILocator loginButton => _page.GetByRole(AriaRole.Button, new() { Name = "Login" });
        private ILocator swagLabItem => _page.Locator("a:has-text('Sauce Labs Bike Light')");

        #endregion Locators

        public async Task<IResponse> GotoAsync()
        {
            var result = await _page.GotoAsync("https://www.saucedemo.com/");

            await _page.WaitForLoadStateAsync(LoadState.Load);

            if (result.Status == 200)
            {
                return result;
            }
            else
            {
                await _page.TakeScreenshotAsync();
                throw new Exception(result.Status.ToString());
            }
        }

        private async Task EnterUserNameAsync(string username)
        {
            await _page.EnterTextAsync("Username", username);
        }

        private async Task EnterPasswordAsync(string password)
        {
            await _page.EnterTextAsync("Password", password);
        }

        private async Task LoginAsync(string username, string password)
        {
            await EnterUserNameAsync(username);
            await EnterPasswordAsync(password);
            await loginButton.ClickAsync();
            await _page.WaitForNetworkIdleAsync();
            //await Task.Delay(TimeSpan.FromSeconds(3));
        }

        public async Task UserLoginAsync(UserRole userRole)
        {
            await GotoAsync();
            await LoginAsync(userRole.Username, userRole.Password);
        }

        public async Task<string> getHomePageTitle()
        {
            return await _page.TitleAsync();
        }

        public async Task clickSwagLabItem()
        {
            await swagLabItem.ClickAsync();
        }

        public async Task<string> GetAddToCartItemPrice()
        {
            return await _cartPage.getItemPrice();
        }
    }
}