
using InstagramClient;

using InstaSharp;
using InstaSharp.Endpoints;

using System;
using System.Collections.Generic;

namespace TestConsole
{
    public class Program
    {

        static  void Main(string[] args)
        {
            string username = "user";
            string password = "password";

            const string clientId = "1234567890abcdef1234567890abcdef";
            const string clientSecret = "1234567890abcdef1234567890abcdef";
            const string redirectUri = "http://localhost/";

            var config = new InstagramConfig(clientId, clientSecret, redirectUri);
            var scopes = new List<OAuth.Scope>()
{
    OAuth.Scope.Basic
};

            var auth = Instagram.AuthByCredentials(username, password, config, scopes);

            var users = new Users(config, auth);
          //  var userFeed = await users.Feed(null, null, null);

        }
    }

}

