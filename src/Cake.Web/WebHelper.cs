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

        public void CreateSite(SiteSettings settings)
        {
            if(!this.fileSystem.Exist(new DirectoryPath(settings.PhysicalPath)))
            {
                throw new DirectoryNotFoundException(settings.PhysicalPath); 
            }

            using (var server = new ServerManager())
            {
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
                    settings.BindingProtocol,
                    settings.BindingInformation, 
                    settings.PhysicalPath);

                // setup site basic settings
                site.ServerAutoStart = settings.ServerAutoStart;
                site.ApplicationDefaults.ApplicationPoolName = settings.ApplicationPool;

                // commit changes to server
                server.CommitChanges();

                // log success
                this.log.Debug("Site created.");
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

        public void CreateApplicationPool(ApplicationPoolSettings settings)
        {
            using (var server = new ServerManager())
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
                appPool.ManagedPipelineMode   = settings.ClassicManagedPipelineMode ? ManagedPipelineMode.Classic : ManagedPipelineMode.Integrated;
                appPool.Enable32BitAppOnWin64 = settings.Enable32BitAppOnWin64;
                appPool.AutoStart             = settings.Autostart;

                // commit changes to server
                server.CommitChanges();

                // log success
                this.log.Debug("Application pool created.");
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