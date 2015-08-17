#region Using Statements
    using Cake.Core;
    using Cake.Core.Annotations;

    using Microsoft.Web.Administration;
#endregion



namespace Cake.IIS
{
    [CakeAliasCategory("IIS")]
    [CakeNamespaceImport("Microsoft.Web.Administration")]
    public static class WebFarmAliases
    {
        [CakeMethodAlias]
        public static bool WebFarmExists(this ICakeContext context, string name)
        {
            return context.WebFarmExists("", name);
        }

        [CakeMethodAlias]
        public static bool WebFarmExists(this ICakeContext context, string server, string name)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                return WebFarmManager
                        .Using(context.Environment, context.Log, manager)
                        .Exists(name);
            }
        }



        [CakeMethodAlias]
        public static void CreateWebFarm(this ICakeContext context, WebFarmSettings settings)
        {
            context.CreateWebFarm("", settings);
        }

        [CakeMethodAlias]
        public static void CreateWebFarm(this ICakeContext context, string server, WebFarmSettings settings)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                WebFarmManager
                    .Using(context.Environment, context.Log, manager)
                    .Create(settings);
            }
        }



        [CakeMethodAlias]
        public static bool DeleteWebFarm(this ICakeContext context, string name)
        {
            return context.DeleteWebFarm("", name);
        }

        [CakeMethodAlias]
        public static bool DeleteWebFarm(this ICakeContext context, string server, string name)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                return WebFarmManager
                        .Using(context.Environment, context.Log, manager)
                        .Delete(name);
            }
        }



        [CakeMethodAlias]
        public static bool AddServer(this ICakeContext context, string farm, string address)
        {
            return context.AddServer("", farm, address);
        }

        [CakeMethodAlias]
        public static bool AddServer(this ICakeContext context, string server, string farm, string address)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                return WebFarmManager
                        .Using(context.Environment, context.Log, manager)
                        .AddServer(farm, address);
            }
        }



        [CakeMethodAlias]
        public static bool RemoveServer(this ICakeContext context, string farm, string address)
        {
            return context.RemoveServer("", farm, address);
        }

        [CakeMethodAlias]
        public static bool RemoveServer(this ICakeContext context, string server, string farm, string address)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                return WebFarmManager
                        .Using(context.Environment, context.Log, manager)
                        .RemoveServer(farm, address);
            }
        }



        [CakeMethodAlias]
        public static bool ServerExists(this ICakeContext context, string farm, string address)
        {
            return context.ServerExists("", farm, address);
        }

        [CakeMethodAlias]
        public static bool ServerExists(this ICakeContext context, string server, string farm, string address)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                return WebFarmManager
                        .Using(context.Environment, context.Log, manager)
                        .ServerExists(farm, address);
            }
        }



        [CakeMethodAlias]
        public static void SetServerHealthy(this ICakeContext context, string farm, string address)
        {
            context.SetServerHealthy("", farm, address);
        }

        [CakeMethodAlias]
        public static void SetServerHealthy(this ICakeContext context, string server, string farm, string address)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                WebFarmManager
                    .Using(context.Environment, context.Log, manager)
                    .SetServerHealthy(farm, address);
            }
        }



        [CakeMethodAlias]
        public static void SetServerUnhealthy(this ICakeContext context, string farm, string address)
        {
            context.SetServerUnhealthy("", farm, address);
        }

        [CakeMethodAlias]
        public static void SetServerUnhealthy(this ICakeContext context, string server, string farm, string address)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                WebFarmManager
                    .Using(context.Environment, context.Log, manager)
                    .SetServerUnhealthy(farm, address);
            }
        }



        [CakeMethodAlias]
        public static void SetServerAvailable(this ICakeContext context, string farm, string address)
        {
            context.SetServerAvailable("", farm, address);
        }

        [CakeMethodAlias]
        public static void SetServerAvailable(this ICakeContext context, string server, string farm, string address)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                WebFarmManager
                    .Using(context.Environment, context.Log, manager)
                    .SetServerAvailable(farm, address);
            }
        }



        [CakeMethodAlias]
        public static void SetServerUnavailable(this ICakeContext context, string farm, string address)
        {
            context.SetServerUnavailable("", farm, address);
        }

        [CakeMethodAlias]
        public static void SetServerUnavailable(this ICakeContext context, string server, string farm, string address)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                WebFarmManager
                    .Using(context.Environment, context.Log, manager)
                    .SetServerUnavailable(farm, address);
            }
        }



        [CakeMethodAlias]
        public static bool GetServerIsHealthy(this ICakeContext context, string farm, string address)
        {
            return context.GetServerIsHealthy("", farm, address);
        }

        [CakeMethodAlias]
        public static bool GetServerIsHealthy(this ICakeContext context, string server, string farm, string address)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                return WebFarmManager
                        .Using(context.Environment, context.Log, manager)
                        .GetServerIsHealthy(farm, address);
            }
        }



        [CakeMethodAlias]
        public static string GetServerState(this ICakeContext context, string farm, string address)
        {
            return context.GetServerState("", farm, address);
        }

        [CakeMethodAlias]
        public static string GetServerState(this ICakeContext context, string server, string farm, string address)
        {
            using (ServerManager manager = BaseManager.Connect(server))
            {
                return WebFarmManager
                        .Using(context.Environment, context.Log, manager)
                        .GetServerState(farm, address);
            }
        }
    }
}
