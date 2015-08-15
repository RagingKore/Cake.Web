#region Using Statements
    using System;
    using System.Linq;
    using System.Threading;

    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.IO.Arguments;
    using Cake.Core.Diagnostics;

using Microsoft.Web.Administration;
#endregion



namespace Cake.IIS
{
    public abstract class BaseSiteManager : BaseManager
    {
        #region Constructor (1)
            public BaseSiteManager(ICakeEnvironment environment, ICakeLog log)
                : base(environment, log)
            {

            }
        #endregion





        #region Functions (9)
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
                this.SetWorkingDirectory(settings);

                site = _Server.Sites.Add(
                    settings.Name,
                    settings.BindingProtocol.ToString().ToLower(),
                    settings.BindingInformation,
                    settings.PhysicalDirectory.MakeAbsolute(settings.WorkingDirectory).FullPath);

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

                    _Log.Information("Anonymous Authentication enabled: {0}", settings.Authentication.EnableAnonymousAuthentication);



                    // Basic Authentication
                    var basicAuthentication = site
                        .GetChildElement(server)
                        .GetChildElement("security")
                        .GetChildElement("authentication")
                        .GetChildElement("basicAuthentication");

                    basicAuthentication.SetAttributeValue("enabled", settings.Authentication.EnableBasicAuthentication);
                    basicAuthentication.SetAttributeValue("userName", settings.Authentication.Username);
                    basicAuthentication.SetAttributeValue("password", settings.Authentication.Password);

                    _Log.Information("Basic Authentication enabled: {0}", settings.Authentication.EnableBasicAuthentication);



                    // Windows Authentication
                    var windowsAuthentication = site
                        .GetChildElement(server)
                        .GetChildElement("security")
                        .GetChildElement("authentication")
                        .GetChildElement("windowsAuthentication");

                    windowsAuthentication.SetAttributeValue("enabled", settings.Authentication.EnableWindowsAuthentication);

                    _Log.Information("Windows Authentication enabled: {0}", settings.Authentication.EnableWindowsAuthentication);
                }

                return site;
            }

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
                    catch (System.Runtime.InteropServices.COMException e)
                    {
                        _Log.Information("Waiting for IIS to activate new config");
                        Thread.Sleep(1000);
                    }

                    _Log.Information("Site '{0}' started.", site.Name);
                    return true;
                }
            }

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
                    catch (System.Runtime.InteropServices.COMException e)
                    {
                        _Log.Information("Waiting for IIS to activate new config");
                        Thread.Sleep(1000);
                    }

                    _Log.Information("Site '{0}' stopped.", site.Name);
                    return true;
                }
            }

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
                    foreach (Binding b in site.Bindings)
                    {
                        if ((b.Protocol == settings.BindingProtocol.ToString()) && (b.BindingInformation == settings.BindingInformation))
                        {
                            throw new Exception("A binding with the same ip, port and host header already exists.");
                        }
                    }



                    //Add Binding
                    Binding newBinding = site.Bindings.CreateElement();

                    newBinding.Protocol = settings.BindingProtocol.ToString();
                    newBinding.BindingInformation = settings.BindingInformation;

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



                Site site = _Server.Sites.SingleOrDefault(p => p.Name == settings.Name);

                if (site != null)
                {
                    Binding binding = null;

                    foreach (Binding b in site.Bindings)
                    {
                        if ((b.Protocol == settings.BindingProtocol.ToString()) && (b.BindingInformation == settings.BindingInformation))
                        {
                            binding = b;
                        }
                    }



                    if (binding != null)
                    {
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
                        vDir.Path = settings.VirtualDirectoryPath;
                        vDir.PhysicalPath = settings.PhysicalPath;

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