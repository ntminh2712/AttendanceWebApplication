using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MusicBox
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.MainFrame.Navigate(typeof(View.Home));
            String linkDefautlPlaying = "https://media2.fishtank.my/app_themes/mix/assets/images/default-album-art.png";
            this.ThumnailPlaying.Source = new BitmapImage(new Uri(linkDefautlPlaying, UriKind.Absolute));
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public async void checkToken()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile fileToken = await storageFolder.GetFileAsync("token.txt");
        }
        private void btnMenu(object sender, RoutedEventArgs e)
        {
            this.SplitMenu.IsPaneOpen = !this.SplitMenu.IsPaneOpen;
        }

        private void chooseMenu(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            switch (radio.Tag.ToString())
            {
                case "Recently":
                    this.SplitMenu.IsPaneOpen = false;
                    this.MainFrame.Navigate(typeof(View.Recently));
                    break;
                case "Home":
                    this.SplitMenu.IsPaneOpen = false;
                    this.MainFrame.Navigate(typeof(View.Home));
                    break;
                case "Account":
                    this.SplitMenu.IsPaneOpen = false;
                    this.MainFrame.Navigate(typeof(View.Account));
                    break;
                case "Playlist":
                    this.SplitMenu.IsPaneOpen = false;
                    this.MainFrame.Navigate(typeof(View.Playlist));
                    break;
                default:
                    break;

            }

        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private async void Logout(object sender, RoutedEventArgs e)
        {
            LogoutUser();
        }
        private async void LogoutUser()
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog("Do you want to logout ?.");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(
                "NO",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));
            messageDialog.Commands.Add(new UICommand(
                "Yes",
                new UICommandInvokedHandler(handlerLogoutAsync)));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }
        private async void handlerLogoutAsync(IUICommand command)
        {
            StorageFile deleteCurrentToken = await ApplicationData.Current.LocalFolder.GetFileAsync("token.txt");
            
            await deleteCurrentToken.DeleteAsync();
            this.Main.Navigate(typeof(View.Login));
            
        }
        private void CommandInvokedHandler(IUICommand command) {
           
        }
    }
}
