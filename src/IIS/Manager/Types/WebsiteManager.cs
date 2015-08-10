#region Using Statements
    using System;
    using System.Linq;

    using Microsoft.Web.Administration;

    using Cake.Core.Diagnostics;
#endregion



namespace Cake.IIS
{
    public class WebsiteManager : BaseSiteManager
    {
        #region Constructor (1)
            public WebsiteManager(ServerManager server, ICakeLog log)
                : base(server, log)
            {

            }
        #endregion





        #region Functions (2)
            public static WebsiteManager Using(ServerManager server, ICakeLog log)
            {
                return new WebsiteManager(server, log);
            }



            public void Create(WebsiteSettings settings)
            {
                bool exists;
                Site site = base.CreateSite(settings, out exists);

                if (!exists)
                {
                    this.Log.Information("Web Site '{0}' created.", settings.Name);
                }
            }
        #endregion
    }
}