using System;
using System.Linq;
using Microsoft.Web.Administration;
using Xunit;

namespace Cake.IIS.Tests
{
    public class ApplicationPoolTests
    {
        [Fact]
        public void ShouldCreateAppPool()
        {
            // Arrange
            ApplicationPoolSettings settings = GetAppPoolSettings();
            DeletePool(settings.Name);

            {
                // Act
                var applicationPoolManager = CakeHelper.CreateApplicationPoolManager();
                applicationPoolManager.Create(settings);
            }

            // Assert
            using (var serverManager = new ServerManager())
            {
                ApplicationPool pool = serverManager.ApplicationPools.FirstOrDefault(x => x.Name == settings.Name);
                Assert.NotNull(pool);
            }
        }

        private void DeletePool(string name)
        {
            using (var serverManager = new ServerManager())
            {
                ApplicationPool pool = serverManager.ApplicationPools.FirstOrDefault(x => x.Name == name);
                if (pool != null)
                {
                    serverManager.ApplicationPools.Remove(pool);
                }

                serverManager.CommitChanges();
            }
        }

        private ApplicationPoolSettings GetAppPoolSettings()
        {
            var settings = new ApplicationPoolSettings
            {
                Name = "superman.web",
                Autostart = true,
                MaxProcesses = 1,
                Enable32BitAppOnWin64 = false,
                IdleTimeout = TimeSpan.FromMinutes(20),
                ShutdownTimeLimit = TimeSpan.FromSeconds(90),
                StartupTimeLimit = TimeSpan.FromSeconds(90),
                PingingEnabled = true,
                PingInterval = TimeSpan.FromSeconds(30),
                PingResponseTime = TimeSpan.FromSeconds(90)
            };

            return settings;
        }
    }
}