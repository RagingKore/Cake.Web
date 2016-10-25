namespace Cake.IIS
{
    public class FtpsiteSettings : SiteSettings
    {
        #region Constructor (1)
            public FtpsiteSettings()
                : base()
            {
                this.Binding = IISBindings.Ftp;
            }
        #endregion
    }
}
