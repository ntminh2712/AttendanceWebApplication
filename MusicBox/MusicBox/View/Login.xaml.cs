using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using System.Diagnostics;
using Windows.Storage;
using MusicBox.Entity;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Windows.UI.Popups;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicBox.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {

        static Member memberOld;
        public Login()
        {
            this.InitializeComponent();
            memberOld = new Member();
            setup();
            autoLogin();
        }

        private async void  autoLogin( )
        {
            try
            {
                LoadingIndicator.IsActive = true;
                StorageFile config_login = await ApplicationData.Current.LocalFolder.GetFileAsync("token.txt");
                String info = await FileIO.ReadTextAsync(config_login);
                JObject data = JObject.Parse(info);
                var token = ((data.SelectToken("token")));
                HttpClient client2 = new HttpClient();
                client2.DefaultRequestHeaders.Add("Authorization", "Basic " + token);
                var resp = client2.GetAsync("http://2-dot-backup-server-002.appspot.com/_api/v2/members/information").Result;
                var responseInfo = await resp.Content.ReadAsStringAsync();
                Member infoUser = JsonConvert.DeserializeObject<Member>(responseInfo);
                if (resp.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    FrameLogin.Navigate(typeof(MainPage));
                    memberOld.avatar = infoUser.avatar;
                    memberOld.address = infoUser.address;
                    memberOld.birthday = infoUser.birthday;
                    memberOld.email = infoUser.email;
                    memberOld.firstName = infoUser.firstName;
                    memberOld.gender = infoUser.gender;
                    memberOld.lastName = infoUser.lastName;
                    memberOld.phone = infoUser.phone;
                    memberOld.id = infoUser.id;
                    LoadingIndicator.IsActive = false;
                }
                else
                {

                }
            } catch(FileNotFoundException)
            {
                
            }
        }
        void setup()
        {
           this.FrameLogin.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(this.BaseUri, "Assets/BackgroundLogin.jpg")), Stretch = Stretch.None };
            ContentValidate.Foreground = new SolidColorBrush(Colors.Red);
           
        }
        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        void validate()
        {

            this.ContentValidate.Text = "";
            var email = Email.Text;
            var password = Password.Password;
            if (email == "" && password == "" )
            {
                this.ContentValidate.Text = "Please enter your infomation !";
                return;
            }
            else
            {
                ContentValidate.Text = "";
            }
            if (email == "")
            {
                this.ContentValidate.Text = "Please enter your email !";
                return;
            }else
            {
                ContentValidate.Text = "";
            }
            if (password == "")
            {
                this.ContentValidate.Text = "Please enter your password !";
                return;
            }
            else
            {
                ContentValidate.Text = "";
            }
        }
        private async void btnLogin(object sender, RoutedEventArgs e)
        {
            validate();
            Dictionary<String, String> loginInfo = new Dictionary<string, string>();
            loginInfo.Add("email", this.Email.Text);
            loginInfo.Add("password", this.Password.Password);
            HttpClient httpClient = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginInfo), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("https://2-dot-backup-server-002.appspot.com/_api/v2/members/authentication", content).Result;

            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                
                // save file...
                Debug.WriteLine(responseContent);
                // Doc token
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

                // Luu token
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.CreateFileAsync("token.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, responseContent);

                // Lay thong tin ca nhan bang token.
                HttpClient client2 = new HttpClient();
                client2.DefaultRequestHeaders.Add("Authorization", "Basic " + token.token);
                var resp = client2.GetAsync("http://2-dot-backup-server-002.appspot.com/_api/v2/members/information").Result;
                var responseInfo = await resp.Content.ReadAsStringAsync();
                Member infoUser = JsonConvert.DeserializeObject<Member>(responseInfo);
                memberOld.avatar = infoUser.avatar;
                memberOld.address = infoUser.address;
                memberOld.birthday = infoUser.birthday;
                memberOld.email = infoUser.email;
                memberOld.firstName = infoUser.firstName;
                memberOld.gender = infoUser.gender;
                memberOld.lastName = infoUser.lastName;
                memberOld.phone = infoUser.phone;
                memberOld.id = infoUser.id;
                var messageDialog = new MessageDialog("Login successful .");
                await messageDialog.ShowAsync();
                FrameLogin.Navigate(typeof(MainPage));
            }
            else
            {
                // Xu ly loi.
                    ErrorResponse errorObject = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                    if (errorObject != null && errorObject.error.Count > 0)
                    {
                        foreach (var key in errorObject.error.Keys)
                        {
                            ContentValidate.Text = "Email or password invalid";
                        }
                    }
            }

        }

        private void btnSignup(object sender, RoutedEventArgs e)
        {
            this.FrameLogin.Navigate(typeof(View.Register));
        }
    }
}
