using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Xml;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using RestSharp;
using RestSharp.Authenticators;

namespace ExtApi.Engine
{
    public enum QueryType
    {
        GetRequestToken,
        GetAccessToken,
        SendApiRequest,
    }
    public enum RequestMethod
    {
        Get,
        Post
    }

    public class ApiParameter
    {
        /// <summary>
        /// Gets or sets the name of the API parameter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the unencoded value of the api parameter
        /// </summary>
        public string UnencodedValue { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter encoded for use in an URL
        /// </summary>
        public string EncodedValue
        {
            get { return HttpUtility.UrlEncode(UnencodedValue); }
            set { UnencodedValue = HttpUtility.UrlDecode(value); }
        }

        public ApiParameter()
        {
            Name = string.Empty;
            UnencodedValue = string.Empty;
        }

        public override string ToString()
        {
            return String.Format("{0}={1}", Name, EncodedValue);
        }
    }

    public class ExtApiCallResult
    {
        /// <summary>
        /// Gets or sets the status code of the api call callResult
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        protected Stream _responseStream;

        /// <summary>
        /// Gets or sets the api call responseBytes's stream
        /// </summary>
        public Stream ResponseStream
        {
            get { return _responseStream; }
            set
            {
                // Copy the stream into a memory string, so it can be re-read
                const int readSize = 256;
                byte[] buffer = new byte[readSize];
                MemoryStream ms = new MemoryStream();

                int count = value.Read(buffer, 0, readSize);
                while (count > 0)
                {
                    ms.Write(buffer, 0, count);
                    count = value.Read(buffer, 0, readSize);
                }
                ms.Position = 0;
                value.Close();

                _responseStream = ms;
            }
        }

        /// <summary>
        /// Retrieves the api call's callResult as xml
        /// </summary>
        public XDocument XmlResponse
        {
            get
            {
                // Attempt to turn the responseBytes stream into an Linq-To-Xml document
                try { return XDocument.Load(_responseStream); }
                catch (XmlException)
                {
                    return null;
                }

                finally
                {
                    // Reset the stream to the beginning
                    _responseStream.Position = 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets the URL that was built after all parameters were added
        /// </summary>
        public string FinalUrl { get; set; }
    }

    public class ApiRunner
    {
        /// <summary>
        /// Performs an OAuthed api call with the specified access token
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="parameters"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown when no token manager has been specified</exception>
        public ExtApiCallResult ExecuteOAuthApiCall(QueryType queryType, string apiUrl, IList<ApiParameter> parameters, RequestMethod method,
                                                    string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            var client = new RestClient(apiUrl);

            switch (queryType)
            {
                case QueryType.GetRequestToken:
                    client.Authenticator = OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret);
                    break;
                case QueryType.GetAccessToken:
                    client.Authenticator = OAuth1Authenticator.ForAccessToken(consumerKey, consumerSecret, token, tokenSecret);
                    break;
                case QueryType.SendApiRequest:
                    client.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, token, tokenSecret);
                    break;
                default:
                    throw new NotImplementedException();
            }

            var request = new RestRequest();
            request.Method = ConvertRequestMethod(method);

            foreach (var param in parameters)
                request.Parameters.Add(new Parameter { Name = param.Name, Value = param.UnencodedValue, Type = ParameterType.GetOrPost });

            // Execute the request
            return CreateExtApiCallResult(client.Execute(request));
        }

        protected ExtApiCallResult CreateExtApiCallResult(RestResponse response)
        {
            // Put the responseBytes content into a memorystream for reading
            var stream = new MemoryStream();
            var bytes = new System.Text.ASCIIEncoding().GetBytes(response.Content);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Position = 0;

            return new ExtApiCallResult
            {
                StatusCode = response.StatusCode,
                ResponseStream = stream,
                FinalUrl = response.ResponseUri.AbsoluteUri
            };
        }

        /// <summary>
        /// Builds a GET URL based on the specified api URL and the passed in parameters
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string BuildGetUrl(string apiUrl, IList<ApiParameter> parameters)
        {
            StringBuilder resultingUrl = new StringBuilder(apiUrl);

            if (parameters == null || parameters.Count == 0)
                return apiUrl;

            // Add all the parameters to the url
            foreach (var param in parameters)
            {
                // Check if we already have a question mark in the URL (e.g. not the first parameter)
                if (resultingUrl.ToString().Contains('?'))
                    resultingUrl.Append('&');
                else
                    resultingUrl.Append('?');

                resultingUrl.Append(HttpUtility.UrlEncode(param.Name));
                resultingUrl.Append('=');
                resultingUrl.Append(param.EncodedValue);
            }

            return resultingUrl.ToString();
        }

        protected Method ConvertRequestMethod(RequestMethod method)
        {
            if (method == RequestMethod.Get)
                return Method.GET;
            else if (method == RequestMethod.Post)
                return Method.POST;
            else
                throw new NotSupportedException(
                    String.Format("The request method of {0} is not supported", method.ToString()));
        }
    }
}
