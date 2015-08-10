#region Using Statements
    using Cake.Core;
    using Cake.Core.Annotations;

    using Microsoft.Web.Administration;
#endregion



namespace Cake.IIS
{
    [CakeAliasCategory("IIS")]
    [CakeNamespaceImport("Microsoft.Web.Administration")]
    public static class SiteAliases
    {
        [CakeMethodAlias]
        public static bool SiteExists(this ICakeContext context, string name)
        {
            return context.SiteExists("", name);
        }

        [CakeMethodAlias]
        public static bool SiteExists(this ICakeContext context, string server, string name)
        {
            using (ServerManager manager = IISManager.Connect(server))
            {
                return WebsiteManager
                        .Using(manager, context.Log)
                        .Exists(name);
            }
        }



        [CakeMethodAlias]
        public static bool DeleteSite(this ICakeContext context, string name)
        {
            return context.DeleteSite("", name);
        }

        [CakeMethodAlias]
        public static bool DeleteSite(this ICakeContext context, string server, string name)
        {
            using (ServerManager manager = IISManager.Connect(server))
            {
                return WebsiteManager
                        .Using(manager, context.Log)
                        .Delete(name);
            }
        }



        [CakeMethodAlias]
        public static bool StartSite(this ICakeContext context, string name)
        {
            return context.StartSite("", name);
        }

        [CakeMethodAlias]
        public static bool StartSite(this ICakeContext context, string server, string name)
        {
            using (ServerManager manager = IISManager.Connect(server))
            {
                return WebsiteManager
                        .Using(manager, context.Log)
                        .Start(name);
            }
        }



        [CakeMethodAlias]
        public static bool StopSite(this ICakeContext context, string name)
        {
            return context.StopSite("", name);
        }

        [CakeMethodAlias]
        public static bool StopSite(this ICakeContext context, string server, string name)
        {
            using (ServerManager manager = IISManager.Connect(server))
            {
                return WebsiteManager
                        .Using(manager, context.Log)
                        .Stop(name);
            }
        }



        [CakeMethodAlias]
        public static void AddSiteBinding(this ICakeContext context, WebsiteSettings settings)
        {
            context.AddSiteBinding("", settings);
        }

        [CakeMethodAlias]
        public static void AddSiteBinding(this ICakeContext context, string server, WebsiteSettings settings)
        {
            using (ServerManager manager = IISManager.Connect(server))
            {
                WebsiteManager
                    .Using(manager, context.Log)
                    .AddBinding(settings);
            }
        }



        [CakeMethodAlias]
        public static void RemoveSiteBinding(this ICakeContext context, WebsiteSettings settings)
        {
            context.RemoveSiteBinding("", settings);
        }

        [CakeMethodAlias]
        public static void RemoveSiteBinding(this ICakeContext context, string server, WebsiteSettings settings)
        {
            using (ServerManager manager = IISManager.Connect(server))
            {
                WebsiteManager
                    .Using(manager, context.Log)
                    .AddBinding(settings);
            }
        }



        [CakeMethodAlias]
        public static void AddSiteApplication(this ICakeContext context, ApplicationSettings settings)
        {
            context.AddSiteApplication("", settings);
        }

        [CakeMethodAlias]
        public static void AddSiteApplication(this ICakeContext context, string server, ApplicationSettings settings)
        {
            using (ServerManager manager = IISManager.Connect(server))
            {
                WebsiteManager
                    .Using(manager, context.Log)
                    .AddApplication(settings);
            }
        }



        [CakeMethodAlias]
        public static void RemoveSiteApplication(this ICakeContext context, ApplicationSettings settings)
        {
            context.RemoveSiteApplication("", settings);
        }

        [CakeMethodAlias]
        public static void RemoveSiteApplication(this ICakeContext context, string server, ApplicationSettings settings)
        {
            using (ServerManager manager = IISManager.Connect(server))
            {
                WebsiteManager
                    .Using(manager, context.Log)
                    .AddApplication(settings);
            }
        }
    }
}
