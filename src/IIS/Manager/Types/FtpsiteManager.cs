#region Using Statements
    using Microsoft.Web.Administration;

    using Cake.Core;
    using Cake.Core.Diagnostics;
#endregion



namespace Cake.IIS
{
    /// <summary>
    /// Class for managing FTP sites
    /// </summary>
    public class FtpsiteManager : BaseSiteManager
    {
        #region Constructor (1)
            /// <summary>
            /// Initializes a new instance of the <see cref="FtpsiteManager" /> class.
            /// </summary>
            /// <param name="environment">The environment.</param>
            /// <param name="log">The log.</param>
            public FtpsiteManager(ICakeEnvironment environment, ICakeLog log)
                : base(environment, log)
            {

            }
        #endregion





        #region Fucntions (2)
            public static FtpsiteManager Using(ICakeEnvironment environment, ICakeLog log, ServerManager server)
            {
                FtpsiteManager manager = new FtpsiteManager(environment, log);

                manager.SetServer(server);

                return manager;
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
                    var hostNameSupport = _Server
                        .GetApplicationHostConfiguration()
                        .GetSection("system.ftpServer/serverRuntime")
                        .GetChildElement("hostNameSupport");

                    hostNameSupport.SetAttributeValue("useDomainNameAsHostName", true);

                    _Server.CommitChanges();

                    _Log.Information("Ftp Site '{0}' created.", settings.Name);
                }
            }
        #endregion
    }
}