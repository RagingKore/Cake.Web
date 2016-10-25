using Cake.IIS.Bindings;


namespace Cake.IIS
{
    public static class IISBindings
    {
        /// <summary>
        /// Creates http binding (port: 80)
        /// </summary>
        public static BindingSettings Http
        {
            get
            {
                return new BindingSettings(BindingProtocol.Http)
                {
                    HostName = "*",
                    IpAddress = "*",
                    Port = 80
                };
            }
        }

        /// <summary>
        /// Creates https binding (port: 443)
        /// </summary>
        public static BindingSettings Https
        {
            get
            {
                return new BindingSettings(BindingProtocol.Https)
                {
                    HostName = "*",
                    IpAddress = "*",
                    Port = 443
                };
            }
        }



        /// <summary>
        /// Creates ftp binding (port: 21)
        /// </summary>
        public static BindingSettings Ftp
        {
            get
            {
                return new PortBindingSettings(BindingProtocol.Ftp)
                {
                    Port = 21
                };
            }
        }

        /// <summary>
        /// Creates net.tcp binding (port: 808)
        /// </summary>
        public static BindingSettings NetTcp
        {
            get
            {
                return new PortBindingSettings(BindingProtocol.NetTcp)
                {
                    Port = 808,
                    HostName = "*"
                };
            }
        }

        /// <summary>
        /// Creates net.pipe binding
        /// </summary>
        public static BindingSettings NetPipe
        {
            get
            {
                return new HostBindingSettings(BindingProtocol.NetPipe)
                {
                    HostName = "*"
                };
            }
        }

        /// <summary>
        /// Creates net.msmq binding.
        /// </summary>
        public static BindingSettings NetMsmq
        {
            get
            {
                return new HostBindingSettings(BindingProtocol.NetMsmq)
                {
                    HostName = "localhost"
                };
            }
        }

        /// <summary>
        /// Creates msmq.formatname binding
        /// </summary>
        public static BindingSettings MsmqFormatName
        {
            get
            {
                return new BindingSettings(BindingProtocol.MsmqFormatName)
                {
                    HostName = "localhost"
                };
            }
        }
    }
}