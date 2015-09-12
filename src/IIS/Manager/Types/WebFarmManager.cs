#region Using Statements
    using System;
    using System.Linq;
    using System.Threading;

    using Microsoft.Web.Administration;

    using Cake.Core;
    using Cake.Core.Diagnostics;
#endregion



namespace Cake.IIS
{
    public class WebFarmManager : BaseManager
    {
        #region Constructor (1)
            public WebFarmManager(ICakeEnvironment environment, ICakeLog log)
                : base(environment, log)
            {

            }
        #endregion





        #region Functions (14)
            public static WebFarmManager Using(ICakeEnvironment environment, ICakeLog log, ServerManager server)
            {
                WebFarmManager manager = new WebFarmManager(environment, log);
            
                manager.SetServer(server);

                return manager;
            }



            //Farms
            public void Create(WebFarmSettings settings)
            {
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }

                if (string.IsNullOrWhiteSpace(settings.Name))
                {
                    throw new ArgumentException("WebFarm name cannot be null!");
                }



                //Get Farm
                ConfigurationElementCollection farms = this.GetFarms();
                ConfigurationElement farm = farms.FirstOrDefault(f => f.GetAttributeValue("name").ToString() == settings.Name);

                if (farm != null)
                {
                    _Log.Information("WebFarm '{0}' already exists.", settings.Name);

                    if (settings.Overwrite)
                    {
                        _Log.Information("WebFarm '{0}' will be overriden by request.", settings.Name);

                        this.Delete(settings.Name);
                    }
                    else
                    {
                        return;
                    }
                }



                //Create Farm
                farm = farms.CreateElement("webFarm");
                farm["name"] = settings.Name;



                //Add Server
                ConfigurationElementCollection servers = farm.GetCollection();

                foreach (string server in settings.Servers)
                {
                    ConfigurationElement serverElement = servers.CreateElement("server");
                    serverElement["address"] = server;

                    servers.Add(serverElement);

                    _Log.Information("Adding server '{0}'.", server);
                }

                farms.Add(farm);
                _Server.CommitChanges();

                _Log.Information("WebFarm created.");
            }

            public bool Delete(string name)
            {
                ConfigurationElementCollection farms = this.GetFarms();
                ConfigurationElement farm = farms.FirstOrDefault(f => f.GetAttributeValue("name").ToString() == name);

                if (farm != null)
                {
                    farms.Remove(farm);
                    _Server.CommitChanges();

                    _Log.Information("WebFarm deleted.");
                    return true;
                }
                else
                {
                    _Log.Information("The webfarm '{0}' does not exists.", name);
                    return false;
                }
            }
        
            public bool Exists(string name)
            {
                ConfigurationElementCollection farms = this.GetFarms();
                ConfigurationElement farm = farms.FirstOrDefault(f => f.GetAttributeValue("name").ToString() == name);

                if (farm != null)
                {
                    _Log.Information("The webfarm '{0}' exists.", name);
                    return true;
                }
                else
                {
                    _Log.Information("The webfarm '{0}' does not exists.", name);
                    return false;
                }
            }



            //Servers
            public bool AddServer(string farm, string address)
            {
                ConfigurationElement farmElement = this.GetFarm(farm);

                if (farmElement != null)
                {
                    ConfigurationElementCollection servers = farmElement.GetCollection();
                    ConfigurationElement server = servers.FirstOrDefault(f => f.GetAttributeValue("address").ToString() == address);

                    if (server == null)
                    {
                        ConfigurationElement serverElement = servers.CreateElement("server");
                        serverElement["address"] = server;

                        servers.Add(serverElement);
                        _Server.CommitChanges();

                        _Log.Information("Adding server '{0}'.", address);
                        return true;
                    }
                    else
                    {
                        _Log.Information("The server '{0}' already exists.", address);
                        return false;
                    }
                }

                return false;
            }

            public bool RemoveServer(string farm, string address)
            {
                ConfigurationElement farmElement = this.GetFarm(farm);

                if (farmElement != null)
                {
                    ConfigurationElementCollection servers = farmElement.GetCollection();
                    ConfigurationElement server = servers.FirstOrDefault(f => f.GetAttributeValue("address").ToString() == address);

                    if (server != null)
                    {
                        servers.Remove(server);
                        _Server.CommitChanges();

                        _Log.Information("Removed server '{0}'.", address);
                        return true;
                    }
                    else
                    {
                        _Log.Information("The server '{0}' does not exists.", address);
                        return false;
                    }
                }

                return false;
            }
        
            public bool ServerExists(string farm, string address)
            {
                ConfigurationElement farmElement = this.GetFarm(farm);

                if (farmElement != null)
                {
                    ConfigurationElementCollection servers = farmElement.GetCollection();
                    ConfigurationElement server = servers.FirstOrDefault(f => f.GetAttributeValue("address").ToString() == address);

                    if (server != null)
                    {
                        _Log.Information("The server '{0}' exists.", address);
                        return true;
                    }
                    else
                    {
                        _Log.Information("The server '{0}' does not exists.", address);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }



            //Health
            public void SetServerHealthy(string farm, string address)
            {
                ConfigurationElement arrElement = this.GetServerArr(farm, address);

                if (arrElement != null)
                {
                    ConfigurationMethod method = arrElement.Methods["SetHealthy"];
                    ConfigurationMethodInstance instance = method.CreateInstance();

                    instance.Execute();

                    _Log.Information("Marking the server '{0}' as healthy.", address);
                }
            }

            public void SetServerUnhealthy(string farm, string address)
            {
                ConfigurationElement arrElement = this.GetServerArr(farm, address);

                if (arrElement != null)
                {
                    ConfigurationMethod method = arrElement.Methods["SetUnhealthy"];
                    ConfigurationMethodInstance instance = method.CreateInstance();

                    instance.Execute();

                    _Log.Information("Marking the server '{0}' as unhealthy.", address);
                }
            }



            //Available
            public void SetServerAvailable(string farm, string address)
            {
                ConfigurationElement arrElement = this.GetServerArr(farm, address);

                if (arrElement != null)
                {
                    ConfigurationMethod method = arrElement.Methods["SetState"];
                    ConfigurationMethodInstance instance = method.CreateInstance();

                    instance.Input.Attributes[0].Value = 0;
                    instance.Execute();

                    _Log.Information("Marking the server '{0}' as available.", address);
                }
            }

            public void SetServerUnavailable(string farm, string address)
            {
                ConfigurationElement arrElement = this.GetServerArr(farm, address);

                if (arrElement != null)
                {
                    ConfigurationMethod method = arrElement.Methods["SetState"];
                    ConfigurationMethodInstance instance = method.CreateInstance();

                    instance.Input.Attributes[0].Value = 3;
                    instance.Execute();

                    _Log.Information("Marking the server '{0}' as unavailable.", address);
                }
            }



            //State
            public bool GetServerIsHealthy(string farm, string address)
            {
                ConfigurationElement arrElement = this.GetServerArr(farm, address);

                if (arrElement != null)
                {
                    ConfigurationElement countersElement = arrElement.GetChildElement("counters");

                    string value = countersElement.GetAttributeValue("isHealthy").ToString().ToLower();

                    return (value == "true");
                }
                else
                {
                    return false;
                }
            }

            public string GetServerState(string farm, string address)
            {
                ConfigurationElement arrElement = this.GetServerArr(farm, address);

                if (arrElement != null)
                {
                    ConfigurationElement countersElement = arrElement.GetChildElement("counters");

                    return countersElement.GetAttributeValue("isHealthy").ToString();
                }
                else
                {
                    return "";
                }
            }



            //Helpers
            private ConfigurationElementCollection GetFarms()
            {
                Configuration config = _Server.GetApplicationHostConfiguration();
                ConfigurationSection section = config.GetSection("webFarms");

                return section.GetCollection();
            }

            private ConfigurationElement GetFarm(string name)
            {
                ConfigurationElementCollection farms = this.GetFarms();
                ConfigurationElement farm = farms.FirstOrDefault(f => f.GetAttributeValue("name").ToString() == name);

                if (farm == null)
                {
                    _Log.Information("The webfarm '{0}' does not exists.", name);
                }

                return farm;
            }

            private ConfigurationElement GetServer(ConfigurationElement farm, string address)
            {
                ConfigurationElementCollection servers = farm.GetCollection();
                ConfigurationElement server = servers.FirstOrDefault(f => f.GetAttributeValue("address").ToString() == address);

                if (server == null)
                {
                    _Log.Information("The server '{0}' does not exists.", address);
                }

                return server;
            }

            private ConfigurationElement GetServerArr(string farm, string address)
            {
                ConfigurationElement farmElement = this.GetFarm(farm);

                if (farmElement != null)
                {
                    ConfigurationElement serverElement = this.GetServer(farmElement, address);

                    if (serverElement != null)
                    {
                        return serverElement.GetChildElement("applicationRequestRouting");
                    }
                }

                return null;
            }
        #endregion
    }
}