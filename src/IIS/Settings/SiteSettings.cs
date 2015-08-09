namespace Cake.IIS
{
    public abstract class SiteSettings
    {
        #region Fields (1)
            private string _PhysicalPath;
        #endregion





        #region Constructor (1)
            public SiteSettings()
            {
                this.BindingProtocol = BindingProtocol.Http;
                this.IpAddress       = "*";
                this.Port            = 80;
                this.HostName        = "*";

                this.ServerAutoStart = true;
                this.Overwrite = false;

                this.ApplicationPool = new ApplicationPoolSettings();
            }
        #endregion





        #region Properties (10)
            public string Name { get; set; }

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



            public string IpAddress { get; set; }

            public int Port { get; set; }

            public string HostName { get; set; }



            public bool ServerAutoStart { get; set; }

            public AuthenticationSettings Authentication { get; set; }

            public ApplicationPoolSettings ApplicationPool { get; set; }



            public BindingProtocol BindingProtocol
            {
                get; set;
            }

            public string BindingInformation
            {
                get
                {
                    return string.Format(@"{0}:{1}:{2}", IpAddress, Port, HostName);
                }
            }



            public byte[] CertificateHash { get; set; }

            public string CertificateStoreName { get; set; }


        
            public bool TraceFailedRequestsEnabled { get; set; }

            public string TraceFailedRequestsDirectory { get; set; }

            public long TraceFailedRequestsMaxLogFiles { get; set; }



            public bool Overwrite { get; set; }
        #endregion
    }
}