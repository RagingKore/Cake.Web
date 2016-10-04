namespace Cake.IIS
{
    public class WebsiteSettings : SiteSettings
    {
        #region Constructor (1)
            public WebsiteSettings()
                : base()
            {
<<<<<<< HEAD
                Binding = IISBindings.Http;
=======
                this.BindingProtocol = BindingProtocol.Http;
                this.Port = 80;
>>>>>>> origin/master
            }
        #endregion
    }
}
