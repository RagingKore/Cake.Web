using Cake.IIS.Tests.Utils;
using Xunit;

namespace Cake.IIS.Tests
{
    public class WebsiteTests
    {
        [Fact]
        public void ShouldCreateWebsite()
        {
            var settings = CakeHelper.GetWebsiteSettings();

            // Arrange
            CakeHelper.DeleteWebsite(settings.Name);

            {
                // Act
                CakeHelper.CreateWebsiteManager().Create(settings);
            }

            // Assert
            CakeHelper.AssertWebsiteExists(settings.Name);
        }

        [Fact]
        public void ShouldDeleteWebsite()
        {
            var settings = CakeHelper.GetWebsiteSettings();

            // Arrange
            CakeHelper.CreateWebsite(settings);
            CakeHelper.AssertWebsiteExists(settings.Name);

            {
                // Act
                CakeHelper.CreateWebsiteManager().Delete(settings.Name);
            }

            // Assert
            CakeHelper.AssertWebsiteNotExists(settings.Name);
        }
    }
}