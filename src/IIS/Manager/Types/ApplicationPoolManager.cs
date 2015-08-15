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
    public class ApplicationPoolManager : BaseManager
    {
        #region Fields (1)
            private static readonly string[] ApplicationPoolBlackList =
            {
                    "DefaultAppPool",
                    "Classic .NET AppPool",
                    "ASP.NET v4.0 Classic",
                    "ASP.NET v4.0"
            };
        #endregion





        #region Constructor (1)
            public ApplicationPoolManager(ICakeEnvironment environment, ICakeLog log)
                    : base(environment, log)
            {

            }
        #endregion





        #region Functions (8)
            public static ApplicationPoolManager Using(ICakeEnvironment environment, ICakeLog log, ServerManager server)
            {
                ApplicationPoolManager manager = new ApplicationPoolManager(environment, log);
            
                manager.SetServer(server);

                return manager;
            }



            public void Create(ApplicationPoolSettings settings)
            {
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }

                if (string.IsNullOrWhiteSpace(settings.Name))
                {
                    throw new ArgumentException("Application pool name cannot be null!");
                }

                if (this.IsSystemDefault(settings.Name))
                {
                    return;
                }



                //Get Pool
                var pool = _Server.ApplicationPools.FirstOrDefault(p => p.Name == settings.Name);

                if(pool != null)
                {
                    _Log.Information("Application pool '{0}' already exists.", settings.Name);

                    if(settings.Overwrite)
                    {
                        _Log.Information("Application pool '{0}' will be overriden by request.", settings.Name);
                        this.Delete(settings.Name);
                    }
                    else return;
                }
            


                //Add Pool
                pool = _Server.ApplicationPools.Add(settings.Name);

                pool.AutoStart             = settings.Autostart;
                pool.Enable32BitAppOnWin64 = settings.Enable32BitAppOnWin64;
                pool.ManagedRuntimeVersion = settings.ManagedRuntimeVersion;
                pool.ManagedPipelineMode   = settings.ClassicManagedPipelineMode
                    ? ManagedPipelineMode.Classic
                    : ManagedPipelineMode.Integrated;



                //Set Identity
                _Log.Information("Application pool identity type: {0}", settings.IdentityType.ToString());

                switch(settings.IdentityType)
                {
                    case IdentityType.LocalSystem:
                        pool.ProcessModel.IdentityType = ProcessModelIdentityType.LocalSystem;
                        break;
                    case IdentityType.LocalService:
                        pool.ProcessModel.IdentityType = ProcessModelIdentityType.LocalService;
                        break;
                    case IdentityType.NetworkService:
                        pool.ProcessModel.IdentityType = ProcessModelIdentityType.NetworkService;
                        break;
                    case IdentityType.ApplicationPoolIdentity:
                        pool.ProcessModel.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;
                        break;
                    case IdentityType.SpecificUser:
                        pool.ProcessModel.IdentityType = ProcessModelIdentityType.SpecificUser;
                        pool.ProcessModel.UserName     = settings.Username;
                        pool.ProcessModel.Password     = settings.Password;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }



                //Set ProcessModel
                pool.ProcessModel.LoadUserProfile = settings.LoadUserProfile;
                pool.ProcessModel.MaxProcesses = settings.MaxProcesses;

                pool.ProcessModel.PingingEnabled = settings.PingingEnabled;

                if (settings.PingResponseTime != TimeSpan.MinValue)
                {
                    pool.ProcessModel.PingInterval = settings.PingInterval;
                }
                if (settings.PingResponseTime != TimeSpan.MinValue)
                {
                    pool.ProcessModel.PingResponseTime = settings.PingResponseTime;
                }
                if (settings.IdleTimeout != TimeSpan.MinValue)
                {
                    pool.ProcessModel.IdleTimeout = settings.IdleTimeout;
                }
                if (settings.ShutdownTimeLimit != TimeSpan.MinValue)
                {
                    pool.ProcessModel.ShutdownTimeLimit = settings.ShutdownTimeLimit;
                }
                if (settings.StartupTimeLimit != TimeSpan.MinValue)
                {
                    pool.ProcessModel.StartupTimeLimit = settings.StartupTimeLimit;
                }



                _Server.CommitChanges();

                _Log.Information("Application pool created.");
            }

            public bool Delete(string name)
            {
                if (!this.IsSystemDefault(name))
                {
                    var pool = _Server.ApplicationPools.FirstOrDefault(p => p.Name == name);

                    if (pool == null)
                    {
                        _Log.Information("Application pool '{0}' not found.", name);
                        return false;
                    }
                    else
                    {
                        _Server.ApplicationPools.Remove(pool);
                        _Server.CommitChanges();

                        _Log.Information("Application pool '{0}' deleted.", pool.Name);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            public bool Recycle(string name)
            {
                var pool = _Server.ApplicationPools.FirstOrDefault(p => p.Name == name);

                if (pool == null)
                {
                    _Log.Information("Application pool '{0}' not found.", name);
                    return false;
                }
                else
                {
                    try
                    {
                        pool.Recycle();
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        _Log.Information("Waiting for IIS to activate new config");
                        Thread.Sleep(1000);
                    }

                    _Log.Information("Application pool '{0}' recycled.", pool.Name);
                    return true;
                }
            }

            public bool Start(string name)
            {
                var pool = _Server.ApplicationPools.FirstOrDefault(p => p.Name == name);

                if (pool == null)
                {
                    _Log.Information("Application pool '{0}' not found.", name);
                    return false;
                }
                else
                {
                    try
                    {
                        pool.Start();
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        _Log.Information("Waiting for IIS to activate new config");
                        Thread.Sleep(1000);
                    }

                    _Log.Information("Application pool '{0}' started.", pool.Name);
                    return true;
                }
            }

            public bool Stop(string name)
            {
                var pool = _Server.ApplicationPools.FirstOrDefault(p => p.Name == name);

                if (pool == null)
                {
                    _Log.Information("Application pool '{0}' not found.", name);
                    return false;
                }
                else
                {
                    try
                    {
                        pool.Stop();
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        _Log.Information("Waiting for IIS to activate new config");
                        Thread.Sleep(1000);
                    }

                    _Log.Information("Application pool '{0}' stopped.", pool.Name);
                    return true;
                }
            }

            public bool Exists(string name)
            {
                if (_Server.ApplicationPools.SingleOrDefault(p => p.Name == name) != null)
                {
                    _Log.Information("The ApplicationPool '{0}' exists.", name);
                    return true;
                }
                else
                {
                    _Log.Information("The ApplicationPool '{0}' does not exist.", name);
                    return false;
                }
            }

            public bool IsSystemDefault(string name)
            {
                if (ApplicationPoolBlackList.Contains(name))
                {
                    return true;
                }

                _Log.Information("Application pool '{0}' is system's default.", name);
                return false;
            }
        #endregion
    }
}