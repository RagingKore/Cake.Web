#region Using Statements

using System;
using System.IO;
using System.Linq;
using Cake.Core;
using Microsoft.Web.Administration;
using NSubstitute;
using Xunit;

#endregion

namespace Cake.IIS.Tests.Utils
{
    internal static class CakeHelper
    {
        #region Functions (4)

        public static ICakeEnvironment CreateEnvironment()
        {
            var environment = Substitute.For<ICakeEnvironment>();
            environment.WorkingDirectory = Directory.GetCurrentDirectory();

            return environment;
        }

        public static ApplicationPoolManager CreateApplicationPoolManager()
        {
            return new ApplicationPoolManager(new ServerManager(), new DebugLog());
        }

        public static FtpsiteManager CreateFtpsiteManager()
        {
            return new FtpsiteManager(new ServerManager(), new DebugLog());
        }

        public static WebsiteManager CreateWebsiteManager()
        {
            return new WebsiteManager(new ServerManager(), new DebugLog());
        }

        public static ApplicationPoolSettings GetAppPoolSettings()
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
                PingResponseTime = TimeSpan.FromSeconds(90),
                Overwrite = false
            };

            return settings;
        }

        public static WebsiteSettings GetWebsiteSettings()
        {
            var settings = new WebsiteSettings
            {
                Name = "superman.web",
                BindingProtocol = BindingProtocol.Http,
                HostName = "superman.web",
                PhysicalPath = "./",
                ApplicationPool = GetAppPoolSettings(),
                Port = 80,
                ServerAutoStart = true,
                Overwrite = false
            };

            return settings;
        }

        public static void CreateWebsite(WebsiteSettings settings)
        {
            var websiteManager = CreateWebsiteManager();
            websiteManager.Create(settings);
            AssertWebsiteExists(settings.Name);
        }

        public static void AssertWebsiteExists(string name)
        {
            var website = GetWebsite(name);
            Assert.NotNull(website);
        }

        public static void AssertWebsiteNotExists(string name)
        {
            var website = GetWebsite(name);
            Assert.Null(website);
        }

        private static Site GetWebsite(string name)
        {
            using (var serverManager = new ServerManager())
            {
                var site = serverManager.Sites.FirstOrDefault(x => x.Name == name);
                return site;
            }
        }

        public static void DeleteWebsite(string name)
        {
            using (var server = new ServerManager())
            {
                var site = server.Sites.FirstOrDefault(x => x.Name == name);
                if (site != null)
                {
                    server.Sites.Remove(site);
                    server.CommitChanges();
                }
            }
        }

        public static void StartWebsite(string name)
        {
            using (var server = new ServerManager())
            {
                Site site = server.Sites.FirstOrDefault(x => x.Name == name);
                Assert.NotNull(site);
                site.Start();
                server.CommitChanges();
            }
        }

        public static void StopWebsite(string name)
        {
            using (var server = new ServerManager())
            {
                Site site = server.Sites.FirstOrDefault(x => x.Name == name);
                Assert.NotNull(site);
                if (site.State != ObjectState.Stopped)
                {
                    site.Stop();
                    server.CommitChanges();
                }
            }
        }

        public static void AssertWebsiteStarted(string name)
        {
            var site = GetWebsite(name);
            Assert.NotNull(site);
            Assert.True(site.State == ObjectState.Started);
        }

        public static void AssertWebsiteStopped(string name)
        {
            var site = GetWebsite(name);
            Assert.NotNull(site);
            Assert.True(site.State == ObjectState.Stopped);
        }

        public static void AssertPoolStarted(string name)
        {
            ApplicationPool pool = GetPool(name);
            Assert.NotNull(pool);
            Assert.True(pool.State == ObjectState.Started);
        }

        public static void AssertPoolStopped(string name)
        {
            ApplicationPool pool = GetPool(name);
            Assert.NotNull(pool);
            Assert.True(pool.State == ObjectState.Stopped);
        }

        public static void StopPool(string name)
        {
            using (var server = new ServerManager())
            {
                ApplicationPool pool = server.ApplicationPools.FirstOrDefault(x => x.Name == name);
                Assert.NotNull(pool);
                if (pool.State != ObjectState.Stopped)
                {
                    pool.Stop();
                    server.CommitChanges();
                }
            }
        }

        public static void StartPool(string name)
        {
            using (var server = new ServerManager())
            {
                ApplicationPool pool = server.ApplicationPools.FirstOrDefault(x => x.Name == name);
                Assert.NotNull(pool);
                pool.Start();
                server.CommitChanges();
            }
        }

        public static void CreatePool(ApplicationPoolSettings settings)
        {
            ApplicationPoolManager applicationPoolManager = CakeHelper.CreateApplicationPoolManager();
            applicationPoolManager.Create(settings);
            AssertPoolExists(settings.Name);
        }

        public static void AssertPoolExists(string name)
        {
            ApplicationPool pool = GetPool(name);
            Assert.NotNull(pool);
        }

        public static void AssertPoolNotExists(string name)
        {
            ApplicationPool pool = GetPool(name);
            Assert.Null(pool);
        }

        public static ApplicationPool GetPool(string name)
        {
            using (var server = new ServerManager())
            {
                ApplicationPool pool = server.ApplicationPools.FirstOrDefault(x => x.Name == name);
                return pool;
            }
        }

        public static void DeletePool(string name)
        {
            using (var server = new ServerManager())
            {
                ApplicationPool pool = server.ApplicationPools.FirstOrDefault(x => x.Name == name);
                if (pool != null)
                {
                    server.ApplicationPools.Remove(pool);
                    server.CommitChanges();
                }
            }
        }

        #endregion
    }
}
