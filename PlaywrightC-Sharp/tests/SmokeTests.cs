using playwrightcs.applogic;
using playwrightcs.frameworkbase;
using NUnit.Framework;

namespace playwrightcs.Tests
{
    [TestFixture]
    public class SmokeTests : Execute
    {
        [Test, Category("Smoke"), Description("Swag Labs Title Verification")]
        public async Task TC01VerifyTitleOfSwagLabs()
        {
            await RunAsync(SmokeLogic.VerifySwagLabsCopyRights);
        }

        [Test, Category("Smoke"), Description("Check the price of items in cart")]
        public async Task TC02VerifySwagLabsItems()
        {
            await RunAsync(SmokeLogic.VerifyPriceOfAddToCartItems);
        }
    }
}