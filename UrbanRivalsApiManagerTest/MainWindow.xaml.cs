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
        

        private void GetClans(object sender, RoutedEventArgs e)
        {
            if (!isFullyAuthenticated)
            {
                MessageBox.Show("Finish step 3 before this");
                return;
            }

            var getClansCall = new ApiCallList.Characters.GetClans();
            getClansCall.ItemsFilter = new List<string>() { "name" };
            var request = new ApiRequest(getClansCall);
            string responseString;
            HttpStatusCode status = manager.SendRequest(request, out responseString);
            if (status != HttpStatusCode.OK)
            {
                if (status == HttpStatusCode.MethodNotAllowed)
                {
                    MessageBox.Show("You don't have Public access");
                    return;
                }
                else
                {
                    MessageBox.Show("Something wrong happened, this is the server response: " + status.ToString());
                    return;
                }
            }

            dynamic response = JsonDecoder.Decode(responseString);

            StringBuilder builder = new StringBuilder("These are the available clans: ");
            foreach (dynamic value in response[getClansCall.Call]["items"])
                builder.AppendFormat("{0}, ", value["name"].ToString());
            builder.Remove(builder.Length - 2, 2);

            MessageBox.Show(builder.ToString());
        }
        private void GetPlayerNameAndLevel(object sender, RoutedEventArgs e)
        {
            if (!isFullyAuthenticated)
            {
                MessageBox.Show("Finish step 3 before this");
                return;
            }

            var getPlayerName = new ApiCallList.General.GetPlayer();
            getPlayerName.ContextFilter = new List<string>() { "player.name", "player.level" }; 

            string responseString;
            var request = new ApiRequest(getPlayerName);
            HttpStatusCode status = manager.SendRequest(request, out responseString);

            if (status != HttpStatusCode.OK)
            {
                if (status == HttpStatusCode.MethodNotAllowed)
                {
                    MessageBox.Show("You don't have User access");
                    return;
                }
                else
                {
                    MessageBox.Show("Something wrong happened, this is the server response: " + status.ToString());
                    return;
                }
            }
            dynamic response = JsonDecoder.Decode(responseString);

            string name = response[getPlayerName.Call]["context"]["player"]["name"].ToString();
            int level = int.Parse(response[getPlayerName.Call]["context"]["player"]["level"].ToString());

            MessageBox.Show(String.Format("The player name is {0} and achieved level {1}", name, level));
        }
        private void GetTipsOnLanguage(object sender, RoutedEventArgs e)
        {
            if (!isFullyAuthenticated)
            {
                MessageBox.Show("Finish step 4 before this");
                return;
            }

            string language = "en";
            if ((bool)es.IsChecked)
                language = "es";
            else if ((bool)fr.IsChecked)
                language = "fr";

            var setLanguageCall = new ApiCallList.Players.SetLanguages(new List<string>() { language });

            var getTipsCall = new ApiCallList.General.GetTips();

            var request = new ApiRequest();

            request.EnqueueApiCall(setLanguageCall);
            request.EnqueueApiCall(getTipsCall);

            string responseString;
            HttpStatusCode status = manager.SendRequest(request, out responseString); 
            if (status != HttpStatusCode.OK)
            {
                if (status == HttpStatusCode.MethodNotAllowed)
                {
                    MessageBox.Show("You don't have Public or User access");
                    return;
                }
                else
                {
                    MessageBox.Show("Something wrong happened, this is the server response: " + status.ToString());
                    return;
                }
            }
            dynamic response = JsonDecoder.Decode(responseString);

            StringBuilder builder = new StringBuilder();
            foreach (string tip in response[getTipsCall.Call]["items"])
                builder.AppendLine(tip);

            MessageBox.Show(builder.ToString());
        }
        private void SendMessageToGuild(object sender, RoutedEventArgs e)
        {
            if (!isFullyAuthenticated)
            {
                MessageBox.Show("Finish step 4 before this");
                return;
            }

            if (SendMessage.Text == "")
            {
                MessageBox.Show("You must write a message");
                return;
            }

            var sendMessageCall = new ApiCallList.Guilds.SendGuildMsg(SendMessage.Text);

            sendMessageCall.SetParamenterValue("msg", SendMessage.Text);
            sendMessageCall.TrySetParamenterValue("msg", SendMessage.Text);
            sendMessageCall.msg = SendMessage.Text;

            string responseString;
            var request = new ApiRequest(sendMessageCall);
            HttpStatusCode status = manager.SendRequest(request, out responseString); 
            if (status != HttpStatusCode.OK)
            {
                if (status == HttpStatusCode.MethodNotAllowed)
                {
                    MessageBox.Show("You don't have Action access");
                    return;
                }
                else
                {
                    MessageBox.Show("Something wrong happened, this is the server response: " + status.ToString());
                    return;
                }
            }

            dynamic response = JsonDecoder.Decode(responseString);
            string error = response[sendMessageCall.Call]["context"]["error"].ToString();
            if (error == "0" || error == "false")
                MessageBox.Show("The message was sent");
            else
                MessageBox.Show(String.Format("The message couldn't be sent"));
        }        

        private void CopyURL(object sender, RoutedEventArgs e)
        {
            if (AuthorizeURL.Text == "")
            {
                MessageBox.Show("There is no URL. Finish previous steps and click on get URL");
                return;
            }
            Clipboard.SetDataObject(AuthorizeURL.Text);
        }
        private void GoToUrl(object sender, RoutedEventArgs e)
        {
            if (AuthorizeURL.Text == "")
            {
                MessageBox.Show("There is no URL. Finish previous steps and click on get URL");
                return;
            }
            System.Diagnostics.Process.Start(AuthorizeURL.Text);
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}
