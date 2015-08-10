namespace Cake.IIS
{
    public abstract class SiteSettings : BindingSettings
    {
        #region Fields (1)
            private string _PhysicalPath;
        #endregion





        #region Constructor (1)
            public SiteSettings()
                : base()
            {
                this.ServerAutoStart = true;
                this.Overwrite = false;

                this.ApplicationPool = new ApplicationPoolSettings();
            }
        #endregion





        #region Properties (10)
            public string PhysicalPath
            {
                get
                {
                    return _PhysicalPath;
                }

                set
                {
                    _PhysicalPath = value.Replace("/", @"\");
                }
            }



            public bool ServerAutoStart { get; set; }

            public AuthenticationSettings Authentication { get; set; }

            public ApplicationPoolSettings ApplicationPool { get; set; }



            public byte[] CertificateHash { get; set; }

            public string CertificateStoreName { get; set; }


        
            public bool TraceFailedRequestsEnabled { get; set; }

            public string TraceFailedRequestsDirectory { get; set; }

            public long TraceFailedRequestsMaxLogFiles { get; set; }



            public bool Overwrite { get; set; }
        #endregion
    }
}