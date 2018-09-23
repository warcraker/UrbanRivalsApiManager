using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

using ExtApi.Engine;
using OAuth;

namespace UrbanRivalsApiManager
{
    /// <summary>
    /// Manages authentication through OAuth with Urban Rivals and allows to send requests.
    /// </summary>
    /// <remarks> The OAuth protocol (Consumer > Request > AuthorizeRequest > Access) must be followed in order. Allows the use of a valid access token to avoid session re-authentication.</remarks>
    public class ApiManager
    {
        private static class ApiURLs
        {
            public static readonly string RequestToken = @"http://www.urban-rivals.com/api/auth/request_token.php";
            public static readonly string AuthorizeToken = @"http://www.urban-rivals.com/api/auth/authorize.php";
            public static readonly string AccessToken = @"http://www.urban-rivals.com/api/auth/access_token.php";
            public static readonly string Server = @"http://www.urban-rivals.com/api/";
            public static readonly string Callback = @"http://www.urban-rivals.com/";
        }

        /// <summary>
        /// Extracts tokens from the string sent by the server.
        /// </summary>
        private static readonly Regex TokensFromServerResponseRegex = new Regex(@"oauth_token=(?<token>[0-9a-f]+)&oauth_token_secret=(?<token_secret>[0-9a-f]+)");

        /// <summary>
        /// Used on the authorization part of the protocol.
        /// </summary>
        private OAuthBase OAuthBase;
        /// <summary>
        /// Used on the API calls.
        /// </summary>
        private ApiRunner ApiRunner;

        /// <summary>
        /// Stores the last consumer token used.
        /// </summary>
        private string[] ConsumerToken = new string[2];
        /// <summary>
        /// Stores the last request token used.
        /// </summary>
        private string[] RequestToken = new string[2];
        /// <summary>
        /// Stores the last access token used.
        /// </summary>
        private string[] AccessToken = new string[2];

        /// <summary>
        /// Creates a new ApiManager with the specified consumer, access and request tokens.
        /// </summary>
        /// <param name="consumerKey">Consumer token public part.</param>
        /// <param name="consumerSecret">Consumer token secret part.</param>
        /// <param name="accessTokenKey">Access token public part.</param>
        /// <param name="accessTokenSecret">Access token secret part.</param>
        /// <param name="requestTokenKey">Request token public part.</param>
        /// <param name="requestTokenSecret">Request token secret part.</param>
        /// <exception cref="ArgumentNullException"><paramref name="consumerKey"/> or <paramref name="consumerSecret"/> are null, empty or whitespace</exception>
        public ApiManager(string consumerKey, string consumerSecret, string accessTokenKey, string accessTokenSecret, string requestTokenKey, string requestTokenSecret)
        {
            if (String.IsNullOrWhiteSpace(consumerKey))
                throw new ArgumentNullException("consumerKey");
            if (String.IsNullOrWhiteSpace(consumerSecret))
                throw new ArgumentNullException("consumerSecret");

            ApiRunner = new ApiRunner();
            OAuthBase = new OAuthBase();

            ConsumerToken[0] = consumerKey;
            ConsumerToken[1] = consumerSecret;
            AccessToken[0] = accessTokenKey;
            AccessToken[1] = accessTokenSecret;
            RequestToken[0] = requestTokenKey;
            RequestToken[1] = requestTokenSecret;
        }
        /// <summary>
        /// Creates a new ApiManager with the specified consumer and access tokens.
        /// </summary>
        /// <param name="consumerKey">Consumer token public part.</param>
        /// <param name="consumerSecret">Consumer token secret part.</param>
        /// <param name="accessTokenKey">Access token public part.</param>
        /// <param name="accessTokenSecret">Access token secret part.</param>
        /// <exception cref="ArgumentNullException"><paramref name="consumerKey"/> or <paramref name="consumerSecret"/> are null, empty or whitespace</exception>
        public ApiManager(string consumerKey, string consumerSecret, string accessTokenKey, string accessTokenSecret)
            : this (consumerKey, consumerSecret, accessTokenKey, accessTokenSecret, "", "")
        {
            if (String.IsNullOrWhiteSpace(consumerKey))
                throw new ArgumentNullException("consumerKey");
            if (String.IsNullOrWhiteSpace(consumerSecret))
                throw new ArgumentNullException("consumerSecret");

            ApiRunner = new ApiRunner();
            OAuthBase = new OAuthBase();

            ConsumerToken[0] = consumerKey;
            ConsumerToken[1] = consumerSecret;
            AccessToken[0] = accessTokenKey;
            AccessToken[1] = accessTokenSecret;
        }
        /// <summary>
        /// Creates a new ApiManager with the specified consumer token.
        /// </summary>
        /// <param name="consumerKey">Consumer token public part.</param>
        /// <param name="consumerSecret">Consumer token secret part.</param>
        /// <exception cref="ArgumentNullException"><paramref name="consumerKey"/> or <paramref name="consumerSecret"/> are null, empty or whitespace</exception>
        public ApiManager(string consumerKey, string consumerSecret)
            : this (consumerKey, consumerSecret, "", "", "", "") { }
        private ApiManager() { }

        /// <summary>
        /// Uses the stored consumer token to get from the server a request token that will be stored internally.
        /// </summary>
        /// <returns>Success or failure reason of the call. If it is different than <see cref="HttpStatusCode.OK"/> it means that the call failed.</returns>
        public HttpStatusCode GetRequestToken()
        {
            string dummy;
            return GetRequestToken(out dummy, out dummy);
        }
        /// <summary>
        /// Uses the stored consumer token to get from the server a request token that will be stored internally.
        /// </summary>
        /// <param name="requestTokenKey">Request token public part.</param>
        /// <param name="requestTokenSecret">Request token secret part.</param>
        /// <returns>Success or failure reason of the call. If it is different than <see cref="HttpStatusCode.OK"/> it means that the call failed.</returns>
        public HttpStatusCode GetRequestToken(out string requestTokenKey, out string requestTokenSecret)
        {
            requestTokenKey = "";
            requestTokenSecret = "";

            var callResult = ApiRunner.ExecuteOAuthApiCall(
                QueryType.GetRequestToken, ApiURLs.RequestToken, new List<ApiParameter>(), RequestMethod.Post, ConsumerToken[0], ConsumerToken[1], "", "");

            if (callResult.StatusCode != HttpStatusCode.OK)
                return callResult.StatusCode;

            var responseBytes = new byte[callResult.ResponseStream.Length];
            callResult.ResponseStream.Read(responseBytes, 0, (int)callResult.ResponseStream.Length);
            string responseString = System.Text.Encoding.UTF8.GetString(responseBytes);
            var match = TokensFromServerResponseRegex.Match(responseString);
            RequestToken[0] = match.Groups["token"].Value;
            RequestToken[1] = match.Groups["token_secret"].Value;
            requestTokenKey = RequestToken[0];
            requestTokenSecret = RequestToken[1];

            return callResult.StatusCode;
        }
        /// <summary>
        /// Uses the default system web browser to request the user to authorize the stored request token.
        /// </summary>
        /// <remarks>The user must be logged into Urban Rivals before calling this.</remarks>
        /// <exception cref="InvalidOperationException">There is not a request token to authorize</exception>
        public void AuthorizeRequestToken()
        {
            if (String.IsNullOrWhiteSpace(RequestToken[0]) || String.IsNullOrWhiteSpace(RequestToken[0]))
                throw new InvalidOperationException("There is not a request token to authorize");

            AuthorizeRequestToken(RequestToken[0], RequestToken[1]);
        }
        /// <summary>
        /// Uses the default system web browser to request the user to authorize the request token.
        /// </summary>
        /// <param name="requestTokenKey">Request token public part.</param>
        /// <param name="requestTokenSecret">Request token secret part.</param>
        /// <remarks>The user must be logged into Urban Rivals before calling this.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="requestTokenKey"/> or <paramref name="requestTokenSecret"/> are null, empty or whitespace</exception>
        public void AuthorizeRequestToken(string requestTokenKey, string requestTokenSecret)
        {
            if (String.IsNullOrWhiteSpace(requestTokenKey))
                throw new ArgumentNullException("requestTokenKey");
            if (String.IsNullOrWhiteSpace(requestTokenSecret))
                throw new ArgumentNullException("requestTokenSecret");

            string URL = GetAuthorizeRequestTokenURL(requestTokenKey, requestTokenSecret);

            System.Diagnostics.Process.Start(URL);
        }
        /// <summary>
        /// Creates a valid URL to request the user to authorize the stored request token.
        /// </summary>
        /// <remarks>The user must be logged into Urban Rivals before using the URL.</remarks>
        /// <exception cref="InvalidOperationException">There is not a request token to authorize</exception>
        public string GetAuthorizeRequestTokenURL()
        {
            if (String.IsNullOrWhiteSpace(RequestToken[0]) || String.IsNullOrWhiteSpace(RequestToken[0]))
                throw new InvalidOperationException("There is not a request token to authorize");

            return GetAuthorizeRequestTokenURL(RequestToken[0], RequestToken[1]);
        }
        /// <summary>
        /// Creates a valid URL to request the user to authorize the request token.
        /// </summary>
        /// <param name="requestTokenKey">Request token public part.</param>
        /// <param name="requestTokenSecret">Request token secret part.</param>
        /// <remarks>The user must be logged into Urban Rivals before using the URL.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="requestTokenKey"/> or <paramref name="requestTokenSecret"/> are null, empty or whitespace</exception>
        public string GetAuthorizeRequestTokenURL(string requestTokenKey, string requestTokenSecret)
        {
            if (String.IsNullOrWhiteSpace(requestTokenKey))
                throw new ArgumentNullException("requestTokenKey");
            if (String.IsNullOrWhiteSpace(requestTokenSecret))
                throw new ArgumentNullException("requestTokenSecret");

            string timestamp = OAuthBase.GenerateTimeStamp();
            string nonce = OAuthBase.GenerateNonce();
            string normalizedUrl, normalizedRequestParameters;
            string signature = OAuthBase.GenerateSignature(
                new Uri(ApiURLs.AuthorizeToken),
                ConsumerToken[0], ConsumerToken[1], requestTokenKey, requestTokenSecret,
                "POST", timestamp, nonce, out normalizedUrl, out normalizedRequestParameters);

            return normalizedUrl + "?" 
                + normalizedRequestParameters
                + "&oauth_signature=" + signature
                + "&oauth_callback=" + ApiURLs.Callback;
        }
        /// <summary>
        /// Uses the stored request token to get from the server an authorized token that will be stored internally.
        /// </summary>
        /// <returns>Success or failure reason of the call. If it is different than <see cref="HttpStatusCode.OK"/> it means that the call failed.</returns>
        /// <exception cref="InvalidOperationException">There is not a request token to authorize</exception>
        public HttpStatusCode GetAccessToken()
        {
            if (String.IsNullOrWhiteSpace(RequestToken[0]) || String.IsNullOrWhiteSpace(RequestToken[0]))
                throw new InvalidOperationException("There is not a request token to authorize");

            string dummy;
            return GetAccessToken(RequestToken[0], RequestToken[1], out dummy, out dummy);
        }
        /// <summary>
        /// Uses the request token to get from the server an authorized token that will be stored internally.
        /// </summary>
        /// <param name="requestTokenKey">Request token public part.</param>
        /// <param name="requestTokenSecret">Request token secret part.</param>
        /// <returns>Success or failure reason of the call. If it is different than <see cref="HttpStatusCode.OK"/> it means that the call failed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="requestTokenKey"/> or <paramref name="requestTokenSecret"/> are null, empty or whitespace</exception>
        public HttpStatusCode GetAccessToken(string requestTokenKey, string requestTokenSecret) 
        {
            if (String.IsNullOrWhiteSpace(requestTokenKey))
                throw new ArgumentNullException("requestTokenKey");
            if (String.IsNullOrWhiteSpace(requestTokenSecret))
                throw new ArgumentNullException("requestTokenSecret");

            string dummy;
            return GetAccessToken(requestTokenKey, requestTokenSecret, out dummy, out dummy);
        }
        /// <summary>
        /// Uses the stored request token to get from the server an authorized token that will be stored internally.
        /// </summary>
        /// <returns>Success or failure reason of the call. If it is different than <see cref="HttpStatusCode.OK"/> it means that the call failed.</returns>
        /// <exception cref="InvalidOperationException">There is not a request token to authorize</exception>
        public HttpStatusCode GetAccessToken(out string accessTokenKey, out string accessTokenSecret) 
        {
            if (String.IsNullOrWhiteSpace(RequestToken[0]) || String.IsNullOrWhiteSpace(RequestToken[0]))
                throw new InvalidOperationException("There is not a request token to authorize");

            return GetAccessToken(RequestToken[0], RequestToken[1], out accessTokenKey, out accessTokenSecret);
        }
        /// <summary>
        /// Uses the request token to get from the server an authorized token that will be stored internally.
        /// </summary>
        /// <param name="requestTokenKey">Request token public part.</param>
        /// <param name="requestTokenSecret">Request token secret part.</param>
        /// <param name="accessTokenKey">Access token public part.</param>
        /// <param name="accessTokenSecret">Access token secret part.</param>
        /// <returns>Success or failure reason of the call. If it is different than <see cref="HttpStatusCode.OK"/> it means that the call failed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="requestTokenKey"/> or <paramref name="requestTokenSecret"/> are null, empty or whitespace</exception>
        public HttpStatusCode GetAccessToken(string requestTokenKey, string requestTokenSecret, out string accessTokenKey, out string accessTokenSecret)
        {
            if (String.IsNullOrWhiteSpace(requestTokenKey))
                throw new ArgumentNullException("requestTokenKey");
            if (String.IsNullOrWhiteSpace(requestTokenSecret))
                throw new ArgumentNullException("requestTokenSecret");

            accessTokenKey = "";
            accessTokenSecret = "";

            var callResult = ApiRunner.ExecuteOAuthApiCall(
                QueryType.GetAccessToken, ApiURLs.AccessToken, new List<ApiParameter>(), RequestMethod.Post,
                ConsumerToken[0], ConsumerToken[1], requestTokenKey, requestTokenSecret);

            if (callResult.StatusCode != HttpStatusCode.OK)
                return callResult.StatusCode;

            var responseBytes = new byte[callResult.ResponseStream.Length];
            callResult.ResponseStream.Read(responseBytes, 0, (int)callResult.ResponseStream.Length);
            string responseString = System.Text.Encoding.UTF8.GetString(responseBytes);
            var match = TokensFromServerResponseRegex.Match(responseString);
            AccessToken[0] = match.Groups["token"].Value;
            AccessToken[1] = match.Groups["token_secret"].Value;
            accessTokenKey = AccessToken[0];
            accessTokenSecret = AccessToken[1];

            return callResult.StatusCode;
        }
        /// <summary>
        /// Sends a Request (a single ApiCall) to the server using the stored consumer and access tokens. 
        /// </summary>
        /// <param name="call">API call to be sent.</param>
        /// <param name="response">Server response.</param>
        /// <returns>Success or failure reason of the call. If it is different than <see cref="HttpStatusCode.OK"/> it means that the call failed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="call"/> is <code>null</code></exception>
        public HttpStatusCode SendRequest(ApiCall call, out string response)
        {
            if (call == null)
                throw new ArgumentNullException("call");

            var request = new ApiRequest();
            request.EnqueueApiCall(call);
            return SendRequest(request, out response);
        }
        /// <summary>
        /// Sends a Request (one or more ApiCall's) to the server using the stored consumer and access tokens. 
        /// </summary>
        /// <param name="call">API call(s) to be sent.</param>
        /// <param name="response">Server response.</param>
        /// <returns>Success or failure reason of the call. If it is different than <see cref="HttpStatusCode.OK"/> it means that the call failed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request"/> is <code>null</code></exception>
        /// <exception cref="ArgumentException"><paramref name="request"/> must contain at least one <see cref="ApiCall"/></exception>
        public HttpStatusCode SendRequest(ApiRequest request, out string response)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.ApiCallsCount < 1)
                throw new ArgumentException("The request must contains at least one ApiCall", "request");

            List<ApiParameter> parameters = new List<ApiParameter>();
            parameters.Add(new ApiParameter()
                {
                    Name = "request",
                    UnencodedValue = request.ToJson(),
                });

            var callResult = ApiRunner.ExecuteOAuthApiCall(
                QueryType.SendApiRequest, ApiURLs.Server, parameters, RequestMethod.Post, ConsumerToken[0], ConsumerToken[1], AccessToken[0], AccessToken[1]);

            if (callResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBytes = new byte[callResult.ResponseStream.Length];
                callResult.ResponseStream.Read(responseBytes, 0, (int)callResult.ResponseStream.Length);
                response = System.Text.Encoding.UTF8.GetString(responseBytes);
            }
            else
                response = "";

            return callResult.StatusCode;
        }
    }
}