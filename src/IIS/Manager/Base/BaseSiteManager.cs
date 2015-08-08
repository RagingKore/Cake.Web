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