using MusicBox.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicBox.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {

        private static String API_REGISTER = "https://2-dot-backup-server-002.appspot.com/_api/v2/members";
        private Member currentMember;
        private static StorageFile file;
        private static string UploadUrl;

        public Register()
        {
            currentMember = new Member();
            this.InitializeComponent();
            GetUploadUrl();
            setup();

        }
        public void setup()
        {
            var linkDefaultAvatar = "https://i1.wp.com/azurevn.net/wp-content/uploads/2016/07/765-default-avatar.png?fit=300%2C300";
            this.ImgAvatar.Source = new BitmapImage(new Uri(linkDefaultAvatar, UriKind.Absolute));
            Avatar.Text = linkDefaultAvatar;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnLogin(object sender, RoutedEventArgs e)
        {
            this.FrameSignin.Navigate(typeof(View.Login));
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ScrollViewer_ViewChanged()
        {

        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

        }

        private void LastName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public async void validate()
        {
            Dictionary<String, String> RegisterInfor = new Dictionary<string, string>();
            RegisterInfor.Add("email", this.Email.Text);
            RegisterInfor.Add("firstName", this.FirstName.Text);
            RegisterInfor.Add("lastName", this.LastName.Text);
            HttpClient httpClient = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(RegisterInfor), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(API_REGISTER, content).Result;
            

                if (this.Email.Text == "")
            {
                this.email.Text = "Please, enter your email !";
            }
            else
            {
                this.email.Text = "";
            }
            if (Password.Password.ToString() == "")
            {
                this.password.Text = "Please, enter your password !";
            }
            else
            {
                this.email.Text = "";
            }
            if (this.FirstName.Text == "")
            {
                this.firstName.Text = "Please, enter your firstName !";
            }
            else
            {
                this.firstName.Text = "";
            }
            if (this.LastName.Text == "")
            {
                this.lastName.Text = "Please, enter your lastName !";
            }
            else
            {
                this.lastName.Text = "";
            }
            if (this.Avatar.Text == "")
            {
                this.avatar.Text = "Please, take your photo !";
            }
            else
            {
                this.avatar.Text = "";
            }
            if (this.Phone.Text == "")
            {
                this.phone.Text = "Please, enter your phone !";
            }
            else
            {
                this.phone.Text = "";
            }
            if (this.Address.Text == "")
            {
                this.address.Text = "Please, enter your address !";
            }
            else
            {
                this.address.Text = "";
            }


        }
        private async void Capture_Avatar(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);
            file = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (file == null)
            {
                // User cancelled photo capture
                return;
            }
            HttpUploadFile(UploadUrl, "myFile", "image/png");
        }
        private static async void GetUploadUrl()
        {
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            Uri requestUri = new Uri("https://2-dot-backup-server-002.appspot.com/get-upload-token");
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";
            try
            {
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
            Debug.WriteLine(httpResponseBody);
            UploadUrl = httpResponseBody;
        }
        public async void HttpUploadFile(string url, string paramName, string contentType)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            Debug.WriteLine(url);
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";

            Stream rs = await wr.GetRequestStreamAsync();
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", paramName, "path_file", contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            // write file.
            Stream fileStream = await file.OpenStreamForReadAsync();
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);

            WebResponse wresp = null;
            try
            {
                wresp = await wr.GetResponseAsync();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                //Debug.WriteLine(string.Format("File uploaded, server response is: @{0}@", reader2.ReadToEnd()));
                //string imgUrl = reader2.ReadToEnd();
                Uri u = new Uri(reader2.ReadToEnd(), UriKind.Absolute);
                Debug.WriteLine(u.AbsoluteUri);
                Avatar.Text = u.AbsoluteUri;
                this.ImgAvatar.Source = new BitmapImage(u);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error uploading file", ex.StackTrace);
                Debug.WriteLine("Error uploading file", ex.InnerException);
                if (wresp != null)
                {
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }
        private void Select_Gender(object sender, RoutedEventArgs e)
        {
            RadioButton radioGender = sender as RadioButton;
            this.currentMember.gender = Int32.Parse(radioGender.Tag.ToString());
            Debug.WriteLine(this.currentMember.gender);
        }

        private void Change_Birthday(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            this.currentMember.birthday = sender.Date.Value.ToString("yyyy-MM-dd");
        }

        private async void btnSignupMember(object sender, RoutedEventArgs e)
        {
            
            this.currentMember.firstName = this.FirstName.Text;
            this.currentMember.lastName = this.LastName.Text;
            this.currentMember.email = this.Email.Text;
            this.currentMember.password = this.Password.Password;
            this.currentMember.avatar = this.Avatar.Text;
            this.currentMember.phone = this.Phone.Text;
            this.currentMember.address = this.Address.Text;
            validate();
            HttpClient httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(currentMember), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(API_REGISTER, content).Result;
            var responseContent = await response.Content.ReadAsStringAsync();

            ErrorResponse errorObject = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
            if (errorObject != null && errorObject.error.Count > 0)
            {
                foreach (var key in errorObject.error.Keys)
                {
                    var textMessage = this.FindName(key);
                    if (textMessage == null)
                    {
                        continue;
                    }
                    TextBlock textBlock = textMessage as TextBlock;
                    textBlock.Text = errorObject.error[key];
                    textBlock.Visibility = Visibility.Visible;
                }
            }

        }

        private async void ChooseAvatar(object sender, RoutedEventArgs e)
        { FileOpenPicker fileOpenPicker = new FileOpenPicker();

            // Set SuggestedStartLocation
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            // Set ViewMode
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;

            // Filter for file types. For example, if you want to open text files,
            // you will add .txt to the list.

            fileOpenPicker.FileTypeFilter.Clear();
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.FileTypeFilter.Add(".jpeg");
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".bmp");

            // Open FileOpenPicker
            StorageFile fileLocal = await fileOpenPicker.PickSingleFileAsync();
            if (fileLocal != null)
            {
                // Open a stream
                Windows.Storage.Streams.IRandomAccessStream fileStream =
                await fileLocal.OpenAsync(FileAccessMode.Read);

                // Create a BitmapImage and Set stream as source
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(fileStream);
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(UploadUrl);
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";

                Stream rs = await wr.GetRequestStreamAsync();
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", "myFile", "path_file","image / png");
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);
                Stream fileStreamLocal = await fileLocal.OpenStreamForReadAsync();
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                while ((bytesRead = fileStreamLocal.Read(buffer, 0, buffer.Length)) != 0)
                {
                    rs.Write(buffer, 0, bytesRead);
                }

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);

                WebResponse wresp = null;
                try
                {
                    wresp = await wr.GetResponseAsync();
                    Stream stream2 = wresp.GetResponseStream();
                    StreamReader reader2 = new StreamReader(stream2);
                    //Debug.WriteLine(string.Format("File uploaded, server response is: @{0}@", reader2.ReadToEnd()));
                    //string imgUrl = reader2.ReadToEnd();
                    Uri u = new Uri(reader2.ReadToEnd(), UriKind.Absolute);
                    Debug.WriteLine(u.AbsoluteUri);
                    Avatar.Text = u.AbsoluteUri;
                    this.ImgAvatar.Source = new BitmapImage(u);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error uploading file", ex.StackTrace);
                    Debug.WriteLine("Error uploading file", ex.InnerException);
                    if (wresp != null)
                    {
                        wresp = null;
                    }
                }
                finally
                {
                    wr = null;
                }
            }
        }
    }

}
