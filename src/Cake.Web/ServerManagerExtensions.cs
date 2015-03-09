using System.Collections.Generic;
using Microsoft.Web.Administration;

namespace Cake.Web
{
    public static class ServerManagerExtensions
    {
        public static void SetAuthorization(this ServerManager serverManager, string siteName, AuthorizationSettings authorizationSettings)
        {
            var config = serverManager.GetApplicationHostConfiguration();
            var authorizationElement = config.GetSection(
                "system.ftpServer/security/authorization",
                siteName);
            var authCollection = authorizationElement.GetCollection();

            var addElement = authCollection.CreateElement("add");
            addElement.SetAttributeValue("accessType", "Allow");
            if (authorizationSettings.AuthorizationType == AuthorizationType.AllUsers)
                addElement.SetAttributeValue("users", "*");
            if (authorizationSettings.AuthorizationType == AuthorizationType.SpecifiedUser)
                addElement.SetAttributeValue("users", string.Join(", ", authorizationSettings.Users));
            if (authorizationSettings.AuthorizationType == AuthorizationType.SpecifiedRoleOrUserGroup)
                addElement.SetAttributeValue("roles", string.Join(", ", authorizationSettings.Users));

            var permissions = new List<string>();
            if (authorizationSettings.CanRead)
                permissions.Add("Read");
            if (authorizationSettings.CanWrite)
                permissions.Add("Write");
            addElement.SetAttributeValue("permissions", string.Join(", ", permissions));

            authCollection.Clear();
            authCollection.Add(addElement);
        }
    }
}
