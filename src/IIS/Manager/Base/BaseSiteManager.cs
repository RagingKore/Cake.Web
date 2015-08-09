#region Using Statements
    using System;
    using System.Linq;

    using Cake.Core.Diagnostics;

    using Microsoft.Web.Administration;
#endregion



namespace Cake.IIS
{
    public abstract class BaseSiteManager : BaseManager
    {
        #region Constructor (1)
            public BaseSiteManager(ServerManager server, ICakeLog log)
                : base(server, log)
            {

            }
        #endregion





        #region Functions (4)
            protected Site CreateSite(SiteSettings settings, out bool exists)
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
                Site site = this.Server.Sites.FirstOrDefault(p => p.Name == settings.Name);

                if (site != null)
                {
                    exists = true;
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
                        return site;
                    }
                }
                else
                {
                    exists = false;
                }



                //Create Pool
                ApplicationPoolManager
                    .Using(this.Server, this.Log)
                    .Create(settings.ApplicationPool);



                //Site Settings
                site = this.Server.Sites.Add(
                    settings.Name,
                    settings.BindingProtocol.ToString().ToLower(),
                    settings.BindingInformation,
                    settings.PhysicalPath);

                if (settings.CertificateHash != null)
                {
                    site.Bindings[0].CertificateHash = settings.CertificateHash;
                }

                if (!String.IsNullOrEmpty(settings.CertificateStoreName))
                {
                    site.Bindings[0].CertificateStoreName = settings.CertificateStoreName;
                }

                site.ServerAutoStart = settings.ServerAutoStart;
                site.ApplicationDefaults.ApplicationPoolName = settings.ApplicationPool.Name;



                //Set Authentication
                if (settings.Authentication != null)
                {
                    //Get Type
                    string server = "";

                    if (settings is WebsiteSettings)
                    {
                        server = "webServer";
                    }
                    else
                    {
                        server = "ftpServer";
                    }



                    // Anonymous Authentication
                    var anonymousAuthentication = site
                        .GetChildElement(server)
                        .GetChildElement("security")
                        .GetChildElement("authentication")
                        .GetChildElement("anonymousAuthentication");

                    anonymousAuthentication.SetAttributeValue("enabled", settings.Authentication.EnableAnonymousAuthentication);

                    this.Log.Debug("Anonymous Authentication enabled: {0}", settings.Authentication.EnableAnonymousAuthentication);



                    // Basic Authentication
                    var basicAuthentication = site
                        .GetChildElement(server)
                        .GetChildElement("security")
                        .GetChildElement("authentication")
                        .GetChildElement("basicAuthentication");

                    basicAuthentication.SetAttributeValue("enabled", settings.Authentication.EnableBasicAuthentication);
                    basicAuthentication.SetAttributeValue("userName", settings.Authentication.Username);
                    basicAuthentication.SetAttributeValue("password", settings.Authentication.Password);

                    this.Log.Debug("Basic Authentication enabled: {0}", settings.Authentication.EnableBasicAuthentication);



                    // Windows Authentication
                    var windowsAuthentication = site
                        .GetChildElement(server)
                        .GetChildElement("security")
                        .GetChildElement("authentication")
                        .GetChildElement("windowsAuthentication");

                    windowsAuthentication.SetAttributeValue("enabled", settings.Authentication.EnableWindowsAuthentication);

                    this.Log.Debug("Windows Authentication enabled: {0}", settings.Authentication.EnableWindowsAuthentication);
                }

                return site;
            }



            public bool Delete(string name)
            {
                var site = this.Server.Sites.FirstOrDefault(p => p.Name == name);

                if (site == null)
                {
                    this.Log.Debug("Site '{0}' not found.", name);
                    return true;
                }
                else
                {
                    this.Server.Sites.Remove(site);
                    this.Log.Debug("Site '{0}' deleted.", site.Name);
                    return false;
                }
            }



            public bool Start(string name)
            {
                var site = this.Server.Sites.FirstOrDefault(p => p.Name == name);

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
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public bool Stop(string name)
            {
                var site = this.Server.Sites.FirstOrDefault(p => p.Name == name);

                if (site != null)
                {
                    ObjectState state;

                    do
                    {
                        this.Log.Verbose("Site '{0}' stopping...", site.Name);
                        state = site.Start();
                    }
                    while (state != ObjectState.Stopped);

                    this.Log.Debug("Site '{0}' stopped.", site.Name);
                    return true;
                }
                else
                {
                    return false;
                }
            }



            public bool Exists(string name)
            {
                return this.Server.Sites.SingleOrDefault(p => p.Name == name) != null;
            }
        #endregion
    }
}