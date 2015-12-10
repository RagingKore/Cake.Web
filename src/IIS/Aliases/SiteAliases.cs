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
            using (ServerManager manager = BaseManager.Connect(server))
            {
                return WebsiteManager
                        .Using(context.Environment, context.Log, manager)
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
            using (ServerManager manager = BaseManager.Connect(server))
            {
                return WebsiteManager
                        .Using(context.Environment, context.Log, manager)
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
            using (ServerManager manager = BaseManager.Connect(server))
            {
                return WebsiteManager
                        .Using(context.Environment, context.Log, manager)
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
            using (ServerManager manager = BaseManager.Connect(server))
            {
                return WebsiteManager
                        .Using(context.Environment, context.Log, manager)
                        .Stop(name);
            }
        }



        [CakeMethodAlias]
        public static bool RestartSite(this ICakeContext context, string name)
        {
            return context.RestartSite("", name);
        }

        [CakeMethodAlias]
        public static bool RestartSite(this ICakeContext context, string server, string name)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                WebsiteManager webManager = WebsiteManager.Using(context.Environment, context.Log, manager);

                if (webManager.Stop(name))
                {
                    return webManager.Start(name);
                }
                else
                {
                    return false;
                }
            }
        }



        [CakeMethodAlias]
        public static void AddSiteBinding(this ICakeContext context, BindingSettings settings)
        {
            context.AddSiteBinding("", settings);
        }

        [CakeMethodAlias]
        public static void AddSiteBinding(this ICakeContext context, string server, BindingSettings settings)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                WebsiteManager
                    .Using(context.Environment, context.Log, manager)
                    .AddBinding(settings);
            }
        }



        [CakeMethodAlias]
        public static void RemoveSiteBinding(this ICakeContext context, BindingSettings settings)
        {
            context.RemoveSiteBinding("", settings);
        }

        [CakeMethodAlias]
        public static void RemoveSiteBinding(this ICakeContext context, string server, BindingSettings settings)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                WebsiteManager
                    .Using(context.Environment, context.Log, manager)
                    .RemoveBinding(settings);
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
            using (ServerManager manager = BaseManager.Connect(server))
            {
                settings.ComputerName = server;

                WebsiteManager
                    .Using(context.Environment, context.Log, manager)
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
            using (ServerManager manager = BaseManager.Connect(server))
            {
                WebsiteManager
                    .Using(context.Environment, context.Log, manager)
                    .RemoveApplication(settings);
            }
        }
    }
}
