#region Using Statements
    using Microsoft.Web.Administration;

    using Cake.Core.Diagnostics;
#endregion



namespace Cake.IIS
{
    public class FtpsiteManager : BaseSiteManager
    {
        #region Constructor (1)
            public FtpsiteManager(ServerManager server, ICakeLog log)
                : base(server, log)
            {

            }
        #endregion





        #region Fucntions (2)
            public static FtpsiteManager Using(ServerManager server, ICakeLog log)
            {
                return new FtpsiteManager(server, log);
            }



            public void Create(FtpsiteSettings settings)
            {
                //Create Site
                bool exists;
                Site site = base.CreateSite(settings, out exists);



                if (!exists)
                {
                    // SSL policy
                    var ssl = site
                        .GetChildElement("ftpServer")
                        .GetChildElement("security")
                        .GetChildElement("ssl");

                    ssl.SetAttributeValue("controlChannelPolicy", "SslAllow");
                    ssl.SetAttributeValue("dataChannelPolicy", "SslAllow");



                    // Host name support
                    var hostNameSupport = this.Server
                        .GetApplicationHostConfiguration()
                        .GetSection("system.ftpServer/serverRuntime")
                        .GetChildElement("hostNameSupport");

                    hostNameSupport.SetAttributeValue("useDomainNameAsHostName", true);

                    this.Log.Information("Ftp Site '{0}' created.", settings.Name);
                }
            }
        #endregion
    }
}