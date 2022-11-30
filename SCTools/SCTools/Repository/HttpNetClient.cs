using System;
using System.Net;
using System.Net.Http;

namespace NSW.StarCitizen.Tools.Repository
{
    public static class HttpNetClient
    {
        private static readonly HttpClientHandler _clientHandler;
        public static HttpClient Client { get; }
        static HttpNetClient()
        {
            if (!Program.Settings.AllowTls13 || !InitTls13SecuritySupport())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocols.Base;
            }
            _clientHandler = new HttpClientHandler
            {
                UseProxy = Program.Settings.UseHttpProxy,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };
            Client = new HttpClient(_clientHandler);
            Client.DefaultRequestHeaders.UserAgent.ParseAdd($"{Program.Name}/{Program.Version.ToString(3)}");
            Client.Timeout = TimeSpan.FromMinutes(1);
        }

        private static bool InitTls13SecuritySupport()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocols.Base | SecurityProtocols.Tls13;
            }
            catch (NotSupportedException)
            {
                return false;
            }
            return true;
        }

        private static class SecurityProtocols
        {
            //
            // Summary:
            //     Specifies the (SSL) 3.0 and TLS 1.2 security protocols
            public const SecurityProtocolType Base = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;
            //
            // Summary:
            //     Specifies the TLS 1.3 security protocol. The TLS protocol is defined in IETF RFC 8446.
            public const SecurityProtocolType Tls13 = (SecurityProtocolType)12288;
        }
    }
}
