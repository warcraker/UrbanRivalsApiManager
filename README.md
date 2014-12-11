UrbanRivalsApiManager
=====================

C# library to work with the Urban Rivals API. It contains example code in /UrbanRivalsApiManagerTest

Basic usage:
------------
1. Go to http://www.urban-rivals.com/api/developer/ and get your ConsumerKey and ConsumerSecret

2. Create an instance of ApiManager using this constructor:

    public ApiManager(string consumerKey, string consumerSecret)

3. Get a Request Token with:

    public HttpStatusCode GetRequestToken()

4. Instruct the user to authorize your app using the URL provided with:

    public string GetAuthorizeRequestTokenURL()

5. Once authorized, get one AccessKey and AccessSecret with:

    public HttpStatusCode GetAccessToken()

6. Create an ApiCall using one of the list of ApiCallList. Use only allowed calls for your consumer keys.
Here are some examples:

    var getInfoFromPlayerCall = new ApiCallList.General.GetPlayer();
    var sendGuildMessageCall= new ApiCallList.Guilds.SendGuildMsg("My guild is the best");

7. Finally, you can send requests to the server using this method:

    public HttpStatusCode SendRequest(ApiCall call, out string response)

8. You can use the "response" parameter from the previous call if you were expecting data from the server

Advanced usage:
---------------
I'll expect you to know some basics about how the OAuth protocol works.

The ApiManager instance stores the last used Request and Access tokens, but still you can get the result of the GetRequestToken() and GetAccessToken() with the corresponding overloads:

    public HttpStatusCode GetRequestToken(out string requestTokenKey, out string requestTokenSecret)
    public HttpStatusCode GetAccessToken(out string accessTokenKey, out string accessTokenSecret)

If you stored an Access Token from a previous sesion, you can instantiate ApiManager with this overload:

    public ApiManager(string consumerKey, string consumerSecret, string accessTokenKey, string accessTokenSecret)

You can send multiple ApiCall's on the same request using ApiRequest. Here's an example:

    var firstCall = new ApiCallList.General.GetPlayer();
    var secondCall = new ApiCallList.Guilds.SendGuildMsg("My guild is the best");
    var thirdCall = new ApiCallList.Characters.GetCharacterLevels(1000) // Card with ID=1000
    var request = new ApiRequest();
    request.EnqueueApiCall(firstCall);
    request.EnqueueApiCall(secondCall);
    request.EnqueueApiCall(thirdCall);
    // manager is an ApiManager instance with correct Consumer and Access tokens
    string response;
    manager.SendRequest(request, out response);

You can customize calls setting their parameters like this:

    var call = new ApiCallList.Characters.GetCharacterLevels();
    // I want the ID=987
    call.SetParameterValue("characterID", 987);
    // I've decided that I want the 888 instead
    call.characterID = 888

You can also set the ItemFilter and ContextFilter of any ApiCall to make the server response lighter.

Check /UrbanRivalsApiManagerTest to see more code examples
