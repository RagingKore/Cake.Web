#region Using Statements
    using System;
    using System.Linq;

    using Microsoft.Web.Administration;

    using Cake.Core.Diagnostics;
#endregion



namespace Cake.IIS
{
    public class WebsiteManager : BaseSiteManager
    {
        #region Constructor (1)
            public WebsiteManager(ServerManager server, ICakeLog log)
                : base(server, log)
            {

            }
        #endregion





        #region Functions (2)
            public static WebsiteManager Using(ServerManager server, ICakeLog log)
            {
                return new WebsiteManager(server, log);
            }



            public void Create(WebsiteSettings settings)
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
                var site = this.Server.Sites.FirstOrDefault(p => p.Name == settings.Name);

                if(site != null)
                {
                    this.Log.Debug("Site '{0}' already exists.", settings.Name);

                    if(settings.Overwrite)
                    {
                        this.Log.Debug("Site '{0}' will be overriden by request.", settings.Name);

                        this.Delete(settings.Name);

                        ApplicationPoolManager
                            .Using(this.Server, this.Log)
                            .Delete(site.ApplicationDefaults.ApplicationPoolName);
                    }
                    else return;
                }



                //Create Pool
                ApplicationPoolManager
                    .Using(this.Server, this.Log)
                    .Create(settings.ApplicationPool);



                //Add Site
                site = Server.Sites.Add(
                    settings.Name,
                    settings.BindingProtocol.ToString().ToLower(),
                    settings.BindingInformation,
                    settings.PhysicalPath);

                site.ServerAutoStart = settings.ServerAutoStart;
                site.ApplicationDefaults.ApplicationPoolName = settings.ApplicationPool.Name;



                //Set Authentication
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

                    this.Log.Debug("Anonymous Authentication enabled: {0}", settings.Authentication.EnableAnonymousAuthentication);



                    // Basic Authentication
                    var basicAuthentication = site
                        .GetChildElement("webServer")
                        .GetChildElement("security")
                        .GetChildElement("authentication")
                        .GetChildElement("basicAuthentication");

                    basicAuthentication.SetAttributeValue("enabled", settings.Authentication.EnableBasicAuthentication);

                    this.Log.Debug("Basic Authentication enabled: {0}", settings.Authentication.EnableBasicAuthentication);



                    // Windows Authentication
                    var windowsAuthentication = site
                        .GetChildElement("webServer")
                        .GetChildElement("security")
                        .GetChildElement("authentication")
                        .GetChildElement("windowsAuthentication");

                    windowsAuthentication.SetAttributeValue("enabled", true);

                    this.Log.Debug("Windows Authentication enabled: {0}", settings.Authentication.EnableWindowsAuthentication);
                }

                this.Log.Debug("Web Site '{0}' created.", settings.Name);
            }
        #endregion
    }
}