#region Using Statements
    using Microsoft.Web.Administration;
#endregion



namespace Cake.IIS
{
    public static class FtpSiteExtensions
    {
        public static void SetAnonymousAuthentication(this Site site, bool anonymousAuthenticationEnabled)
        {
            var authenticationElement = site
                .GetChildElement("ftpServer")
                .GetChildElement("security")
                .GetChildElement("authentication")
                .GetChildElement("anonymousAuthentication");

            authenticationElement.SetAttributeValue("enabled", anonymousAuthenticationEnabled);
        }

        public static void SetBasicAuthentication(this Site site, bool basicAuthenticationEnabled)
        {
            var authenticationElement = site
                .GetChildElement("ftpServer")
                .GetChildElement("security")
                .GetChildElement("authentication")
                .GetChildElement("basicAuthentication");

            authenticationElement.SetAttributeValue("enabled", basicAuthenticationEnabled);
        }
    }
}