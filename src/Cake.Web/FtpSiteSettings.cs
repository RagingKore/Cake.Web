namespace Cake.Web
{
    public class FtpSiteSettings : SiteSettings
    {
        public FtpSiteSettings()
        {
            BindingProtocol       = BindingProtocol.Ftp;
            AuthorizationSettings = new AuthorizationSettings();
            Port                  = 21;
        }

        public bool EnableAnonymousAuthentication { get; set; }

        public bool EnableBasicAuthentication { get; set; }

        public AuthorizationSettings AuthorizationSettings { get; set; }
    }
}
