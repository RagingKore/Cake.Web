namespace Cake.IIS
{
    public class WebsiteSettings : SiteSettings
    {
        #region Constructor (1)
            public WebsiteSettings()
                : base()
            {
                Binding = IISBindings.Http;
            }
        #endregion
    }
}
