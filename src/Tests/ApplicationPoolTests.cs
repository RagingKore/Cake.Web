using Cake.IIS.Tests.Utils;
using Xunit;

namespace Cake.IIS.Tests
{
    public class ApplicationPoolTests
    {
        [Fact]
        public void ShouldCreateAppPool()
        {
            var settings = CakeHelper.GetAppPoolSettings();
            
            // Arrange
            CakeHelper.DeletePool(settings.Name);

            {
                // Act 
                CakeHelper.CreateApplicationPoolManager().Create(settings);
            }

            // Assert
            CakeHelper.AssertPoolExists(settings.Name);
        }

        [Fact]
        public void ShouldDeleteAppPool()
        {
            var settings = CakeHelper.GetAppPoolSettings();

            // Arrange
            CakeHelper.CreatePool(settings);
            CakeHelper.AssertPoolExists(settings.Name);
            
            {
                // Act
                CakeHelper.CreateApplicationPoolManager().Delete(settings.Name);
            }

            // Assert
            CakeHelper.AssertPoolNotExists(settings.Name);
        }

        [Fact]
        public void ShouldStartAppPool()
        {
            var settings = CakeHelper.GetAppPoolSettings();

            // Arrange
            CakeHelper.CreatePool(settings);
            CakeHelper.AssertPoolExists(settings.Name);
            CakeHelper.StopPool(settings.Name);

            {
                // Act
                CakeHelper.CreateApplicationPoolManager().Start(settings.Name);
            }

            // Assert
            CakeHelper.AssertPoolStarted(settings.Name);
        }

        [Fact]
        public void ShouldStopAppPool()
        {
            var settings = CakeHelper.GetAppPoolSettings();

            // Arrange
            CakeHelper.CreatePool(settings);
            CakeHelper.AssertPoolExists(settings.Name);
            CakeHelper.StartPool(settings.Name);

            {
                // Act
                CakeHelper.CreateApplicationPoolManager().Stop(settings.Name);
            }

            // Assert
            CakeHelper.AssertPoolStopped(settings.Name);
        }

       
    }
}