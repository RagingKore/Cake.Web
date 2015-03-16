using Autofac.Extras.FakeItEasy;
using Cake.Core.IO;
using NUnit.Framework;

namespace Cake.Web.Tests
{
    [TestFixture]
    public class WebHelperTests
    {
        public readonly AutoFake AutoFaker = new AutoFake();

        [Test, Ignore]
        public void DeployFtpSite_FullyDeploysSite_WithMinimumSettings()
        {
            // arrange
            var settings = new FtpSiteSettings
            {
                Name                      = "mnp.FUCKOFF.test", 
                PhysicalPath              = @"c:\Deployments\MnpFtp", 
                EnableBasicAuthentication = true,
                Port = 888,
                AuthorizationSettings = new AuthorizationSettings
                {
                    Users = new[] { @"TRUPHONE\MNR-FTP-Dev" },
                    AuthorizationType = AuthorizationType.SpecifiedUser,
                    CanRead = true,
                    CanWrite = true
                }
            };

            AutoFaker.Provide<IFileSystem>(new FileSystem());

            //A.CallTo(() => AutoFaker
            //    .Resolve<IFileSystem>()
            //    .GetDirectory(settings.PhysicalPath).Exists)
            //    .Returns(true);

            var helper = this.AutoFaker.Resolve<WebHelper>();

            // act
            helper.DeployFtpSite(settings);

            // assert
        }

        [Test, Ignore]
        public void DeployWebSite_FullyDeploysSite_WithMinimumSettings()
        {
            // arrange
            var settings = new WebSiteSettings
            {
                Name            = "Truphone MNP Portal",
                HostName = "mnp.kebas.truphone.com",
                PhysicalPath    = @"C:\dev\MobileNumberPorting\Framework\Mnp.Web",
                ApplicationPool = new ApplicationPoolSettings
                {
                    Name = "mnp.kebas.truphone.com",
                    Username = @"NT AUTHORITY\NETWORK SERVICE"           
                }
            };

            AutoFaker.Provide<IFileSystem>(new FileSystem());

            var helper = this.AutoFaker.Resolve<WebHelper>();

            // act
            helper.DeployWebSite(settings);

            // assert
            var kebas = "234234";
        }

        //[Test]
        //public void DeployFtpSite_FullyDeploysSite_WithMinimumSettings()
        //{
        //    // arrange
        //    var settings = new FtpSiteSettings
        //    {
        //        Name = "mnp.ftp.test",
        //        PhysicalPath = @"c:\Deployments\MnpFtp",
        //        EnableBasicAuthentication = true,
        //        //AuthorizationSettings     = new AuthorizationSettings
        //        //{
        //        //    Users                 = new[] { @"TRUPHONE\MNR-FTP-Dev" }, 
        //        //    AuthorizationType     = AuthorizationType.SpecifiedUser, 
        //        //    CanRead               = true, 
        //        //    CanWrite              = true
        //        //}
        //    };

        //    AutoFaker.Provide<IFileSystem>(new FileSystem());

        //    //A.CallTo(() => AutoFaker
        //    //    .Resolve<IFileSystem>()
        //    //    .GetDirectory(settings.PhysicalPath).Exists)
        //    //    .Returns(true);

        //    var helper = this.AutoFaker.Resolve<WebHelper>();

        //    // act
        //    helper.DeployFtpSite(settings);

        //    // assert
        //}
    }
}