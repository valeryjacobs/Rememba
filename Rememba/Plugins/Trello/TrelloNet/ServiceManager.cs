using System;
using RestSharp;

namespace TrelloNet
{
    public static class ServiceManager
    {
        private static RestClient _restClient;

        public static void Init(string token, string key)
        {
            _restClient = new RestClient("https://api.trello.com")
            {
                Authenticator = new OAuth2UriQueryParameterAuthenticator(token)

            };
            _restClient.AddDefaultParameter("key", key);
        }


        public static T Execute<T>(RestRequest request)  where T : new()
        {
            var response = _restClient.Execute<T>(request);
            Console.WriteLine(response.Content);
            return response.Data;
        }
    }
}