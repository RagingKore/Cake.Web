using System;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core;
using Microsoft.Web.Administration;

namespace Cake.Web
{
    public sealed class WebHelper
    {
        private readonly ICakeContext cake;
 
        public WebHelper(ICakeContext cake)
        {
            this.cake = cake;
        }

        public static WebHelper Using(ICakeContext cake)
        {
            return new WebHelper(cake);
        }

        public void DeployWebSite(WebSiteSettings settings, string sourcePath = null)
        {
            if(settings == null) throw new ArgumentNullException("settings");

            // Prepare deployment folder
            if(!this.cake.DirectoryExists(settings.PhysicalPath))
            {
                this.cake.CreateDirectory(settings.PhysicalPath);
                this.cake.Information("Deployment folder created.");
            }

            if(!string.IsNullOrWhiteSpace(sourcePath))
            {
                this.cake.CleanDirectory(settings.PhysicalPath);
                this.cake.Information("Deployment folder cleaned.");

                this.cake.Debug("Copying files to deployment folder...");
                this.cake.CopyFiles(sourcePath + "*.*", settings.PhysicalPath);
                this.cake.Information("Files copied to deployment folder.");
            }

            using(var server = new ServerManager())
            {
                WebSiteHelper
                    .Using(server, this.cake.Log)
                    .Create(settings, true);

                server.CommitChanges();

                this.cake.Information("Web Site '{0}' deployed.", settings.Name);
            }
        }

        public void DeployFtpSite(FtpSiteSettings settings)
        {
            if(settings == null) throw new ArgumentNullException("settings");

            // Prepare deployment folder
            if(!this.cake.DirectoryExists(settings.PhysicalPath))
            {
                this.cake.CreateDirectory(settings.PhysicalPath);
                this.cake.Information("Deployment folder created.");
            }

            using (var server = new ServerManager())
            {
                FtpSiteHelper
                    .Using(server, this.cake.Log)
                    .Create(settings, true);

                server.CommitChanges();

                this.cake.Information("Ftp Site '{0}' deployed.", settings.Name);
            }
        }
    }
}
