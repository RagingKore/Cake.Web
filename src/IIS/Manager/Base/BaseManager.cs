#region Using Statements
    using System;

    using Cake.Core.Diagnostics;

    using Microsoft.Web.Administration;
#endregion



namespace Cake.IIS
{
    public abstract class BaseManager
    {
        #region Fields (2)
            protected readonly ServerManager Server;
            protected readonly ICakeLog Log;
        #endregion





        #region Constructor (1)
            public BaseManager(ServerManager server, ICakeLog log)
            {
                if (server == null)
                {
                    throw new ArgumentNullException("server");
                }
                if (log == null)
                {
                    throw new ArgumentNullException("log");
                }

                this.Server = server;
                this.Log = log;
            }
        #endregion
    }
}