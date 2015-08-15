#region Using Statements
    using Cake.Core;
    using Cake.Core.Annotations;

    using Microsoft.Web.Administration;
#endregion



namespace Cake.IIS
{
    [CakeAliasCategory("IIS")]
    [CakeNamespaceImport("Microsoft.Web.Administration")]
    public static class FtpsiteAliases
    {
        [CakeMethodAlias]
        public static void CreateFtpsite(this ICakeContext context, FtpsiteSettings settings)
        {
            context.CreateFtpsite("", settings);
        }

        [CakeMethodAlias]
        public static void CreateFtpsite(this ICakeContext context, string server, FtpsiteSettings settings)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                settings.ComputerName = server;

                FtpsiteManager
                    .Using(context.Environment, context.Log, manager)
                    .Create(settings);
            }
        }
    }
}
