using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using ExtApi.Engine;
using OAuth;

namespace Warcraker.UrbanRivals.ApiManager
{
    public class ApiManager
    {
        private static readonly Regex PRV_TOKENS_FROM_SERVER_RESPONSE_REGEX = new Regex(@"oauth_token=(?<token>[0-9a-f]+)&oauth_token_secret=(?<token_secret>[0-9a-f]+)");
        private static readonly Uri PRV_AUTHORIZE_TOKEN_URI = new Uri(@"https://www.urban-rivals.com/api/auth/authorize.php");
        private const string PRV_REQUEST_TOKEN_URL = @"https://www.urban-rivals.com/api/auth/request_token.php";
        private const string PRV_ACCESS_TOKEN_URL = @"https://www.urban-rivals.com/api/auth/access_token.php";
        private const string PRV_SERVER_URL = @"https://www.urban-rivals.com/api/";
        private const string PRV_CALLBACK_URL = @"https://www.urban-rivals.com/";
        private const string PRV_HTTP_POST_METHOD = "POST";

        private readonly OAuthBase oAuthBase;
        private readonly ApiRunner apiRunner;
        private readonly string[] consumerToken;
        private readonly string[] requestToken;
        private readonly string[] accessToken;
        private bool urlObtained;
        private bool accessTokenObtained;

        public static ApiManager CreateApiManager(string consumerKey, string consumerSecret)
        {
            ApiManager apiManager;

            apiManager = new ApiManager(consumerKey, consumerSecret);

            return apiManager;
        }
        public static ApiManager CreateApiManagerReadyForRequests(string consumerKey, string consumerSecret, string accessTokenKey, string accessTokenSecret)
        {
            ApiManager apiManager;

            if (String.IsNullOrWhiteSpace(accessTokenKey))
                throw new ArgumentNullException(nameof(accessTokenKey));
            if (String.IsNullOrWhiteSpace(accessTokenSecret))
                throw new ArgumentNullException(nameof(accessTokenSecret));

            apiManager = new ApiManager(consumerKey, consumerSecret);
            apiManager.accessToken[0] = accessTokenKey;
            apiManager.accessToken[1] = accessTokenSecret;
            apiManager.accessTokenObtained = true;

            return apiManager;
        }
        private ApiManager(string consumerKey, string consumerSecret)
        {
            if (String.IsNullOrWhiteSpace(consumerKey))
                throw new ArgumentNullException(nameof(consumerKey));
            if (String.IsNullOrWhiteSpace(consumerSecret))
                throw new ArgumentNullException(nameof(consumerSecret));

            this.apiRunner = new ApiRunner();
            this.oAuthBase = new OAuthBase();

            this.consumerToken = prv_initTokenPair(consumerKey, consumerSecret);
            this.requestToken = prv_initTokenPair("", "");
            this.accessToken = prv_initTokenPair("", "");

            this.urlObtained = false;
            this.accessTokenObtained = false;
        }

        public HttpStatusCode GetAuthorizeURL(out string authorizeUrl)
        {
            HttpStatusCode httpStatusCode;
            ExtApiCallResult callResponse;
            List<ApiParameter> emptyParametersList;

            emptyParametersList = new List<ApiParameter>();
            callResponse = this.apiRunner.ExecuteOAuthApiCall(
                QueryType.GetRequestToken, PRV_REQUEST_TOKEN_URL, emptyParametersList, RequestMethod.Post, 
                this.consumerToken[0], this.consumerToken[1], "", "");
            httpStatusCode = callResponse.StatusCode;
            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                {
                    string responseString;
                    bool isMatch;

                    responseString = prv_getStringFromResponse(callResponse);
                    isMatch = PRV_TOKENS_FROM_SERVER_RESPONSE_REGEX.IsMatch(responseString);
                    if (isMatch == true)
                    {
                        Match responseMatch;
                        string timestamp;
                        string nonce;
                        string signature;
                        string normalizedUrl;
                        string normalizedRequestParameters;

                        responseMatch = PRV_TOKENS_FROM_SERVER_RESPONSE_REGEX.Match(responseString);
                        this.requestToken[0] = responseMatch.Groups["token"].Value;
                        this.requestToken[1] = responseMatch.Groups["token_secret"].Value;
                        timestamp = this.oAuthBase.GenerateTimeStamp();
                        nonce = this.oAuthBase.GenerateNonce();
                        signature = this.oAuthBase.GenerateSignature(PRV_AUTHORIZE_TOKEN_URI,
                            this.consumerToken[0], this.consumerToken[1], this.requestToken[0], this.requestToken[1],
                            PRV_HTTP_POST_METHOD, timestamp, nonce, out normalizedUrl, out normalizedRequestParameters);
                        authorizeUrl = $"{normalizedUrl}?{normalizedRequestParameters}&oauth_signature={signature}&oauth_callback={PRV_CALLBACK_URL}";
                        this.urlObtained = true;
                    }
                    else
                    {
                        httpStatusCode = (HttpStatusCode)418; // :)
                        authorizeUrl = "";
                        this.urlObtained = false;
                    }
                    break;
                }
                default:
                {
                    authorizeUrl = "";
                    this.urlObtained = false;
                    break;
                }
            }

            return httpStatusCode;
        }
        public HttpStatusCode GetAccessToken(out string accessTokenKey, out string accessTokenSecret)
        {
            HttpStatusCode httpStatusCode;
            ExtApiCallResult callResponse;
            List<ApiParameter> emptyParametersList;

            if (this.urlObtained == false)
                throw new InvalidOperationException($"The user must access to the URL provided by {nameof(GetAuthorizeURL)} and click on [Authorize App] before running this method");

            emptyParametersList = new List<ApiParameter>();
            callResponse = this.apiRunner.ExecuteOAuthApiCall(
                QueryType.GetAccessToken, PRV_ACCESS_TOKEN_URL, emptyParametersList, RequestMethod.Post, 
                this.consumerToken[0], this.consumerToken[1], this.requestToken[0], this.requestToken[1]);
            httpStatusCode = callResponse.StatusCode;

            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                {
                    string responseString;
                    bool isMatch;

                    responseString = prv_getStringFromResponse(callResponse);
                    isMatch = PRV_TOKENS_FROM_SERVER_RESPONSE_REGEX.IsMatch(responseString);
                    if (isMatch == true)
                    {
                        Match match;

                        match = PRV_TOKENS_FROM_SERVER_RESPONSE_REGEX.Match(responseString);
                        this.accessToken[0] = match.Groups["token"].Value;
                        this.accessToken[1] = match.Groups["token_secret"].Value;
                        this.accessTokenObtained = true;
                        accessTokenKey = this.accessToken[0];
                        accessTokenSecret = this.accessToken[1];
                    }
                    else
                    {
                        httpStatusCode = (HttpStatusCode)418;
                        accessTokenKey = "";
                        accessTokenSecret = "";
                    }
                    break;
                }
                default:
                {
                    accessTokenKey = "";
                    accessTokenSecret = "";
                    break;
                }
            }

            return httpStatusCode;
        }
        public HttpStatusCode SendRequest(ApiRequest request, out string response)
        {
            HttpStatusCode httpStatusCode;
            List<ApiParameter> parameters;
            ApiParameter parameter;
            string requestAsJson;
            ExtApiCallResult callResponse;

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.ApiCallsCount < 1)
                throw new ArgumentException($"Request must contains at least one {nameof(ApiCall)}", nameof(request));

            if (this.accessTokenObtained == false)
                throw new InvalidOperationException($"No access token is available. Use {nameof(GetAccessToken)} method or {nameof(CreateApiManagerReadyForRequests)} factory");

            requestAsJson = request.ToJson();
            parameter = new ApiParameter();
            parameter.Name = "request";
            parameter.UnencodedValue = requestAsJson;

            parameters = new List<ApiParameter>();
            parameters.Add(parameter);

            callResponse = this.apiRunner.ExecuteOAuthApiCall(
                QueryType.SendApiRequest, PRV_SERVER_URL, parameters, RequestMethod.Post, 
                this.consumerToken[0], this.consumerToken[1], this.accessToken[0], this.accessToken[1]);

            httpStatusCode = callResponse.StatusCode;
            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                    response = prv_getStringFromResponse(callResponse);
                    break;
                default:
                    response = "";
                    break;
            }

            return httpStatusCode;
        }

        private static string[] prv_initTokenPair(string key, string secret)
        {
            string[] token;

            token = new string[2];
            token[0] = key;
            token[1] = secret;

            return token;
        }
        private static string prv_getStringFromResponse(ExtApiCallResult callResponse)
        {
            string responseString;
            byte[] responseBytes;
            int responseStreamLength;

            responseStreamLength = (int)callResponse.ResponseStream.Length;
            responseBytes = new byte[responseStreamLength];
            callResponse.ResponseStream.Read(responseBytes, 0, responseStreamLength);
            responseString = Encoding.UTF8.GetString(responseBytes);

            return responseString;
        }
    }
}