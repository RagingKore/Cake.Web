using System;
using System.Linq;
using Cake.Core.Diagnostics;
using Microsoft.Web.Administration;

namespace Cake.Web
{
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
}