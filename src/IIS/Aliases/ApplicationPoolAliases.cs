#region Using Statements
    using Cake.Core;
    using Cake.Core.Annotations;

    using Microsoft.Web.Administration;
#endregion



namespace Cake.IIS
{
    [CakeAliasCategory("IIS")]
    [CakeNamespaceImport("Microsoft.Web.Administration")]
    public static class ApplicationPoolAliases
    {
        [CakeMethodAlias]
        public static bool PoolExists(this ICakeContext context, string name)
        {
            return context.PoolExists("", name);
        }

        [CakeMethodAlias]
        public static bool PoolExists(this ICakeContext context, string server, string name)
        {
            using (ServerManager manager = IISManager.Connect(server))
            {
                return ApplicationPoolManager
                        .Using(manager, context.Log)
                        .Exists(name);
            }
        }



        [CakeMethodAlias]
        public static void CreatePool(this ICakeContext context, ApplicationPoolSettings settings)
        {
            context.CreatePool("", settings);
        }

        [CakeMethodAlias]
        public static void CreatePool(this ICakeContext context, string server, ApplicationPoolSettings settings)
        {
            using (ServerManager manager = IISManager.Connect(server))
            {
                ApplicationPoolManager
                    .Using(manager, context.Log)
                    .Create(settings);
            }
        }



        [CakeMethodAlias]
        public static bool DeletePool(this ICakeContext context, string name)
        {
            return context.DeletePool("", name);
        }

        [CakeMethodAlias]
        public static bool DeletePool(this ICakeContext context, string server, string name)
        {
            using (ServerManager manager = IISManager.Connect(server))
            {
                return ApplicationPoolManager
                        .Using(manager, context.Log)
                        .Delete(name);
            }
        }



        [CakeMethodAlias]
        public static bool StartPool(this ICakeContext context, string name)
        {
            return context.StartPool("", name);
        }

        [CakeMethodAlias]
        public static bool StartPool(this ICakeContext context, string server, string name)
        {
            using (ServerManager manager = IISManager.Connect(server))
            {
                return ApplicationPoolManager
                        .Using(manager, context.Log)
                        .Start(name);
            }
        }



        [CakeMethodAlias]
        public static bool StopPool(this ICakeContext context, string name)
        {
            return context.StopPool("", name);
        }

        [CakeMethodAlias]
        public static bool StopPool(this ICakeContext context, string server, string name)
        {
            using (ServerManager manager = IISManager.Connect(server))
            {
                return ApplicationPoolManager
                        .Using(manager, context.Log)
                        .Stop(name);
            }
        }



        [CakeMethodAlias]
        public static bool RecyclePool(this ICakeContext context, string name)
        {
            return context.RecyclePool("", name);
        }

        [CakeMethodAlias]
        public static bool RecyclePool(this ICakeContext context, string server, string name)
        {
            using (ServerManager manager = IISManager.Connect(server))
            {
                return ApplicationPoolManager
                        .Using(manager, context.Log)
                        .Recycle(name);
            }
        }
    }
}
