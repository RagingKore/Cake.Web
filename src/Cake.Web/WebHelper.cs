using System.IO;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Microsoft.Web.Administration;

namespace Cake.Web
{
    internal sealed class WebHelper
    {
        private readonly IFileSystem fileSystem;
        private readonly ICakeLog log;

        public static WebHelper Using(ICakeContext context)
        {
            return new WebHelper(context.FileSystem, context.Log);
        }

        internal WebHelper(IFileSystem fileSystem, ICakeLog log)
        {
            this.fileSystem = fileSystem;
            this.log        = log;
        }

        public void CreateWebSite(WebSiteSettings settings)
        {
            if(!this.fileSystem.Exist(new DirectoryPath(settings.PhysicalPath)))
            {
                throw new DirectoryNotFoundException(settings.PhysicalPath); 
            }

            using (var server = new ServerManager())
            {
                // delete the application pool if there is already one with the same name
                CreateApplicationPool(server, settings.ApplicationPool);

                var site = server.Sites.FirstOrDefault(p => p.Name == settings.Name);

                // delete if found
                if(site != null)
                {
                    site.Delete();
                    this.log.Debug("Site found and deleted.");
                }

                // create site
                site = server.Sites.Add(
                    settings.Name,
                    settings.BindingProtocol.ToString(),
                    settings.BindingInformation, 
                    settings.PhysicalPath);

                // setup site basic settings
                site.ServerAutoStart = settings.ServerAutoStart;
                site.ApplicationDefaults.ApplicationPoolName = settings.ApplicationPool.Name;

                // commit changes to server
                server.CommitChanges();

                // log success
                this.log.Debug("Web Site created.");
            }
        }

        public void CreateFtpSite(FtpSiteSettings settings)
        {
            if (!this.fileSystem.Exist(new DirectoryPath(settings.PhysicalPath)))
            {
                throw new DirectoryNotFoundException(settings.PhysicalPath);
            }

            using (var server = new ServerManager())
            {
                var site = server.Sites.FirstOrDefault(p => p.Name == settings.Name);

                // delete if found
                if (site != null)
                {
                    site.Delete();
                    this.log.Debug("Site found and deleted.");
                }

                // create site
                site = server.Sites.Add(
                    settings.Name,
                    settings.BindingProtocol.ToString(),
                    settings.BindingInformation,
                    settings.PhysicalPath);

                // setup site basic settings
                site.ServerAutoStart = settings.ServerAutoStart;
                site.ApplicationDefaults.ApplicationPoolName = settings.ApplicationPool.Name;

                // setup site authentication mode
                this.log.Debug("Setting up authentication mode");
                site.SetAnonymousAuthentication(settings.EnableAnonymousAuthentication);
                site.SetBasicAuthentication(settings.EnableBasicAuthentication);

                // setup server authentication for site (only when using basic authentication)
                if(settings.EnableBasicAuthentication)
                {
                    this.log.Debug("Setting up user's authorization using basic authentication");
                    server.SetAuthorization(site.Name, settings.AuthorizationSettings);
                }

                // commit changes to server
                server.CommitChanges();

                // log success
                this.log.Debug("Ftp Site created.");
            }
        }
        
        public bool DeleteSite(string name)
        {
            var deleted = false;

            using (var server = new ServerManager())
            {
                var site = server.Sites.FirstOrDefault(p => p.Name == name);

                if (site != null)
                {
                    site.Delete();
                    server.CommitChanges();
                    deleted = true;
                    this.log.Debug("Site deleted.");
                }
                else
                {
                    this.log.Debug("Site not found.");
                }
            }

            return deleted;
        }

        public bool CheckSite(string name)
        {
            using (var server = new ServerManager())
            {
                return server.Sites
                    .SingleOrDefault(p => p.Name == name) != null;
            }
        }

        public bool StartSite(string name)
        {
            var started = false;

            using (var server = new ServerManager())
            {
                var site = server.Sites.FirstOrDefault(p => p.Name == name);

                if (site != null)
                {
                    site.Start();
                    server.CommitChanges();
                    started = true;
                    this.log.Debug("Site started.");
                }
                else
                {
                    this.log.Debug("Site not found.");
                }
            }

            return started;
        }

        public bool StopSite(string name)
        {
            var stopped = false;

            using (var server = new ServerManager())
            {
                var site = server.Sites.FirstOrDefault(p => p.Name == name);

                if (site != null)
                {
                    site.Stop();
                    server.CommitChanges();
                    stopped = true;
                    this.log.Debug("Site stopped.");
                }
                else
                {
                    this.log.Debug("Site not found.");
                }
            }

            return stopped;
        }

        private void CreateApplicationPool(ServerManager server, ApplicationPoolSettings settings)
        {
            var appPool = server.ApplicationPools.FirstOrDefault(p => p.Name == settings.Name);

            // delete if found
            if (appPool != null)
            {
                appPool.Delete();
                this.log.Debug("Application pool found and deleted.");
            }

            // create app pool
            appPool = server.ApplicationPools.Add(settings.Name);

            // setup app pool basic settings
            appPool.ManagedRuntimeVersion = settings.ManagedRuntimeVersion;
            appPool.ManagedPipelineMode = settings.ClassicManagedPipelineMode ? ManagedPipelineMode.Classic : ManagedPipelineMode.Integrated;
            appPool.Enable32BitAppOnWin64 = settings.Enable32BitAppOnWin64;
            appPool.AutoStart = settings.Autostart;

            // log success
            this.log.Debug("Application pool created.");
        }
        
        public void CreateApplicationPool(ApplicationPoolSettings settings)
        {
            using (var server = new ServerManager())
            {
                CreateApplicationPool(server, settings);

                // commit changes to server
                server.CommitChanges();
            }
        }

        public bool DeleteApplicationPool(string name)
        {
            var deleted = false;

            using (var server = new ServerManager())
            {
                var appPool = server.ApplicationPools.FirstOrDefault(p => p.Name == name);

                if (appPool != null)
                {
                    appPool.Delete();
                    server.CommitChanges();
                    deleted = true;
                    this.log.Debug("Application pool deleted.");
                }
                else
                {
                    this.log.Debug("Application pool not found.");
                }
            }

            return deleted;
        }

        public bool RecycleApplicationPool(string name)
        {
            var recycled = false;

            using (var server = new ServerManager())
            {
                var appPool = server.ApplicationPools.FirstOrDefault(p => p.Name == name);

                if (appPool != null)
                {
                    appPool.Delete();
                    server.CommitChanges();
                    recycled = true;
                    this.log.Debug("Application pool recycled.");
                }
                else
                {
                    this.log.Debug("Application pool not found.");
                }
            }

            return recycled;
        }

        public bool CheckApplicationPool(string name)
        {
            using (var server = new ServerManager())
            {
                return server.ApplicationPools
                    .SingleOrDefault(p => p.Name == name) != null;
            }
        }
    }
}