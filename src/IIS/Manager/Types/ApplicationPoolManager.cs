#region Using Statements
    using System;
    using System.Linq;

    using Microsoft.Web.Administration;

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
            public ApplicationPoolManager(ServerManager server, ICakeLog log)
                : base(server, log)
            {

            }
        #endregion





        #region Functions (5)
            public static ApplicationPoolManager Using(ServerManager server, ICakeLog log)
            {
                return new ApplicationPoolManager(server, log);
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
                var pool = this.Server.ApplicationPools.FirstOrDefault(p => p.Name == settings.Name);

                if(pool != null)
                {
                    this.Log.Debug("Application pool '{0}' already exists.", settings.Name);

                    if(settings.Overwrite)
                    {
                        this.Log.Debug("Application pool '{0}' will be overriden by request.", settings.Name);
                        this.Delete(settings.Name);
                    }
                    else return;
                }
            


                //Add Pool
                pool = this.Server.ApplicationPools.Add(settings.Name);

                pool.AutoStart             = settings.Autostart;
                pool.Enable32BitAppOnWin64 = settings.Enable32BitAppOnWin64;
                pool.ManagedRuntimeVersion = settings.ManagedRuntimeVersion;
                pool.ManagedPipelineMode   = settings.ClassicManagedPipelineMode
                    ? ManagedPipelineMode.Classic
                    : ManagedPipelineMode.Integrated;



                //Set Identity
                this.Log.Debug("Application pool identity type: {0}", settings.IdentityType.ToString());

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
                pool.ProcessModel.PingInterval = settings.PingInterval;
                pool.ProcessModel.PingResponseTime = settings.PingResponseTime;

                pool.ProcessModel.IdleTimeout = settings.IdleTimeout;
                pool.ProcessModel.ShutdownTimeLimit = settings.ShutdownTimeLimit;
                pool.ProcessModel.StartupTimeLimit = settings.StartupTimeLimit;

                this.Log.Debug("Application pool created.");
            }

            public bool Delete(string name)
            {
                if (!this.IsSystemDefault(name))
                {
                    var pool = this.Server.ApplicationPools.FirstOrDefault(p => p.Name == name);

                    if (pool == null)
                    {
                        this.Log.Debug("Application pool '{0}' not found.", name);
                        return false;
                    }
                    else
                    {
                        this.Server.ApplicationPools.Remove(pool);
                        this.Log.Debug("Application pool '{0}' deleted.", pool.Name);
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
                var pool = this.Server.ApplicationPools.FirstOrDefault(p => p.Name == name);

                if (pool == null)
                {
                    this.Log.Debug("Application pool '{0}' not found.", name);
                    return false;
                }
                else
                {
                    pool.Recycle();
                    this.Log.Debug("Application pool '{0}' recycled.", pool.Name);
                    return true;
                }
            }

            public bool Start(string name)
            {
                var pool = this.Server.ApplicationPools.FirstOrDefault(p => p.Name == name);

                if (pool == null)
                {
                    this.Log.Debug("Application pool '{0}' not found.", name);
                    return false;
                }
                else
                {
                    pool.Start();
                    this.Log.Debug("Application pool '{0}' recycled.", pool.Name);
                    return true;
                }
            }

            public bool Stop(string name)
            {
                var pool = this.Server.ApplicationPools.FirstOrDefault(p => p.Name == name);

                if (pool == null)
                {
                    this.Log.Debug("Application pool '{0}' not found.", name);
                    return false;
                }
                else
                {
                    pool.Stop();
                    this.Log.Debug("Application pool '{0}' recycled.", pool.Name);
                    return true;
                }
            }

            public bool Exists(string name)
            {
                return this.Server.ApplicationPools.SingleOrDefault(p => p.Name == name) != null;
            }

            public bool IsSystemDefault(string name)
            {
                if (ApplicationPoolBlackList.Contains(name))
                {
                    return true;
                }

                this.Log.Debug("Application pool '{0}' is system's default.", name);
                return false;
            }
        #endregion
    }
}