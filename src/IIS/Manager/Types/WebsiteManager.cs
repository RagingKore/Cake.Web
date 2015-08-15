#region Using Statements
    using Microsoft.Web.Administration;

    using Cake.Core;
    using Cake.Core.Diagnostics;
#endregion



namespace Cake.IIS
{
    public class WebsiteManager : BaseSiteManager
    {
        #region Constructor (1)
            public WebsiteManager(ICakeEnvironment environment, ICakeLog log)
                : base(environment, log)
            {

            }
        #endregion





        #region Functions (2)
            public static WebsiteManager Using(ICakeEnvironment environment, ICakeLog log, ServerManager server)
            {
                WebsiteManager manager = new WebsiteManager(environment, log);

                manager.SetServer(server);

                return manager;
            }



            public void Create(WebsiteSettings settings)
            {
                bool exists;
                Site site = base.CreateSite(settings, out exists);
                
                if (!exists)
                {
                    _Server.CommitChanges();
                    _Log.Information("Web Site '{0}' created.", settings.Name);
                }
            }
        #endregion
    }
}