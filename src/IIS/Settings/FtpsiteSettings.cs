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
            }
        #endregion
    }
}
