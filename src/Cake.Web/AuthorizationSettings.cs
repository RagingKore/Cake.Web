namespace Cake.Web
{
    public class AuthorizationSettings
    {
        public AuthorizationSettings()
        {
            this.CanRead  = true;
            this.CanWrite = true;
        }

        public AuthorizationType AuthorizationType { get; set; }

        public string[] Users { get; set; }

        public string[] Roles { get; set; }

        public bool CanRead { get; set; }

        public bool CanWrite { get; set; }
    }
}