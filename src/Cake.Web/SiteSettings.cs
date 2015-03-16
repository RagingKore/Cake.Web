namespace Cake.Web
{
    public class SiteSettings
    {
        private string physicalPath;

        public SiteSettings()
        {
            // defaults
            BindingProtocol = BindingProtocol.Http;
            IpAddress       = "*";
            Port            = 80;
            HostName        = "*";
            ServerAutoStart = true;
            ApplicationPool = new ApplicationPoolSettings();
            Authentication  = new AuthenticationSettings();
        }

        public string Name { get; set; }

        public BindingProtocol BindingProtocol { get; set; }

        public string PhysicalPath
        {
            get
            {
                return this.physicalPath;
            }

            set
            {
                this.physicalPath = value.Replace("/", @"\");
            }
        }

        public string IpAddress { get; set; }

        public int Port { get; set; }

        public string HostName { get; set; }

        public bool ServerAutoStart { get; set; }

        public ApplicationPoolSettings ApplicationPool { get; set; }

        public string BindingInformation { get { return string.Format(@"{0}:{1}:{2}", IpAddress, Port, HostName); } }

        public AuthenticationSettings Authentication { get; set; }    
    }

    public class AuthenticationSettings
    {
        public AuthenticationSettings()
        {
            EnableAnonymousAuthentication = true;
        }

        public bool EnableBasicAuthentication { get; set; }

        public bool EnableWindowsAuthentication { get; set; }

        public bool EnableAnonymousAuthentication { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}