#region Using Statements
    using System;
    using System.IO;
    using System.Collections.Generic;

    using Microsoft.Web.Administration;

    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.Diagnostics;
    using Cake.IIS;

    using NSubstitute;
#endregion



namespace Cake.IIS.Tests
{
    internal static class CakeHelper
    {
        #region Functions (4)
            public static ICakeEnvironment CreateEnvironment()
            {
                var environment = Substitute.For<ICakeEnvironment>();
                environment.WorkingDirectory = Directory.GetCurrentDirectory();

                return environment;
            }



            public static ApplicationPoolManager CreateApplicationPoolManager()
            {
                return new ApplicationPoolManager(new ServerManager(), new DebugLog());
            }

            public static FtpsiteManager CreateFtpsiteManager()
            {
                return new FtpsiteManager(new ServerManager(), new DebugLog());
            }
            public static WebsiteManager CreateWebsiteManager()
            {
                return new WebsiteManager(new ServerManager(), new DebugLog());
            }
        #endregion
    }
}
