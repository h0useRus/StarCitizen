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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            _clientHandler = new HttpClientHandler();
            _clientHandler.UseProxy = Program.Settings.UseHttpProxy;
            Client = new HttpClient(_clientHandler);
            Client.DefaultRequestHeaders.UserAgent.ParseAdd($"{Program.Name}/{Program.Version.ToString(3)}");
            Client.Timeout = TimeSpan.FromMinutes(1);
        }
    }
}
