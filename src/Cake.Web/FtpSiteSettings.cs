namespace Cake.Web
{
    public class FtpSiteSettings : SiteSettings
    {
        public bool EnableAnonymousAuthentication { get; set; }

        public bool EnableBasicAuthentication { get; set; }

        public AuthorizationSettings AuthorizationSettings { get; set; }
    }
}
