using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyWay.Passport.Mobile.Services
{
    public class RequestService
    {
        private readonly HttpClient client;

        public RequestService()
        {
            client = new HttpClient();
        }

        public async Task<HttpResponseMessage> SendRequest(
            string url,
            Dictionary<string, string> payload)
        {
            Console.WriteLine($"[{nameof(RequestService)}]: POST {url}");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(payload)
            };

            var response = await client.SendAsync(request);

            Console.WriteLine($"[{nameof(RequestService)}]: Got response {response.StatusCode}");

            return response;
        }

    }
}
