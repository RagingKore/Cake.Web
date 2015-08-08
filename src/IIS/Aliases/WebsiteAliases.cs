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
            using (ServerManager manager = IISManager.Connect(server))
            {
                WebsiteManager
                    .Using(manager, context.Log)
                    .Create(settings);
            }
        }
    }
}
