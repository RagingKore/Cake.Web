namespace Cake.Web
{
    public class AuthorizationSettings
    {
        public AuthorizationType AuthorizationType { get; set; }

        public string[] Users { get; set; }

        public bool CanRead { get; set; }

        public bool CanWrite { get; set; }
    }
}