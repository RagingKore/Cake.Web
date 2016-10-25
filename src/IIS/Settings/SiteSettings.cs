﻿#region Using Statements
    using Cake.Core.IO;
#endregion



namespace Cake.IIS
{
    public abstract class SiteSettings : IDirectorySettings
    {
        #region Constructor (1)
            public SiteSettings()
                : base()
            {
                this.Binding = IISBindings.Http;

                this.ServerAutoStart = true;
                this.Overwrite = false;

                this.ApplicationPool = new ApplicationPoolSettings();
            }
        #endregion





        #region Properties (10)
            public string Name { get; set; }

            public string ComputerName { get; set; }

            public DirectoryPath WorkingDirectory { get; set; }

            public DirectoryPath PhysicalDirectory { get; set; }

            public BindingSettings Binding { get; set; }

            public string AlternateEnabledProtocols { get; set; }

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