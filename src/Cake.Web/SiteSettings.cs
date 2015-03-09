namespace Cake.Web
{
    public class SiteSettings
    {
        public SiteSettings()
        {
            // defaults
            BindingProtocol = BindingProtocol.Http;
            IpAddress       = "*";
            Port            = 80;
            Host            = "*";
            ServerAutoStart = true;
        }

        public string Name { get; set; }

        public BindingProtocol BindingProtocol { get; set; }

        public string PhysicalPath { get; set; }

        public string IpAddress { get; set; }

        public int Port { get; set; }

        public string Host { get; set; }

        public bool ServerAutoStart { get; set; }

        public ApplicationPoolSettings ApplicationPool { get; set; }

        public string BindingInformation { get { return string.Format(@"{0}:{1}:{2}", IpAddress, Port, Host); } }
    }
}