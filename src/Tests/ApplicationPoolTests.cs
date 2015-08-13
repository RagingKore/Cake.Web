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
            var settings = GetAppPoolSettings();
            
            // Arrange
            DeletePool(settings.Name);

            {
                // Act 
                CakeHelper.CreateApplicationPoolManager().Create(settings);
            }

            // Assert
            AssertPoolExists(settings.Name);
        }

        [Fact]
        public void ShouldDeleteAppPool()
        {
            var settings = GetAppPoolSettings();

            // Arrange
            CreatePool(settings);
            AssertPoolExists(settings.Name);
            
            {
                // Act
                CakeHelper.CreateApplicationPoolManager().Delete(settings.Name);
            }

            // Assert
            AssertPoolNotExists(settings.Name);
        }

        private void CreatePool(ApplicationPoolSettings settings)
        {
            ApplicationPoolManager applicationPoolManager = CakeHelper.CreateApplicationPoolManager();
            applicationPoolManager.Create(settings);
            AssertPoolExists(settings.Name);
        }

        private void AssertPoolExists(string name)
        {
            ApplicationPool pool = GetPool(name);
            Assert.NotNull(pool);
        }

        private void AssertPoolNotExists(string name)
        {
            ApplicationPool pool = GetPool(name);
            Assert.Null(pool);
        }

        private ApplicationPool GetPool(string name)
        {
            using (var serverManager = new ServerManager())
            {
                ApplicationPool pool = serverManager.ApplicationPools.FirstOrDefault(x => x.Name == name);
                return pool;
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