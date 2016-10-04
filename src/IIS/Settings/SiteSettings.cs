#region Using Statements
    using Cake.Core.IO;
#endregion



namespace Cake.IIS
{
<<<<<<< HEAD
    public abstract class SiteSettings : 
        IDirectorySettings
=======
    public abstract class SiteSettings : BindingSettings, IDirectorySettings
>>>>>>> origin/master
    {
        #region Constructor (1)
            public SiteSettings()
                : base()
            {
<<<<<<< HEAD
                this.Binding = IISBindings.Http;
=======
>>>>>>> origin/master
                this.ServerAutoStart = true;
                this.Overwrite = false;

                this.ApplicationPool = new ApplicationPoolSettings();
            }
        #endregion





        #region Properties (10)
            public string ComputerName { get; set; }

            public DirectoryPath WorkingDirectory { get; set; }

            public DirectoryPath PhysicalDirectory { get; set; }


<<<<<<< HEAD
            public BindingSettings Binding { get; set; }
=======
>>>>>>> origin/master

            public ApplicationPoolSettings ApplicationPool { get; set; }

            public AuthenticationSettings Authentication { get; set; }

            public AuthorizationSettings Authorization { get; set; }



            public bool TraceFailedRequestsEnabled { get; set; }

            public string TraceFailedRequestsDirectory { get; set; }

            public long TraceFailedRequestsMaxLogFiles { get; set; }


        
            public bool ServerAutoStart { get; set; }

            public bool Overwrite { get; set; }
        #endregion
    }
}