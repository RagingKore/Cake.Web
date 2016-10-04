namespace Cake.IIS
{
    public class FtpsiteSettings : SiteSettings
    {
        #region Constructor (1)
            public FtpsiteSettings()
                : base()
            {
<<<<<<< HEAD
                this.Binding = IISBindings.Ftp;
=======
                this.BindingProtocol               = BindingProtocol.Ftp;
                this.Port                          = 21;
>>>>>>> origin/master
            }
        #endregion
    }
}
