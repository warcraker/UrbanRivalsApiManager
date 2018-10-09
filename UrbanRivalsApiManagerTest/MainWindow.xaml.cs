using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using UrbanRivalsApiManager;
using Procurios.Public;

namespace UrbanRivalsApiManager.Test
{
    public partial class MainWindow : Window
    {
        ApiManager manager;
        bool isFullyAuthenticated;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void onConsumerTokenKeyTextChanged(object sender, TextChangedEventArgs e)
        {
            prv_clearFields();
            prv_initializeManager();
        }
        private void onConsumerTokenSecretTextChanged(object sender, TextChangedEventArgs e)
        {
            prv_clearFields();
            prv_initializeManager();
        }
        private void onGetUrlButtonClicked(object sender, RoutedEventArgs e)
        {
            if (this.manager == null)
            {
                MessageBox.Show("Finish step 1 before this");
            }
            else
            {
                HttpStatusCode statusCode;
                string url;

                statusCode = this.manager.GetAuthorizeURL(out url);
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                    {
                        this.AuthorizeURL.Text = url;
                        break;
                    }
                    default:
                    {
                        MessageBox.Show($"Something happened while asking the server for the authorize token. Check if the consumer token is correctly written. Error code: {statusCode}");
                        break;
                    }
                }
            }
        }
        private void onGetAccessTokenButtonClicked(object sender, RoutedEventArgs e)
        {
            string url;

            url = this.AuthorizeURL.Text;
            if (String.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Finish step 2 before this");
            }
            else
            {
                HttpStatusCode statusCode;
                string accessKey;
                string accessSecret;

                statusCode = this.manager.GetAccessToken(out accessKey, out accessSecret);
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                        this.AccessKey.Text = accessKey;
                        this.AccessSecret.Text = accessSecret;
                        this.isFullyAuthenticated = true;
                        break;
                    default:
                        MessageBox.Show($"Something happened while asking the server for the access token. Make sure that you log into urban rivals BEFORE using the url provided on the previous step, and that the authorize button is clicked. Error code: {statusCode}");
                        break;
                }
            }
        }
        private void onGetClansButtonClicked(object sender, RoutedEventArgs e)
        {
            if (this.isFullyAuthenticated == false)
            {
                MessageBox.Show("Finish step 3 before this");
            }
            else
            {
                List<string> itemsFilterByName;
                ApiCall getClansCall;
                ApiRequest getClansRequest;
                HttpStatusCode statusCode;
                string responseString;

                itemsFilterByName = new List<string>();
                itemsFilterByName.Add("name");
                getClansCall = new ApiCallList.Characters.GetClans();
                getClansCall.ItemsFilter = itemsFilterByName;
                getClansRequest = new ApiRequest(getClansCall);

                statusCode = manager.SendRequest(getClansRequest, out responseString);
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                    {
                        StringBuilder builder;
                        dynamic response;
                        dynamic callPart;
                        dynamic itemsPart;
                        string message;

                        builder = new StringBuilder("These are the available clans: ");
                        response = JsonDecoder.Decode(responseString);
                        callPart = response[getClansCall.Call];
                        itemsPart = callPart["items"];

                        foreach (dynamic item in itemsPart)
                        {
                            string name;

                            name = prv_getDynamicKeyAsString(item, "name");
                            builder.Append($"{name}, ");
                        }
                        builder.Remove(builder.Length - 2, 2);
                        message = builder.ToString();
                        MessageBox.Show(message);
                        break;
                    }
                    case HttpStatusCode.MethodNotAllowed:
                    {
                        MessageBox.Show("The consumer token provided doesn't have Public access");
                        break;
                    }
                    default:
                    {
                        MessageBox.Show($"Something happened while asking the server for the clans. Error code: {statusCode}");
                        break;
                    }
                }
            }
        }
        private void onGetPlayerNameAndLevelButtonClicked(object sender, RoutedEventArgs e)
        {
            if (this.isFullyAuthenticated == false)
            {
                MessageBox.Show("Finish step 3 before this");
            }
            else
            {
                List<string> contextFilterByNameAndLevel;
                ApiCall getPlayerCall;
                ApiRequest getPlayerRequest;
                HttpStatusCode statusCode;
                string responseString;

                contextFilterByNameAndLevel = new List<string>();
                contextFilterByNameAndLevel.Add("player.name");
                contextFilterByNameAndLevel.Add("player.level");
                getPlayerCall = new ApiCallList.General.GetPlayer();
                getPlayerCall.ContextFilter = contextFilterByNameAndLevel;
                getPlayerRequest = new ApiRequest(getPlayerCall);

                statusCode = manager.SendRequest(getPlayerRequest, out responseString);

                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                    {
                        dynamic response;
                        dynamic callPart;
                        dynamic contextPart;
                        dynamic playerPart;
                        string playerName;
                        string playerLevel;
                        string message;

                        response = JsonDecoder.Decode(responseString);
                        callPart = response[getPlayerCall.Call];
                        contextPart = callPart["context"];
                        playerPart = contextPart["player"];

                        playerName = prv_getDynamicKeyAsString(playerPart, "name");
                        playerLevel = prv_getDynamicKeyAsString(playerPart, "level");
                        message = $"The player name is {playerName} and achieved level {playerLevel}";
                        MessageBox.Show(message);
                        break;
                    }
                    case HttpStatusCode.MethodNotAllowed:
                    {
                        MessageBox.Show("The consumer token provided doesn't have User access");
                        break;
                    }
                    default:
                    {
                        MessageBox.Show($"Something happened while asking the server for the player details. Error code: {statusCode}");
                        break;
                    }
                }

            }
        }
        private void onGetTipsButtonClicked(object sender, RoutedEventArgs e)
        {
            if (isFullyAuthenticated == false)
            {
                MessageBox.Show("Finish step 3 before this");
            }
            else
            {
                string selectedLanguage;
                List<string> languagesList;
                ApiCall setLanguageCall;
                ApiCall getTipsCall;
                ApiRequest request;
                HttpStatusCode statusCode;
                string responseString;

                selectedLanguage = prv_getSelectedLanguage();
                languagesList = new List<string>();
                languagesList.Add(selectedLanguage);

                setLanguageCall = new ApiCallList.Players.SetLanguages(languagesList);
                getTipsCall = new ApiCallList.General.GetTips();

                request = new ApiRequest(setLanguageCall);
                request.EnqueueApiCall(getTipsCall);

                statusCode = manager.SendRequest(request, out responseString);
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                    {
                        dynamic response;
                        dynamic callPart;
                        dynamic itemsPart;
                        StringBuilder builder;

                        response = JsonDecoder.Decode(responseString);
                        callPart = response[getTipsCall.Call];
                        itemsPart = callPart["items"];

                        builder = new StringBuilder();
                        foreach (dynamic item in itemsPart)
                        {
                            string tip;
                            string line;

                            tip = item.ToString();
                            line = $"- {tip}";
                            builder.AppendLine(line);
                        }

                        MessageBox.Show(builder.ToString());
                        break;
                    }
                    case HttpStatusCode.MethodNotAllowed:
                    {
                        MessageBox.Show("The consumer token provided doesn't have Public or User access");
                        break;
                    }
                    default:
                    {
                        MessageBox.Show($"Something happened while asking the server for game tips. Error code: {statusCode}");
                        break;
                    }
                }
            }
        }
        private void onSendMessageToGuildButtonClicked(object sender, RoutedEventArgs e)
        {
            string messageToSend;

            messageToSend = this.SendMessage.Text;
            if (SendMessage.Text == "")
            {
                MessageBox.Show("You must write a message");
            }
            else if (isFullyAuthenticated == false)
            {
                MessageBox.Show("Finish step 4 before this");
            }
            else
            {
                ApiCall sendMessageCall;
                ApiRequest request;
                HttpStatusCode statusCode;
                string responseString;

                sendMessageCall = new ApiCallList.Guilds.SendGuildMsg(messageToSend);
                request = new ApiRequest(sendMessageCall);
                statusCode = manager.SendRequest(request, out responseString);
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                    {
                        dynamic response;
                        dynamic callPart;
                        dynamic contextPart;
                        string errorCode;

                        response = JsonDecoder.Decode(responseString);
                        callPart = response[sendMessageCall.Call];
                        contextPart = callPart["context"];
                        errorCode = prv_getDynamicKeyAsString(contextPart, "error");

                        if (errorCode == "0" || errorCode == "false")
                        {
                            MessageBox.Show("The message was sent");
                        }
                        else
                        {
                            MessageBox.Show($"The message failed to be sent. Error code: {errorCode}");
                        }
                        break;
                    }
                    case HttpStatusCode.MethodNotAllowed:
                    {
                        MessageBox.Show("The consumer token provided doesn't have Action access");
                        break;
                    }
                    default:
                    {
                        MessageBox.Show($"Something happened while sending the message to the guild. Error code: {statusCode}");
                        break;
                    }
                }

            }
        }
        private void onCopyUrlButtonClicked(object sender, RoutedEventArgs e)
        {
            string url;

            url = this.AuthorizeURL.Text;
            if (String.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("There is no URL. Finish previous steps and click on get URL");
            }
            else
            {
                Clipboard.SetDataObject(url);
            }
        }
        private void onNavigateToUrlButtonClicked(object sender, RoutedEventArgs e)
        {
            string url;

            url = this.AuthorizeURL.Text;
            if (String.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("There is no URL. Finish previous steps and click on get URL");
            }
            else
            {
                prv_openUrlUsingDefaultBrowser(url);
            }
        }
        private void onHyperlinkClicked(object sender, RequestNavigateEventArgs e)
        {
            string url;

            url = e.Uri.ToString();
            prv_openUrlUsingDefaultBrowser(url);
        }

        private static string prv_getDynamicKeyAsString(dynamic item, string key)
        {
            string value;
            object valueAsObject;

            valueAsObject = item[key];
            value = valueAsObject.ToString();

            return value;
        }
        private void prv_clearFields()
        {
            AuthorizeURL.Text = "";
            AccessKey.Text = "";
            AccessSecret.Text = "";
            SendMessage.Text = "";
        }
        private void prv_initializeManager()
        {
            string consumerKey;
            string consumerSecret;

            consumerKey = this.ConsumerKey.Text;
            consumerSecret = this.ConsumerSecret.Text;
            if (String.IsNullOrWhiteSpace(consumerKey) == false && String.IsNullOrWhiteSpace(consumerSecret) == false)
            {
                ApiManager manager;

                manager = ApiManager.CreateApiManager(consumerKey, consumerSecret);
                this.manager = manager;
            }
            this.isFullyAuthenticated = false;
        }
        private string prv_getSelectedLanguage()
        {
            string language;

            if (this.en.IsChecked == true)
            {
                language = "en";
            }
            else if (this.es.IsChecked == true)
            {
                language = "es";
            }
            else if (this.fr.IsChecked == true)
            {
                language = "fr";
            }
            else
            {
                throw new Exception("No language is selected");
            }

            return language;
        }
        private static void prv_openUrlUsingDefaultBrowser(string url)
        {
            System.Diagnostics.Process.Start(url);
        }
    }
}
