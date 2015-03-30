using System.Linq;
using Cake.Core.Diagnostics;
using Microsoft.Web.Administration;

namespace Cake.Web
{
    public abstract class SiteHelperBase
    {
        public readonly ServerManager Server;
        public readonly ICakeLog Log;

        protected SiteHelperBase(ServerManager server, ICakeLog log)
        {
            this.Server     = server;
            this.Log        = log;
        }

        public void Delete(string name)
        {
            var site = this.Server.Sites
                .FirstOrDefault(p => p.Name == name);

            if(site == null)
                this.Log.Debug("Site '{0}' not found.", name);
            else
            {
                this.Server.Sites.Remove(site);
                this.Log.Debug("Site '{0}' deleted.", site.Name);
            }
        }

        public void Start(string name)
        {
            var site = this.Server.Sites
                .FirstOrDefault(p => p.Name == name);

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
            }
        }

        public void Stop(string name)
        {
            var site = this.Server.Sites
                .FirstOrDefault(p => p.Name == name);

            if(site != null)
            {
                ObjectState state;
                do
                {
                    this.Log.Verbose("Site '{0}' stopping...", site.Name);
                    state = site.Start();
                }
                while(state != ObjectState.Stopped);

                this.Log.Debug("Site '{0}' stopped.", site.Name);
            }
        }

        public bool Check(string name)
        {
            return this.Server.Sites
                .SingleOrDefault(p => p.Name == name) != null;
        }
    }
}