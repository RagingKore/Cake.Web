using System.Linq;
using Cake.IIS.Tests.Utils;
using Microsoft.Web.Administration;
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
    }
}