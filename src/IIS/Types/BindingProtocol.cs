using System;

namespace Cake.IIS
{
    public class BindingProtocol
    {
        /// <value>
        /// <see cref="BindingProtocol"/> for <c>ftp</c> IIS binding type.
        /// </value>
        public static BindingProtocol Ftp
        {
            get { return new BindingProtocol(Uri.UriSchemeFtp); }
        }

        /// <value>
        /// <see cref="BindingProtocol"/> for <c>http</c> IIS binding type.
        /// </value>
        public static BindingProtocol Http
        {
            get { return new BindingProtocol(Uri.UriSchemeHttp); }
        }

        /// <value>
        /// <see cref="BindingProtocol"/> for <c>https</c> IIS binding type.
        /// </value>
        public static BindingProtocol Https
        {
            get { return new BindingProtocol(Uri.UriSchemeHttps); }
        }

        /// <value>
        /// <see cref="BindingProtocol"/> for <c>net.tcp</c> IIS binding type.
        /// </value>
        public static BindingProtocol NetTcp
        {
            get { return new BindingProtocol(Uri.UriSchemeNetTcp); }
        }

        /// <value>
        /// <see cref="BindingProtocol"/> for <c>net.pipe</c> IIS binding type.
        /// </value>
        public static BindingProtocol NetPipe
        {
            get { return new BindingProtocol(Uri.UriSchemeNetPipe); }
        }

        /// <value>
        /// <see cref="BindingProtocol"/> for <c>net.msmq</c> IIS binding type.
        /// </value>
        public static BindingProtocol NetMsmq
        {
            get { return new BindingProtocol("net.msmq"); }
        }

        /// <summary>
        /// <see cref="BindingProtocol"/> for <c>msmq.formatname</c> IIS binding type.
        /// </summary>
        public static BindingProtocol MsmqFormatName
        {
            get { return new BindingProtocol("msmq.formatname"); }
        }



        private BindingProtocol(string name)
        {
            this.Name = name;
        }



        private string Name { get; set; }



        public override string ToString()
        {
            return this.Name;
        }
    }
}
