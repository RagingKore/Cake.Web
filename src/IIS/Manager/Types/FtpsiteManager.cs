#region Using Statements
    using System;
    using System.Linq;

    using Microsoft.Web.Administration;

    using Cake.Core.Diagnostics;
#endregion



namespace Cake.IIS
{
    public class FtpsiteManager : BaseSiteManager
    {
        #region Constructor (1)
            public FtpsiteManager(ServerManager server, ICakeLog log)
                : base(server, log)
            {

            }
        #endregion





        #region Fucntions (2)
            public static FtpsiteManager Using(ServerManager server, ICakeLog log)
            {
                return new FtpsiteManager(server, log);
            }



            public void Create(FtpsiteSettings settings)
            {
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }

                if (string.IsNullOrWhiteSpace(settings.Name))
                {
                    throw new ArgumentException("Site name cannot be null!");
                }

                if (string.IsNullOrWhiteSpace(settings.HostName))
                {
                    throw new ArgumentException("Host name cannot be null!");
                }



                //Get Site
                var site = Server.Sites.FirstOrDefault(p => p.Name == settings.Name);

                if(site != null)
                {
                    this.Log.Debug("Site '{0}' already exists.", settings.Name);

                    if (settings.Overwrite)
                    {
                        this.Log.Debug("Site '{0}' will be overriden by request.", settings.Name);

                        this.Delete(settings.Name);

                        ApplicationPoolManager
                            .Using(this.Server, this.Log)
                            .Delete(site.ApplicationDefaults.ApplicationPoolName);
                    }
                    else
                    {
                        return;
                    }
                }



                //Create Pool
                ApplicationPoolManager
                    .Using(this.Server, this.Log)
                    .Create(settings.ApplicationPool);



                //Add Site
                site = this.Server.Sites.Add(
                    settings.Name,
                    settings.BindingProtocol.ToString().ToLower(),
                    settings.BindingInformation,
                    settings.PhysicalPath);

                site.ServerAutoStart                         = settings.ServerAutoStart;
                site.ApplicationDefaults.ApplicationPoolName = settings.ApplicationPool.Name;



                //Set Authentication
                this.Log.Verbose("Setting up authentication mode...");
                site.SetAnonymousAuthentication(settings.EnableAnonymousAuthentication);
                site.SetBasicAuthentication(settings.EnableBasicAuthentication);

                this.Log.Verbose("Setting up authorization rules...");
                this.Server.SetAuthorization(site.Name, settings.AuthorizationSettings);



                // SSL policy
                var ssl = site
                    .GetChildElement("ftpServer")
                    .GetChildElement("security")
                    .GetChildElement("ssl");

                ssl.SetAttributeValue("controlChannelPolicy", "SslAllow");
                ssl.SetAttributeValue("dataChannelPolicy", "SslAllow");



                // Host name support
                var hostNameSupport = this.Server
                    .GetApplicationHostConfiguration()
                    .GetSection("system.ftpServer/serverRuntime")
                    .GetChildElement("hostNameSupport");
                hostNameSupport.SetAttributeValue("useDomainNameAsHostName", true);

                this.Log.Debug("Ftp Site '{0}' created.", settings.Name);
            }
        #endregion
    }
}