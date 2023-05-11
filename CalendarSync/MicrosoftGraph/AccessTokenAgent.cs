using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CalendarSync.MicrosoftGraph
{
    public static class AccessTokenAgent
    {
        public static AccessToken Refresh(string refreshToken)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.RequestUri = new Uri("https://login.microsoftonline.com/common/oauth2/v2.0/token");
            httpRequestMessage.Method = HttpMethod.Post;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").AddUserSecrets<Program>(true).Build();
            keyValuePairs.Add("client_id", configuration.GetValue<string>("ClientId"));
            keyValuePairs.Add("grant_type", "refresh_token");
            keyValuePairs.Add("scope", "user.read offline_access");
            keyValuePairs.Add("refresh_token", refreshToken);
            httpRequestMessage.Content = new FormUrlEncodedContent(keyValuePairs);
            HttpResponseMessage httpResponseMessage = httpClient.SendAsync(httpRequestMessage).Result;

            if(httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<AccessToken>(result!);
            }

            else
            {
                AccessToken accessToken = new AccessToken();
                accessToken.Error = true;
                return accessToken;
            }
        }

        public static AccessTokenStatus Status(string accessToken)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.RequestUri = new Uri("https://graph.microsoft.com/v1.0/me");
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + accessToken);
            HttpResponseMessage httpResponseMessage = httpClient.SendAsync(httpRequestMessage).Result;
            string result = httpResponseMessage.Content.ReadAsStringAsync().Result;

            if(httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                return AccessTokenStatus.Valid;
            }

            else if(httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized && result.Contains("80049228"))
            {
                return AccessTokenStatus.Expired;
            }

            else
            {
                return AccessTokenStatus.Error;
            }
        }
    }
}
