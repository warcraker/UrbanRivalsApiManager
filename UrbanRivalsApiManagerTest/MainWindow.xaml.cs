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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApiManager manager;
        bool isFullyAuthenticated;

        public MainWindow()
        {
            InitializeComponent();
        }
        
        // --- OAuth Protocol: Sesion Authentication ---
        private void GetRequestToken(object sender, RoutedEventArgs e)
        {
            if ((String.IsNullOrWhiteSpace(ConsumerKey.Text) 
                || String.IsNullOrWhiteSpace(ConsumerSecret.Text)))
            {
                MessageBox.Show("Finish step 1 before this");
                return;
            }

            manager = new ApiManager(ConsumerKey.Text, ConsumerSecret.Text);
            // Once you finish steps 1 to 4, you can store the Access Token and use this overload to avoid repeating the entire process
            // manager = new ApiManager(consumerKey, consumerSecret, accessTokenKey, accessTokenSecret);

            string requestKey, requestSecret;
            HttpStatusCode status = manager.GetRequestToken(out requestKey, out requestSecret);
            if (status != HttpStatusCode.OK)
            {
                MessageBox.Show("The Consumer Token provided on step 1 is invalid");
                return;
            }
            
            RequestKey.Text = requestKey;
            RequestSecret.Text = requestSecret;
        }
        private void AuthorizeRequestToken(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(RequestKey.Text))
            {
                MessageBox.Show("Finish step 2 before this");
                return;
            }
            AuthorizeURL.Text = manager.GetAuthorizeRequestTokenURL();
        }
        private void GetAccessToken(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(AuthorizeURL.Text))
            {
                MessageBox.Show("Finish step 3 before this");
                return;
            }

            string accessKey, accessSecret;
            HttpStatusCode status = manager.GetAccessToken(out accessKey, out accessSecret);
            if (status != HttpStatusCode.OK)
            {
                MessageBox.Show("Finish step 3 before this. You need to log into Urban Rivals before going into the URL provided, and you need to click on Authorize");
                return;
            }
            
            AccessKey.Text = accessKey;
            AccessSecret.Text = accessSecret;
            isFullyAuthenticated = true;
        }

        // --- API Requests ---
        private void GetClans(object sender, RoutedEventArgs e)
        {
            if (!isFullyAuthenticated)
            {
                MessageBox.Show("Finish step 4 before this");
                return;
            }

            var getClansCall = new ApiCallList.Characters.GetClans();
            // We are only interested in names, so we can make this lighter using ItemsFilter
            getClansCall.ItemsFilter = new List<string>() { "name" };
            string responseString;
            HttpStatusCode status = manager.SendRequest(getClansCall, out responseString);
            if (status != HttpStatusCode.OK)
            {
                // Use in your app only ApiCall's that you have access to, in order to avoid this
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

            // We can parse the response into a dynamic object for confortable using the Procurios.User.JsonDecoder library included
            dynamic response = JsonDecoder.Decode(responseString);

            /* A server response decoded like that has this structure
             * [NameOfTheCall][ItemsOrContext][Values]
             * NameOfTheCall is on the ApiCall.Call field
             * ItemsOrContext is either "items" or "context"
             * Values are the interesting results. Can be empty, contain a single value, recurse into more fields, etc.
             */

            StringBuilder builder = new StringBuilder("These are the available clans: ");
            foreach (dynamic value in response[getClansCall.Call]["items"])
                builder.AppendFormat("{0}, ", value["name"].ToString());
            builder.Remove(builder.Length - 2, 2); // Remove the trailing ", "

            MessageBox.Show(builder.ToString());
        }
        private void GetPlayerNameAndLevel(object sender, RoutedEventArgs e)
        {
            if (!isFullyAuthenticated)
            {
                MessageBox.Show("Finish step 4 before this");
                return;
            }

            var getPlayerName = new ApiCallList.General.GetPlayer();
            // We can get as granular as we want with the filters, as long as we know what fields the server is gona serve us
            getPlayerName.ContextFilter = new List<string>() { "player.name", "player.level" }; // We use the dot (.) syntax here

            string responseString;
            HttpStatusCode status = manager.SendRequest(getPlayerName, out responseString);
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

            // This call have a parameter, and is compulsory, so we put it on the constructor
            var setLanguageCall = new ApiCallList.Players.SetLanguages(new List<string>() { language });

            // This is the second call
            var getTipsCall = new ApiCallList.General.GetTips();

            // Now we have 2 ApiCall's, one to set the language, and other to get the tips. We can use ApiRequest to send multiple ApiCall's
            var request = new ApiRequest();

            // We must add the calls in order
            request.EnqueueApiCall(setLanguageCall);
            request.EnqueueApiCall(getTipsCall);

            string responseString;
            HttpStatusCode status = manager.SendRequest(request, out responseString); // We use ApiRequest here
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
            // We are only interested in the output of the second call. We don't check if the first call failed. Check the oficial documentation to know what would happen
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

            // To set a parameter, you can use any of these variants
            sendMessageCall.SetParamenterValue("msg", SendMessage.Text);
            sendMessageCall.TrySetParamenterValue("msg", SendMessage.Text);
            sendMessageCall.msg = SendMessage.Text;

            // Note: I don't have Action access on my keys, so I can't check if the following code works :(
            string responseString;
            HttpStatusCode status = manager.SendRequest(sendMessageCall, out responseString); // We use ApiRequest here
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

            // Here we could use the response to check if there was any error through the context
            dynamic response = JsonDecoder.Decode(responseString);
            string error = response[sendMessageCall.Call]["context"]["error"].ToString();
            if (error == "0" || error == "false")
                MessageBox.Show("The message was sent");
            else
                MessageBox.Show(String.Format("The message couldn't be sent"));
        }        

        private void Clear(object sender, TextChangedEventArgs e)
        {
            manager = null;
            isFullyAuthenticated = false;
            RequestKey.Text = "";
            RequestSecret.Text = "";
            AuthorizeURL.Text = "";
            AccessKey.Text = "";
            AccessSecret.Text = "";
            SendMessage.Text = "";
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
