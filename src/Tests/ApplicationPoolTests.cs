using System.Linq;
using Cake.IIS.Tests.Utils;
using Microsoft.Web.Administration;
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
            var settings = CakeHelper.GetAppPoolSettings();

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

        [Fact]
        public void ShouldStartAppPool()
        {
            var settings = CakeHelper.GetAppPoolSettings();

            // Arrange
            CreatePool(settings);
            AssertPoolExists(settings.Name);
            StopPool(settings.Name);

            {
                // Act
                CakeHelper.CreateApplicationPoolManager().Start(settings.Name);
            }

            // Assert
            AssertPoolStarted(settings.Name);
        }

        [Fact]
        public void ShouldStopAppPool()
        {
            var settings = CakeHelper.GetAppPoolSettings();

            // Arrange
            CreatePool(settings);
            AssertPoolExists(settings.Name);
            StartPool(settings.Name);

            {
                // Act
                CakeHelper.CreateApplicationPoolManager().Stop(settings.Name);
            }

            // Assert
            AssertPoolStopped(settings.Name);
        }

        private void AssertPoolStarted(string name)
        {
            ApplicationPool pool = GetPool(name);
            Assert.NotNull(pool);
            Assert.True(pool.State == ObjectState.Started);
        }

        private void AssertPoolStopped(string name)
        {
            ApplicationPool pool = GetPool(name);
            Assert.NotNull(pool);
            Assert.True(pool.State == ObjectState.Stopped);
        }

        private void StopPool(string name)
        {
            using (var serverManager = new ServerManager())
            {
                ApplicationPool pool = serverManager.ApplicationPools.FirstOrDefault(x => x.Name == name);
                Assert.NotNull(pool);
                if (pool.State != ObjectState.Stopped)
                {
                    pool.Stop();
                }
                serverManager.CommitChanges();
            }
        }

        private void StartPool(string name)
        {
            using (var serverManager = new ServerManager())
            {
                ApplicationPool pool = serverManager.ApplicationPools.FirstOrDefault(x => x.Name == name);
                Assert.NotNull(pool);
                pool.Start();
                serverManager.CommitChanges();
            }
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
    }
}