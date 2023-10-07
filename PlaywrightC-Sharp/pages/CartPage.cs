using Microsoft.Playwright;
using playwrightcs.utilities.helpers;
using static playwrightcs.models.UserRolesItem;

namespace playwrightcs.pages
{
    public class CartPage
    {
        private readonly IPage _page;

        public CartPage(IPage page)
        {
            _page = page;
        }

        #region Locators

        private ILocator buttonLocator => _page.GetByRole(AriaRole.Button, new() { Name = "Add to cart" });
        private ILocator itemPrice => _page.Locator("//div[@class='inventory_details_desc_container']/div[@class='inventory_details_price']");

        #endregion Locators

        public async Task clickAddToCart()
        {
            await buttonLocator.ClickAsync();
        }

        public async Task<string> getItemPrice()
        {
            return await itemPrice.TextContentAsync();
        }
    }
}