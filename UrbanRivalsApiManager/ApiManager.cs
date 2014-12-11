using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

using ExtApi.Engine;
using OAuth;

namespace UrbanRivalsApiManager
{
    internal static class ApiURLs
    {
        public static readonly string AccessToken = @"http://www.urban-rivals.com/api/auth/access_token.php";
        public static readonly string RequestToken = @"http://www.urban-rivals.com/api/auth/request_token.php";
        public static readonly string AuthorizeToken = @"http://www.urban-rivals.com/api/auth/authorize.php";
        public static readonly string Server = @"http://www.urban-rivals.com/api/";
        public static readonly string Callback = @"http://www.urban-rivals.com/";
    }

    /// <summary>
    /// Manages authentication through OAuth with Urban Rivals and allows to send requests.
    /// </summary>
    /// <remarks> The OAuth protocol (Consumer > Request > AuthorizeRequest > Access) must be followed in order. Allows the use of a valid Access Token to avoid sesion re-authentication.</remarks>
    public class ApiManager
    {
        private static readonly Regex TokensFromServerResponseRegex = new Regex(@"oauth_token=(?<token>[0-9a-f]+)&oauth_token_secret=(?<token_secret>[0-9a-f]+)");

        private ApiRunner ApiRunner;
        private OAuthBase OAuthBase;

        private string[] ConsumerToken = new string[2];
        private string[] RequestToken = new string[2];
        private string[] AccessToken = new string[2];

        /// <summary>
        /// Creates a new ApiManager with the specified Consumer and Access Tokens.
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="accessTokenKey"></param>
        /// <param name="accessTokenSecret"></param>
        /// <exception cref="ArgumentNullException">consumerKey or consumerSecret are null/empty/whitespace</exception>
        public ApiManager(string consumerKey, string consumerSecret, string accessTokenKey, string accessTokenSecret)
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
        /// Creates a new ApiManager with the specified Consumer Token.
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <exception cref="ArgumentNullException">consumerKey or consumerSecret are null/empty/whitespace</exception>
        public ApiManager(string consumerKey, string consumerSecret)
            : this (consumerKey, consumerSecret, "", "") { }
        private ApiManager() { }

        /// <summary>
        /// Uses the stored Consumer Token to get from the server a Request Token that is stored internally.
        /// </summary>
        /// <returns>The server response. If it is different than <code>HttpStatusCode.OK</code> it means that the call failed.</returns>
        public HttpStatusCode GetRequestToken()
        {
            string dummy;
            return GetRequestToken(out dummy, out dummy);
        }
        /// <summary>
        /// Uses the stored Consumer Token to get from the server a Request Token that is stored internally and returned through the out parameters.
        /// </summary>
        /// <param name="requestTokenKey"></param>
        /// <param name="requestTokenSecret"></param>
        /// <returns>The server response. If it is different than <code>HttpStatusCode.OK</code> it means that the call failed, and the output values are invalid.</returns>
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
        /// Uses the default system web browser to request the user to authorize the stored Request Token.
        /// </summary>
        /// <remarks>The user must be logged into Urban Rivals before calling this.</remarks>
        public void AuthorizeRequestToken()
        {
            AuthorizeRequestToken(RequestToken[0], RequestToken[1]);
        }
        /// <summary>
        /// Uses the default system web browser to request the user to authorize the Request Token.
        /// </summary>
        /// <remarks>The user must be logged into Urban Rivals before calling this.</remarks>
        public void AuthorizeRequestToken(string requestTokenKey, string requestTokenSecret)
        {
            string URL = GetAuthorizeRequestTokenURL(requestTokenKey, requestTokenSecret);

            System.Diagnostics.Process.Start(URL);
        }
        /// <summary>
        /// Creates a valid URL to request the user to authorize the stored Request Token.
        /// </summary>
        /// <remarks>The user must be logged into Urban Rivals before using the URL.</remarks>
        public string GetAuthorizeRequestTokenURL()
        {
            return GetAuthorizeRequestTokenURL(RequestToken[0], RequestToken[1]);
        }
        /// <summary>
        /// Creates a valid URL to request the user to authorize the Request Token.
        /// </summary>
        /// <remarks>The user must be logged into Urban Rivals before using the URL.</remarks>
        public string GetAuthorizeRequestTokenURL(string requestTokenKey, string requestTokenSecret)
        {
            if (String.IsNullOrWhiteSpace(requestTokenKey))
                throw new ArgumentNullException("requestTokenKey");
            if (String.IsNullOrWhiteSpace(requestTokenSecret))
                throw new ArgumentNullException("requestTokenSecret");

            string normalizedUrl, normalizedRequestParameters;
            string timestamp, nonce, signature;

            timestamp = OAuthBase.GenerateTimeStamp();
            nonce = OAuthBase.GenerateNonce();
            signature = OAuthBase.GenerateSignature(
                new Uri(ApiURLs.AuthorizeToken),
                ConsumerToken[0], ConsumerToken[1], requestTokenKey, requestTokenSecret,
                "POST", timestamp, nonce, out normalizedUrl, out normalizedRequestParameters);

            return normalizedUrl + "?" 
                + normalizedRequestParameters
                + "&oauth_signature=" + signature
                + "&oauth_callback=" + ApiURLs.Callback;
        }
        /// <summary>
        /// Uses the stored Request Token to get from the server an Authorized Token that is stored internally.
        /// </summary>
        /// <returns>The server response. If it is different than <code>HttpStatusCode.OK</code> it means that the call failed.</returns>
        public HttpStatusCode GetAccessToken()
        {
            string dummy;
            return GetAccessToken(RequestToken[0], RequestToken[1], out dummy, out dummy);
        }
        /// <summary>
        /// Uses the Request Token to get from the server an Authorized Token that is stored internally.
        /// </summary>
        /// <returns>The server response. If it is different than <code>HttpStatusCode.OK</code> it means that the call failed.</returns>
        public HttpStatusCode GetAccessToken(string requestTokenKey, string requestTokenSecret) 
        {
            string dummy;
            return GetAccessToken(requestTokenKey, requestTokenSecret, out dummy, out dummy);
        }
        /// <summary>
        /// Uses the stored Request Token to get from the server an Authorized Token that is stored internally and returned through the out parameters.
        /// </summary>
        /// <returns>The server response. If it is different than <code>HttpStatusCode.OK</code> it means that the call failed, and the output values are invalid.</returns>
        public HttpStatusCode GetAccessToken(out string accessTokenKey, out string accessTokenSecret) 
        {
            return GetAccessToken(RequestToken[0], RequestToken[1], out accessTokenKey, out accessTokenSecret);
        }
        /// <summary>
        /// Uses the Request Token to get from the server an Authorized Token that is stored internally and returned through the out parameters.
        /// </summary>
        /// <returns>The server response. If it is different than <code>HttpStatusCode.OK</code> it means that the call failed, and the output values are invalid.</returns>
        public HttpStatusCode GetAccessToken(string requestTokenKey, string requestTokenSecret, out string accessTokenKey, out string accessTokenSecret)
        {
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
        /// Sends a Request (a single ApiCall) to the server using the stored Consumer and Access Tokens. 
        /// </summary>
        /// <param name="call"></param>
        /// <param name="response"></param>
        /// <returns>The server response. If it is different than <code>HttpStatusCode.OK</code> it means that the call failed, and the output values are invalid.</returns>
        /// <remarks>The server requries the JSON request to be encoded as an array. ApiCall.ToJson() doesn't returns an array. This method takes care of that.</remarks>
        public HttpStatusCode SendRequest(ApiCall call, out string response)
        {
            if (call == null)
                throw new ArgumentNullException("call");

            var request = new ApiRequest();
            request.EnqueueApiCall(call);
            return SendRequest(request, out response);
        }
        /// <summary>
        /// Sends a Request (one or more ApiCall's) to the server using the stored Consumer and Access Tokens. 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns>The server response. If it is different than <code>HttpStatusCode.OK</code> it means that the call failed, and the output values are invalid.</returns>
        public HttpStatusCode SendRequest(ApiRequest request, out string response)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.ApiCallsCount == 0)
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