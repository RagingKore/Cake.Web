#region Using Statements
    using Cake.Core;
    using Cake.Core.Annotations;

    using Microsoft.Web.Administration;
#endregion



namespace Cake.IIS
{
    [CakeAliasCategory("IIS")]
    [CakeNamespaceImport("Microsoft.Web.Administration")]
    public static class WebsiteAliases
    {
        [CakeMethodAlias]
        public static void CreateWebsite(this ICakeContext context, WebsiteSettings settings)
        {
            context.CreateWebsite("", settings);
        }

        [CakeMethodAlias]
        public static void CreateWebsite(this ICakeContext context, string server, WebsiteSettings settings)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                settings.ComputerName = server;

                WebsiteManager
                    .Using(context.Environment, context.Log, manager)
                    .Create(settings);
            }
        }
    }
}
