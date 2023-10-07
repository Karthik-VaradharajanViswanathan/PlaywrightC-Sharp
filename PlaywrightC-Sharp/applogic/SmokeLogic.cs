using AventStack.ExtentReports.Reporter.Configuration;
using FluentAssertions;
using Microsoft.Playwright;
using playwrightcs.models;
using playwrightcs.pages;
using playwrightcs.utilities.helpers;
using static playwrightcs.models.UserRolesItem;

namespace playwrightcs.applogic
{
    public class SmokeLogic
    {
        public static void VerifySwagLabsCopyRights(IPage page)
        {
            Task.Run(async () =>
            {
                //Arrange
                LoginPage loginPage = new(page);
                var userRoleData = await JsonObjectHelper<UserRolesData>.GetAsync();
                UserRole standard = userRoleData.Result.First().Standard;

                // Actions
                await loginPage.UserLoginAsync(standard);
                var title = await loginPage.getHomePageTitle();
                await loginPage.clickSwagLabItem();

                //Assert
                Console.WriteLine("The Page Title is : " + title);
                title.Should().Contain("Swag Labs");
            }).Wait();
        }

        public static void VerifyPriceOfAddToCartItems(IPage page)
        {
            Task.Run(async () =>
            {
                //Arrange
                LoginPage loginPage = new(page);
                CartPage cartPage = new(page);
                var userRoleData = await JsonObjectHelper<UserRolesData>.GetAsync();
                UserRole standard = userRoleData.Result.First().Standard;

                // Actions
                await loginPage.UserLoginAsync(standard);
                await loginPage.clickSwagLabItem();
                await cartPage.clickAddToCart();
                var price = await loginPage.GetAddToCartItemPrice();

                //Assert
                Console.WriteLine("The Swag Item price is : " + price);
                price.Should().Contain("$9.99");
            }).Wait();
        }
    }
}