namespace Cake.Web
{
    public class FtpSiteSettings : SiteSettings
    {
        public FtpSiteSettings()
        {
            BindingProtocol               = BindingProtocol.Ftp;
            Port                          = 21;
            AuthorizationSettings         = new AuthorizationSettings();
            EnableAnonymousAuthentication = true;
        }

        public bool EnableAnonymousAuthentication { get; set; }

        public bool EnableBasicAuthentication { get; set; }

        public AuthorizationSettings AuthorizationSettings { get; set; }
    }
}
