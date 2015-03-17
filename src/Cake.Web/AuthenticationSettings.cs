namespace Cake.Web
{
    public class AuthenticationSettings
    {
        public bool EnableBasicAuthentication { get; set; }

        public bool EnableWindowsAuthentication { get; set; }

        public bool EnableAnonymousAuthentication { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}