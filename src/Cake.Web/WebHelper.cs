using System;
using System.Linq;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.Diagnostics;
using Microsoft.Web.Administration;

namespace Cake.Web
{
    public class ApplicationPoolHelper
    {
        private static readonly string[] ApplicationPoolBlackList =
        {
            "DefaultAppPool",
            "Classic .NET AppPool",
            "ASP.NET v4.0 Classic",
            "ASP.NET v4.0"
        };

        private readonly ServerManager server;
        private readonly ICakeLog log;

        public ApplicationPoolHelper(ServerManager server, ICakeLog log)
        {
            this.server = server;
            this.log    = log;
        }

        public static ApplicationPoolHelper Using(ServerManager server, ICakeLog log)
        {
            return new ApplicationPoolHelper(server, log);
        }

        public void Create(ApplicationPoolSettings settings, bool overwrite = false)
        {
            if(settings == null) 
                throw new ArgumentNullException("settings");

            if(string.IsNullOrWhiteSpace(settings.Name)) 
                throw new ArgumentException("Application pool name cannot be null!");

            if(this.IsSystemDefault(settings.Name)) return;

            if(overwrite) this.Delete(settings.Name);
            
            var pool = this.server.ApplicationPools.Add(settings.Name);

            pool.AutoStart             = settings.Autostart;
            pool.Enable32BitAppOnWin64 = settings.Enable32BitAppOnWin64;
            pool.ManagedRuntimeVersion = settings.ManagedRuntimeVersion;
            pool.ManagedPipelineMode   = settings.ClassicManagedPipelineMode
                ? ManagedPipelineMode.Classic
                : ManagedPipelineMode.Integrated;

            this.log.Debug("Application pool created.");
        }

        public void Delete(string name)
        {
            if(!this.IsSystemDefault(name))
            {
                var pool = this.server.ApplicationPools
                    .FirstOrDefault(p => p.Name == name);

                if(pool == null)
                    this.log.Debug("Application pool '{0}' not found.", name);
                else
                {
                    this.server.ApplicationPools.Remove(pool);
                    this.log.Debug("Application pool '{0}' deleted.", pool.Name);
                }
            }
        }

        public void Recycle(string name)
        {
            var pool = this.server.ApplicationPools
                .FirstOrDefault(p => p.Name == name);

            if(pool == null)
                this.log.Debug("Application pool '{0}' not found.", name);
            else
            {
                pool.Recycle();
                this.log.Debug("Application pool '{0}' recycled.", pool.Name);
            }
        }

        public bool Check(string name)
        {
            return this.server.ApplicationPools
                .SingleOrDefault(p => p.Name == name) != null;
        }

        public bool IsSystemDefault(string name)
        {
            if(!ApplicationPoolBlackList.Contains(name)) return true;
            this.log.Debug("Application pool '{0}' is system's default.", name);
            return false;
        }
    }

    public abstract class SiteHelperBase
    {
        public readonly ServerManager Server;
        public readonly ICakeLog Log;

        protected SiteHelperBase(ServerManager server, ICakeLog log)
        {
            this.Server     = server;
            this.Log        = log;
        }

        public void Delete(string name)
        {
            var site = this.Server.Sites
                .FirstOrDefault(p => p.Name == name);

            if(site == null)
                this.Log.Debug("Site '{0}' not found.", name);
            else
            {
                this.Server.Sites.Remove(site);
                this.Log.Debug("Site '{0}' deleted.", site.Name);
            }
        }

        public void Start(string name)
        {
            var site = Server.Sites
                .FirstOrDefault(p => p.Name == name);

            if(site != null)
            {
                ObjectState state;
                do
                {
                    this.Log.Verbose("Site '{0}' starting...", site.Name);
                    state = site.Start();   
                }
                while(state != ObjectState.Started);

                this.Log.Debug("Site '{0}' started.", site.Name);
            }
        }

        public void Stop(string name)
        {
            var site = Server.Sites
                .FirstOrDefault(p => p.Name == name);

            if(site != null)
            {
                ObjectState state;
                do
                {
                    this.Log.Verbose("Site '{0}' stopping...", site.Name);
                    state = site.Start();
                }
                while(state != ObjectState.Stopped);

                this.Log.Debug("Site '{0}' stopped.", site.Name);
            }
        }

        public bool Check(string name)
        {
            return this.Server.Sites
                .SingleOrDefault(p => p.Name == name) != null;
        }
    }

    public class WebSiteHelper : SiteHelperBase
    {
        public WebSiteHelper(ServerManager server, ICakeLog log)
            : base(server, log)
        {
        }

        public static WebSiteHelper Using(ServerManager server, ICakeLog log)
        {
            return new WebSiteHelper(server, log);
        }

        public void Create(WebSiteSettings settings, bool overwrite = false)
        {
            if(settings == null)
                throw new ArgumentNullException("settings");

            if(string.IsNullOrWhiteSpace(settings.Name))
                throw new ArgumentException("Site name cannot be null!");

            if(string.IsNullOrWhiteSpace(settings.HostName))
                throw new ArgumentException("Host name cannot be null!");

            var site = Server.Sites.FirstOrDefault(p => p.Name == settings.Name);

            if(site != null)
            {
                Log.Debug("Site '{0}' already exists.", settings.Name);

                if(overwrite)
                {
                    Log.Debug("Site '{0}' will be overriden by request.", settings.Name);

                    Delete(settings.Name);

                    ApplicationPoolHelper
                        .Using(Server, Log)
                        .Create(settings.ApplicationPool, true);
                }
            }

            site = Server.Sites.Add(
                settings.Name,
                settings.BindingProtocol.ToString().ToLower(),
                settings.BindingInformation,
                settings.PhysicalPath);

            site.ServerAutoStart = settings.ServerAutoStart;
            site.ApplicationDefaults.ApplicationPoolName = settings.ApplicationPool.Name;

            if(settings.Authentication != null)
            {
                // Anonymous Authentication
                var anonymousAuthentication = site
                    .GetChildElement("webServer")
                    .GetChildElement("security")
                    .GetChildElement("authentication")
                    .GetChildElement("anonymousAuthentication");

                anonymousAuthentication.SetAttributeValue("enabled", settings.Authentication.EnableAnonymousAuthentication);
                anonymousAuthentication.SetAttributeValue("userName", settings.Authentication.Username);
                anonymousAuthentication.SetAttributeValue("password", settings.Authentication.Password);

                Log.Debug("Anonymous Authentication enabled: {0}", settings.Authentication.EnableAnonymousAuthentication);


                // Basic Authentication
                var basicAuthentication = site
                    .GetChildElement("webServer")
                    .GetChildElement("security")
                    .GetChildElement("authentication")
                    .GetChildElement("basicAuthentication");

                basicAuthentication.SetAttributeValue("enabled", settings.Authentication.EnableBasicAuthentication);

                Log.Debug("Basic Authentication enabled: {0}", settings.Authentication.EnableBasicAuthentication);

                // Windows Authentication
                var windowsAuthentication = site
                    .GetChildElement("webServer")
                    .GetChildElement("security")
                    .GetChildElement("authentication")
                    .GetChildElement("windowsAuthentication");

                windowsAuthentication.SetAttributeValue("enabled", true);

                Log.Debug("Windows Authentication enabled: {0}", settings.Authentication.EnableWindowsAuthentication);
            }

            Log.Debug("Web Site '{0}' created.", settings.Name);
        }
    }

    public class FtpSiteHelper : SiteHelperBase
    {
        public FtpSiteHelper(ServerManager server, ICakeLog log)
            : base(server, log)
        {
        }

        public static FtpSiteHelper Using(ServerManager server, ICakeLog log)
        {
            return new FtpSiteHelper(server, log);
        }

        public void Create(FtpSiteSettings settings, bool overwrite = false)
        {
            if(settings == null)
                throw new ArgumentNullException("settings");

            if(string.IsNullOrWhiteSpace(settings.Name))
                throw new ArgumentException("Site name cannot be null!");

            if(string.IsNullOrWhiteSpace(settings.HostName))
                throw new ArgumentException("Host name cannot be null!");

            var site = Server.Sites.FirstOrDefault(p => p.Name == settings.Name);

            if(site != null)
            {
                Log.Debug("Site '{0}' already exists.", settings.Name);

                if(overwrite)
                {
                    Log.Debug("Site '{0}' will be overriden by request.", settings.Name);

                    Delete(settings.Name);

                    ApplicationPoolHelper
                        .Using(Server, Log)
                        .Create(settings.ApplicationPool, true);
                }
            }

            site = Server.Sites.Add(
                settings.Name,
                settings.BindingProtocol.ToString().ToLower(),
                settings.BindingInformation,
                settings.PhysicalPath);

            site.ServerAutoStart                         = settings.ServerAutoStart;
            site.ApplicationDefaults.ApplicationPoolName = settings.ApplicationPool.Name;

            Log.Verbose("Setting up authentication mode...");
            site.SetAnonymousAuthentication(settings.EnableAnonymousAuthentication);
            site.SetBasicAuthentication(settings.EnableBasicAuthentication);

            if(settings.EnableBasicAuthentication)
            {
                Log.Verbose("Setting up user's authorization using basic authentication...");
                Server.SetAuthorization(site.Name, settings.AuthorizationSettings);
            }

            Log.Debug("Ftp Site '{0}' created.", settings.Name);
        }
    }

    public sealed class WebHelper
    {
        private readonly ICakeContext cake;
 
        public WebHelper(ICakeContext cake)
        {
            this.cake = cake;
        }

        public static WebHelper Using(ICakeContext cake)
        {
            return new WebHelper(cake);
        }

        public void DeployWebSite(WebSiteSettings settings, string sourcePath = null)
        {
            if(settings == null) throw new ArgumentNullException("settings");

            // Prepare deployment folder
            if(!this.cake.DirectoryExists(settings.PhysicalPath))
            {
                this.cake.CreateDirectory(settings.PhysicalPath);
                this.cake.Information("Deployment folder created.");
            }

            if(!string.IsNullOrWhiteSpace(sourcePath))
            {
                this.cake.CleanDirectory(settings.PhysicalPath);
                this.cake.Information("Deployment folder cleaned.");

                this.cake.Debug("Copying files to deployment folder...");
                this.cake.CopyFiles(sourcePath + "*.*", settings.PhysicalPath);
                this.cake.Information("Files copied to deployment folder.");
            }

            using(var server = new ServerManager())
            {
                WebSiteHelper
                    .Using(server, this.cake.Log)
                    .Create(settings, true);

                server.CommitChanges();

                this.cake.Information("Web Site '{0}' deployed.", settings.Name);
            }
        }

        public void DeployFtpSite(FtpSiteSettings settings)
        {
            if(settings == null) throw new ArgumentNullException("settings");

            // Prepare deployment folder
            if(!this.cake.DirectoryExists(settings.PhysicalPath))
            {
                this.cake.CreateDirectory(settings.PhysicalPath);
                this.cake.Information("Deployment folder created.");
            }

            using (var server = new ServerManager())
            {
                FtpSiteHelper
                    .Using(server, this.cake.Log)
                    .Create(settings, true);

                server.CommitChanges();

                this.cake.Information("Ftp Site '{0}' deployed.", settings.Name);
            }
        }
    }
}
