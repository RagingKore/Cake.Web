namespace Cake.IIS
{
    public class ApplicationPoolSettings
    {
        #region Fields (1)
            private string _Username;
        #endregion





        #region Constructor (1)
            public ApplicationPoolSettings()
            {
                this.Name                  = "ASP.NET v4.0";
                this.ManagedRuntimeVersion = "v4.0";

                this.IdentityType          = IdentityType.ApplicationPoolIdentity;
                this.ClassicManagedPipelineMode = false;
                this.Enable32BitAppOnWin64 = false;

                this.Autostart = true;
                this.Overwrite = false;
            }
        #endregion





        #region Properties (8)
            public string Name { get; set; }



            public IdentityType IdentityType { get; set; }

            public string Username
            {
                get
                {
                    return _Username;
                }
                set
                {
                    _Username = value;

                    if (!string.IsNullOrEmpty(value))
                    {
                        this.IdentityType = IdentityType.SpecificUser;
                    }
                }
            }

            public string Password { get; set; }



            public string ManagedRuntimeVersion { get; set; }

            public bool ClassicManagedPipelineMode { get; set; }

            public bool Enable32BitAppOnWin64 { get; set; }



            public bool Autostart { get; set; }

            public bool Overwrite { get; set; }
        #endregion
    }
}