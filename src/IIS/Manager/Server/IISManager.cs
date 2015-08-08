#region Using Statements
    using System;

    using Microsoft.Web.Administration;
#endregion



namespace Cake.IIS
{
    public sealed class IISManager
    {
        public static ServerManager Connect(string server)
        {
            if (String.IsNullOrEmpty(server))
            {
                return new ServerManager();
            }
            else
            {
                return ServerManager.OpenRemote(server);
            }
        }
    }
}
