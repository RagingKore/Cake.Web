#region Using Statements
    using System;

    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.Diagnostics;

    using Microsoft.Web.Administration;
#endregion



namespace Cake.IIS
{
    public abstract class BaseManager
    {
        #region Fields (3)
            protected readonly ICakeEnvironment _Environment;
            protected readonly ICakeLog _Log;

            protected ServerManager _Server;
        #endregion





        #region Constructor (1)
            public BaseManager(ICakeEnvironment environment, ICakeLog log)
            {
                if (environment == null)
                {
                    throw new ArgumentNullException("environment");
                }
                if (log == null)
                {
                    throw new ArgumentNullException("log");
                }

                _Environment = environment;
                _Log = log;
            }
        #endregion





        #region Constructor (3)
            public static ServerManager Connect(string server)
            {
                if (String.IsNullOrEmpty(server))
                {
                    return new ServerManager();
                }
                else
                {
                    return ServerManager.OpenRemote(server);
                }
            }



            public void SetServer()
            {
                this.SetServer(BaseManager.Connect(""));
            }

            public void SetServer(string server)
            {
                this.SetServer(BaseManager.Connect(server));
            }

            public void SetServer(ServerManager manager)
            {
                if (manager == null)
                {
                    throw new ArgumentNullException("manager");
                }

                _Server = manager;
            }



            protected string GetPhysicalDirectory(IDirectorySettings settings)
            {
                if (String.IsNullOrEmpty(settings.ComputerName))
                {
                    DirectoryPath workingDirectory = settings.WorkingDirectory ?? _Environment.WorkingDirectory;

                    settings.WorkingDirectory = workingDirectory.MakeAbsolute(_Environment);
                }
                else if (settings.WorkingDirectory == null)
                {
                    settings.WorkingDirectory = new DirectoryPath("C:/");
                }

                var path = settings.PhysicalDirectory.MakeAbsolute(settings.WorkingDirectory).FullPath;
                return path.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
            }
        #endregion
    }
}