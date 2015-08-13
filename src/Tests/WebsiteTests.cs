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

        [Fact]
        public void ShouldStartWebsite()
        {
            var settings = CakeHelper.GetWebsiteSettings();

            // Arrange
            CakeHelper.CreateWebsite(settings);
            CakeHelper.AssertWebsiteExists(settings.Name);
            CakeHelper.StopWebsite(settings.Name);

            {
                // Act
                CakeHelper.CreateWebsiteManager().Start(settings.Name);
            }

            // Assert
            CakeHelper.AssertWebsiteStarted(settings.Name);
        }

        [Fact]
        public void ShouldStopWebsite()
        {
            var settings = CakeHelper.GetWebsiteSettings();

            // Arrange
            CakeHelper.CreateWebsite(settings);
            CakeHelper.AssertWebsiteExists(settings.Name);
            CakeHelper.StartWebsite(settings.Name);

            {
                // Act
                CakeHelper.CreateWebsiteManager().Stop(settings.Name);
            }

            // Assert
            CakeHelper.AssertWebsiteStopped(settings.Name);
        }
    }
}