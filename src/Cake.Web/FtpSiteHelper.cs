using System;
using System.Linq;
using Cake.Core.Diagnostics;
using Microsoft.Web.Administration;

namespace Cake.Web
{
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
                        .Delete(site.ApplicationDefaults.ApplicationPoolName);
                }
                else return;
            }

            ApplicationPoolHelper
                .Using(Server, Log)
                .Create(settings.ApplicationPool, true);

            site = Server.Sites.Add(
                settings.Name,
                settings.BindingProtocol.ToString().ToLower(),
                settings.BindingInformation,
                settings.PhysicalPath);

            site.ServerAutoStart                         = settings.ServerAutoStart;
            site.ApplicationDefaults.ApplicationPoolName = settings.ApplicationPool.Name;

            Log.Verbose("Setting up authentication mode...");
            site.SetAnonymousAuthentication(settings.EnableAnonymousAuthentication);

            // basic auth
            site.SetBasicAuthentication(settings.EnableBasicAuthentication);

            if(settings.EnableBasicAuthentication)
            {
                Log.Verbose("Setting up user's authorization using basic authentication...");
                Server.SetAuthorization(site.Name, settings.AuthorizationSettings);
            }
            
            // SSL policy
            var ssl = site
                .GetChildElement("ftpServer")
                .GetChildElement("security")
                .GetChildElement("ssl");
            ssl.SetAttributeValue("controlChannelPolicy", "SslAllow");
            ssl.SetAttributeValue("dataChannelPolicy", "SslAllow");

            // Host name support

            var hostNameSupport = Server
                .GetApplicationHostConfiguration()
                .GetSection("system.ftpServer/serverRuntime")
                .GetChildElement("hostNameSupport");
            hostNameSupport.SetAttributeValue("useDomainNameAsHostName", true);


            //var serverRuntime = site
            //    .GetChildElement("ftpServer")
            //    .GetCollection()
            //    .CreateElement("serverRuntime");

            //var hostNameSupport = serverRuntime
            //    .GetCollection()
            //    .CreateElement("hostNameSupport");
            //hostNameSupport.SetAttributeValue("useDomainNameAsHostName", true);

            Log.Debug("Ftp Site '{0}' created.", settings.Name);
        }
    }
}