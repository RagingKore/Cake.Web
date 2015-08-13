#region Using Statements
    using System;
    using System.Linq;

    using Microsoft.Web.Administration;
#endregion



namespace Cake.IIS.Tests.Utils
{
    internal static class CakeHelper
    {
        #region Functions (4)
            //Managers
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



            //Settings
            public static ApplicationPoolSettings GetAppPoolSettings()
            {
                return new ApplicationPoolSettings
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
            }

            public static WebsiteSettings GetWebsiteSettings()
            {
                return new WebsiteSettings
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
            }

            

            //Website
            public static void CreateWebsite(WebsiteSettings settings)
            {
                WebsiteManager manager = CakeHelper.CreateWebsiteManager();

                manager.Create(settings);
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

            public static Site GetWebsite(string name)
            {
                using (var serverManager = new ServerManager())
                {
                    return serverManager.Sites.FirstOrDefault(x => x.Name == name);
                }
            }

            public static void StartWebsite(string name)
            {
                using (var server = new ServerManager())
                {
                    Site site = server.Sites.FirstOrDefault(x => x.Name == name);

                    if (site != null)
                    {
                        site.Start();
                    }
                }
            }

            public static void StopWebsite(string name)
            {
                using (var server = new ServerManager())
                {
                    Site site = server.Sites.FirstOrDefault(x => x.Name == name);

                    if (site != null)
                    {
                        site.Stop();
                    }
                }
            }



            //Pool
            public static void CreatePool(ApplicationPoolSettings settings)
            {
                ApplicationPoolManager manager = CakeHelper.CreateApplicationPoolManager();

                manager.Create(settings);
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

            public static ApplicationPool GetPool(string name)
            {
                using (var server = new ServerManager())
                {
                    return server.ApplicationPools.FirstOrDefault(x => x.Name == name);
                }
            }

            public static void StartPool(string name)
            {
                using (var server = new ServerManager())
                {
                    ApplicationPool pool = server.ApplicationPools.FirstOrDefault(x => x.Name == name);

                    if (pool != null)
                    {
                        pool.Start();
                    }
                }
            }

            public static void StopPool(string name)
            {
                using (var server = new ServerManager())
                {
                    ApplicationPool pool = server.ApplicationPools.FirstOrDefault(x => x.Name == name);

                    if (pool != null)
                    {
                        pool.Stop();
                    }
                }
            }
        #endregion
    }
}
