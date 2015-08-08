namespace Cake.IIS
{
    public class FtpsiteSettings : SiteSettings
    {
        #region Constructor (1)
            public FtpsiteSettings()
                : base()
            {
                this.BindingProtocol               = BindingProtocol.Ftp;
                this.Port                          = 21;

                this.EnableAnonymousAuthentication = false;
                this.EnableBasicAuthentication = true;

                this.AuthorizationSettings = new AuthorizationSettings();
            }
        #endregion





        #region Properties (3)
            public bool EnableAnonymousAuthentication { get; set; }

            public bool EnableBasicAuthentication { get; set; }



            public AuthorizationSettings AuthorizationSettings { get; set; }
        #endregion
    }
}
