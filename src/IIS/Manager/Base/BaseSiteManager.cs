#region Using Statements
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Cake.Core;
    using Cake.Core.Diagnostics;

    using Microsoft.Web.Administration;
#endregion



namespace Cake.IIS
{
     /// <summary>
    /// Base class for managing IIS sites
    /// </summary>
    public abstract class BaseSiteManager : BaseManager
    {
        #region Constructor (1)
            /// <summary>
            /// Initializes a new instance of the <see cref="BaseSiteManager" /> class.
            /// </summary>
            /// <param name="environment">The environment.</param>
            /// <param name="log">The log.</param>
            public BaseSiteManager(ICakeEnvironment environment, ICakeLog log)
                : base(environment, log)
            {

            }
        #endregion





        #region Functions (9)
            /// <summary>
            /// Creates a IIS site
            /// </summary>
            /// <param name="settings">The setting of the site</param>
            /// <param name="exists">Check if the site exists</param>
            /// <returns>IIS Site.</returns>
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
                Site site = _Server.Sites.FirstOrDefault(p => p.Name == settings.Name);

                if (site != null)
                {
                    _Log.Information("Site '{0}' already exists.", settings.Name);

                    if (settings.Overwrite)
                    {
                        _Log.Information("Site '{0}' will be overriden by request.", settings.Name);

                        this.Delete(settings.Name);

                        ApplicationPoolManager
                            .Using(_Environment, _Log, _Server)
                            .Delete(site.ApplicationDefaults.ApplicationPoolName);

                        exists = false;
                    }
                    else
                    {
                        exists = true;
                        return site;
                    }
                }
                else
                {
                    exists = false;
                }



                //Create Pool
                ApplicationPoolManager
                    .Using(_Environment, _Log, _Server)
                    .Create(settings.ApplicationPool);



                //Site Settings
                site = _Server.Sites.Add(
                    settings.Name,
                    settings.BindingProtocol.ToString().ToLower(),
                    settings.BindingInformation,
                    this.GetPhysicalDirectory(settings));

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



                //Security
                this.SetAuthentication(settings);
                this.SetAuthorization(settings);

                return site;
            }

            /// <summary>
            /// Sets the authentication settings for the site
            /// </summary>
            /// <param name="settings">The site settings</param>
            protected void SetAuthentication(SiteSettings settings)
            {
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



                    //Authentication
                    var config = _Server.GetApplicationHostConfiguration();
                    var authentication = config.GetSection("system." + server + "/security/authorization", settings.Name);



                    // Anonymous Authentication
                    var anonymousAuthentication = authentication.GetChildElement("anonymousAuthentication");

                    anonymousAuthentication.SetAttributeValue("enabled", settings.Authentication.EnableAnonymousAuthentication);

                    _Log.Information("Anonymous Authentication enabled: {0}", settings.Authentication.EnableAnonymousAuthentication);



                    // Basic Authentication
                    var basicAuthentication = authentication.GetChildElement("basicAuthentication");

                    basicAuthentication.SetAttributeValue("enabled", settings.Authentication.EnableBasicAuthentication);
                    basicAuthentication.SetAttributeValue("userName", settings.Authentication.Username);
                    basicAuthentication.SetAttributeValue("password", settings.Authentication.Password);

                    _Log.Information("Basic Authentication enabled: {0}", settings.Authentication.EnableBasicAuthentication);



                    // Windows Authentication
                    var windowsAuthentication = authentication.GetChildElement("windowsAuthentication");

                    windowsAuthentication.SetAttributeValue("enabled", settings.Authentication.EnableWindowsAuthentication);

                    _Log.Information("Windows Authentication enabled: {0}", settings.Authentication.EnableWindowsAuthentication);
                }
            }

            /// <summary>
            /// Sets the authorization settings for the site
            /// </summary>
            /// <param name="settings">The site settings</param>
            protected void SetAuthorization(SiteSettings settings)
            {
                if (settings.Authorization != null)
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



                    //Authorization
                    var config = _Server.GetApplicationHostConfiguration();
                    var authorization = config.GetSection("system." + server + "/security/authorization", settings.Name);
                    var authCollection = authorization.GetCollection();

                    var addElement = authCollection.CreateElement("add");
                    addElement.SetAttributeValue("accessType", "Allow");

                    switch (settings.Authorization.AuthorizationType)
                    {
                        case AuthorizationType.AllUsers:
                            addElement.SetAttributeValue("users", "*");
                            break;

                        case AuthorizationType.SpecifiedUser:
                            addElement.SetAttributeValue("users", string.Join(", ", settings.Authorization.Users));
                            break;

                        case AuthorizationType.SpecifiedRoleOrUserGroup:
                            addElement.SetAttributeValue("roles", string.Join(", ", settings.Authorization.Roles));
                            break;
                    }



                    //Permissions
                    var permissions = new List<string>();

                    if (settings.Authorization.CanRead)
                    {
                        permissions.Add("Read");
                    }
                    if (settings.Authorization.CanWrite)
                    {
                        permissions.Add("Write");
                    }

                    addElement.SetAttributeValue("permissions", string.Join(", ", permissions));

                    authCollection.Clear();
                    authCollection.Add(addElement);

                    _Log.Information("Windows Authentication enabled: {0}", settings.Authentication.EnableWindowsAuthentication);
                }
            }



            /// <summary>
            /// Delets a site from IIS
            /// </summary>
            /// <param name="name">The name of the site to delete</param>
            /// <returns>If the site was deleted.</returns>
            public bool Delete(string name)
            {
                var site = _Server.Sites.FirstOrDefault(p => p.Name == name);

                if (site == null)
                {
                    _Log.Information("Site '{0}' not found.", name);
                    return true;
                }
                else
                {
                    _Server.Sites.Remove(site);
                    _Server.CommitChanges();

                    _Log.Information("Site '{0}' deleted.", site.Name);
                    return false;
                }
            }

            /// <summary>
            /// Checks if a site exists in IIS
            /// </summary>
            /// <param name="name">The name of the site to check</param>
            /// <returns>If the site exists.</returns>
            public bool Exists(string name)
            {
                if (_Server.Sites.SingleOrDefault(p => p.Name == name) != null)
                {
                    _Log.Information("The site '{0}' exists.", name);
                    return true;
                }
                else
                {
                    _Log.Information("The site '{0}' does not exist.", name);
                    return false;
                }
            }



            /// <summary>
            /// Starts a IIS site
            /// </summary>
            /// <param name="name">The name of the site to start</param>
            /// <returns>If the site was started.</returns>
            public bool Start(string name)
            {
                var site = _Server.Sites.FirstOrDefault(p => p.Name == name);

                if (site == null)
                {
                    _Log.Information("Site '{0}' not found.", name);
                    return false;
                }
                else
                {
                    try
                    {
                        site.Start();
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        _Log.Information("Waiting for IIS to activate new config");
                        Thread.Sleep(1000);
                    }

                    _Log.Information("Site '{0}' started.", site.Name);
                    return true;
                }
            }

            /// <summary>
            /// Stops a IIS site
            /// </summary>
            /// <param name="name">The name of the site to stop</param>
            /// <returns>If the site was stopped.</returns>
            public bool Stop(string name)
            {
                var site = _Server.Sites.FirstOrDefault(p => p.Name == name);

                if (site == null)
                {
                    _Log.Information("Site '{0}' not found.", name);
                    return false;
                }
                else
                {
                    try
                    {
                        site.Stop();
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        _Log.Information("Waiting for IIS to activate new config");
                        Thread.Sleep(1000);
                    }

                    _Log.Information("Site '{0}' stopped.", site.Name);
                    return true;
                }
            }



            public bool AddBinding(BindingSettings settings)
            {
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }

                if (string.IsNullOrWhiteSpace(settings.Name))
                {
                    throw new ArgumentException("Site name cannot be null!");
                }



                //Get Site
                Site site = _Server.Sites.SingleOrDefault(p => p.Name == settings.Name);

                if (site != null)
                {
                    if (site.Bindings.FirstOrDefault(b => (b.Protocol == settings.BindingProtocol.ToString()) && (b.BindingInformation == settings.BindingInformation)) != null)
                    {
                        throw new Exception("A binding with the same ip, port and host header already exists.");
                    }



                    //Add Binding
                    Binding newBinding = site.Bindings.CreateElement();

                    newBinding.Protocol = settings.BindingProtocol.ToString();
                    newBinding.BindingInformation = settings.BindingInformation;

                    if (settings.CertificateHash != null)
                    {
                        newBinding.CertificateHash = settings.CertificateHash;
                    }

                    if (!String.IsNullOrEmpty(settings.CertificateStoreName))
                    {
                        newBinding.CertificateStoreName = settings.CertificateStoreName;
                    }

                    site.Bindings.Add(newBinding);
                    _Server.CommitChanges();

                    _Log.Information("Binding added.");
                    return true;
                }
                else
                {
                    throw new Exception("Site: " + settings.Name + " does not exist.");
                }
            }

            public bool RemoveBinding(BindingSettings settings)
            {
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }

                if (string.IsNullOrWhiteSpace(settings.Name))
                {
                    throw new ArgumentException("Site name cannot be null!");
                }



                //Get Site
                Site site = _Server.Sites.SingleOrDefault(p => p.Name == settings.Name);

                if (site != null)
                {
                    Binding binding = site.Bindings.FirstOrDefault(b => (b.Protocol == settings.BindingProtocol.ToString()) && (b.BindingInformation == settings.BindingInformation));

                    if (binding != null)
                    {
                        //Remove Binding
                        site.Bindings.Remove(binding);
                        _Server.CommitChanges();

                        _Log.Information("Binding removed.");
                        return true;
                    }
                    else
                    {
                        _Log.Information("A binding with the same ip, port and host header does not exists.");
                        return false;
                    }
                }
                else
                {
                    throw new Exception("Site: " + settings.Name + " does not exist.");
                }
            }



            public bool AddApplication(ApplicationSettings settings)
            {
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }

                if (string.IsNullOrWhiteSpace(settings.SiteName))
                {
                    throw new ArgumentException("Site name cannot be null!");
                }

                if (string.IsNullOrWhiteSpace(settings.ApplicationPath))
                {
                    throw new ArgumentException("Applicaiton path cannot be null!");
                }



                //Get Pool
                ApplicationPool appPool = _Server.ApplicationPools.SingleOrDefault(p => p.Name == settings.ApplicationPool);

                if (appPool == null)
                {
                    throw new Exception("Application Pool '" + settings.ApplicationPool + "' does not exist.");
                }



                //Get Site
                Site site = _Server.Sites.SingleOrDefault(p => p.Name == settings.SiteName);

                if (site != null)
                {
                    //Get Application
                    Application app = site.Applications.SingleOrDefault(p => p.Path == settings.ApplicationPath);

                    if (app != null)
                    {
                        throw new Exception("Application '" + settings.ApplicationPath + "' already exists.");
                    }
                    else
                    {
                        app = site.Applications.CreateElement();
                        app.Path = settings.ApplicationPath;
                        app.ApplicationPoolName = settings.ApplicationPool;



                        //Get Directory
                        VirtualDirectory vDir = app.VirtualDirectories.CreateElement();
                        vDir.Path = settings.VirtualDirectory;
                        vDir.PhysicalPath = this.GetPhysicalDirectory(settings);

                        if (!string.IsNullOrEmpty(settings.UserName))
                        {
                            if (string.IsNullOrEmpty(settings.Password))
                            {
                                throw new Exception("Invalid Virtual Directory User Account Password.");
                            }
                            else
                            {
                                vDir.UserName = settings.UserName;
                                vDir.Password = settings.Password;
                            }
                        }

                        app.VirtualDirectories.Add(vDir);
                    }

                    site.Applications.Add(app);
                    _Server.CommitChanges();

                    return true;
                }
                else
                {
                    throw new Exception("Site '" + settings.SiteName + "' does not exist.");
                }
            }

            public bool RemoveApplication(ApplicationSettings settings)
            {
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }

                if (string.IsNullOrWhiteSpace(settings.SiteName))
                {
                    throw new ArgumentException("Site name cannot be null!");
                }

                if (string.IsNullOrWhiteSpace(settings.ApplicationPath))
                {
                    throw new ArgumentException("Applicaiton path cannot be null!");
                }



                //Get Pool
                ApplicationPool appPool = _Server.ApplicationPools.SingleOrDefault(p => p.Name == settings.ApplicationPool);

                if (appPool == null)
                {
                    throw new Exception("Application Pool '" + settings.ApplicationPool + "' does not exist.");
                }



                //Get Site
                Site site = _Server.Sites.SingleOrDefault(p => p.Name == settings.SiteName);

                if (site != null)
                {
                    //Get Application
                    Application app = site.Applications.SingleOrDefault(p => p.Path == settings.ApplicationPath);

                    if (app == null)
                    {
                        throw new Exception("Application '" + settings.ApplicationPath + "' does not exists.");
                    }
                    else
                    {
                        site.Applications.Remove(app);
                        _Server.CommitChanges();

                        return true;
                    }
                }
                else
                {
                    throw new Exception("Site '" + settings.SiteName + "' does not exist.");
                }
            }
        #endregion
    }
}