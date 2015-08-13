#region Using Statements

using System;
using System.IO;
using Cake.Core;
using Microsoft.Web.Administration;
using NSubstitute;

#endregion

namespace Cake.IIS.Tests.Utils
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

        public static ApplicationPoolSettings GetAppPoolSettings()
        {
            var settings = new ApplicationPoolSettings
            {
                Name = "superman.web",
                Autostart = true,
                MaxProcesses = 1,
                Enable32BitAppOnWin64 = false,
                IdleTimeout = TimeSpan.FromMinutes(20),
                ShutdownTimeLimit = TimeSpan.FromSeconds(90),
                StartupTimeLimit = TimeSpan.FromSeconds(90),
                PingingEnabled = true,
                PingInterval = TimeSpan.FromSeconds(30),
                PingResponseTime = TimeSpan.FromSeconds(90)
            };

            return settings;
        }

        #endregion
    }
}
