using System;
using System.Linq;
using Cake.Core.Diagnostics;
using Microsoft.Web.Administration;

namespace Cake.Web
{
    public class ApplicationPoolHelper
    {
        private static readonly string[] ApplicationPoolBlackList =
        {
            "DefaultAppPool",
            "Classic .NET AppPool",
            "ASP.NET v4.0 Classic",
            "ASP.NET v4.0"
        };

        private readonly ServerManager server;
        private readonly ICakeLog log;

        public ApplicationPoolHelper(ServerManager server, ICakeLog log)
        {
            this.server = server;
            this.log    = log;
        }

        public static ApplicationPoolHelper Using(ServerManager server, ICakeLog log)
        {
            return new ApplicationPoolHelper(server, log);
        }

        public void Create(ApplicationPoolSettings settings, bool overwrite = false)
        {
            if(settings == null) 
                throw new ArgumentNullException("settings");

            if(string.IsNullOrWhiteSpace(settings.Name)) 
                throw new ArgumentException("Application pool name cannot be null!");

            if(this.IsSystemDefault(settings.Name)) return;

            var pool = this.server.ApplicationPools.FirstOrDefault(p => p.Name == settings.Name);

            if(pool != null)
            {
                this.log.Debug("Application pool '{0}' already exists.", settings.Name);

                if(overwrite)
                {
                    this.log.Debug("Application pool '{0}' will be overriden by request.", settings.Name);
                    this.Delete(settings.Name);
                }
                else return;
            }
            
            pool = this.server.ApplicationPools.Add(settings.Name);

            pool.AutoStart             = settings.Autostart;
            pool.Enable32BitAppOnWin64 = settings.Enable32BitAppOnWin64;
            pool.ManagedRuntimeVersion = settings.ManagedRuntimeVersion;
            pool.ManagedPipelineMode   = settings.ClassicManagedPipelineMode
                ? ManagedPipelineMode.Classic
                : ManagedPipelineMode.Integrated;

            this.log.Debug("Application pool identity type: {0}", settings.IdentityType.ToString());

            switch(settings.IdentityType)
            {
                case ApplicationPoolIdentityType.LocalSystem:
                    pool.ProcessModel.IdentityType = ProcessModelIdentityType.LocalSystem;
                    break;
                case ApplicationPoolIdentityType.LocalService:
                    pool.ProcessModel.IdentityType = ProcessModelIdentityType.LocalService;
                    break;
                case ApplicationPoolIdentityType.NetworkService:
                    pool.ProcessModel.IdentityType = ProcessModelIdentityType.NetworkService;
                    break;
                case ApplicationPoolIdentityType.ApplicationPoolIdentity:
                    pool.ProcessModel.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;
                    break;
                case ApplicationPoolIdentityType.SpecificUser:
                    pool.ProcessModel.IdentityType = ProcessModelIdentityType.SpecificUser;
                    pool.ProcessModel.UserName     = settings.Username;
                    pool.ProcessModel.Password     = settings.Password;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.log.Debug("Application pool created.");
        }

        public void Delete(string name)
        {
            if(!this.IsSystemDefault(name))
            {
                var pool = this.server.ApplicationPools
                    .FirstOrDefault(p => p.Name == name);

                if(pool == null)
                    this.log.Debug("Application pool '{0}' not found.", name);
                else
                {
                    this.server.ApplicationPools.Remove(pool);
                    this.log.Debug("Application pool '{0}' deleted.", pool.Name);
                }
            }
        }

        public void Recycle(string name)
        {
            var pool = this.server.ApplicationPools
                .FirstOrDefault(p => p.Name == name);

            if(pool == null)
                this.log.Debug("Application pool '{0}' not found.", name);
            else
            {
                pool.Recycle();
                this.log.Debug("Application pool '{0}' recycled.", pool.Name);
            }
        }

        public bool Check(string name)
        {
            return this.server.ApplicationPools
                .SingleOrDefault(p => p.Name == name) != null;
        }

        public bool IsSystemDefault(string name)
        {
            if(ApplicationPoolBlackList.Contains(name)) return true;
            this.log.Debug("Application pool '{0}' is system's default.", name);
            return false;
        }
    }
}