namespace Cake.IIS
{
    public class BindingSettings
    {
        #region Constructor (1)
            public BindingSettings()
            {
                this.BindingProtocol = BindingProtocol.Http;

                this.IpAddress       = "*";
                this.Port            = 80;
                this.HostName        = "*";
            }
        #endregion





        #region Properties (6)
            public string Name { get; set; }



            public string IpAddress { get; set; }

            public int Port { get; set; }

            public string HostName { get; set; }



            public BindingProtocol BindingProtocol { get; set; }

            public string BindingInformation
            {
                get
                {
                    return string.Format(@"{0}:{1}:{2}", IpAddress, Port, HostName);
                }
            }
        #endregion
    }
}