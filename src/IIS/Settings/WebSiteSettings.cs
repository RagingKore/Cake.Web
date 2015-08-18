namespace Cake.IIS
{
    public class WebsiteSettings : SiteSettings
    {
        #region Constructor (1)
            public WebsiteSettings()
                : base()
            {
                this.BindingProtocol = BindingProtocol.Http;
                this.Port = 80;
            }
        #endregion
    }
}
