using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MemoNow
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
            using (var conn = ConnectMemoDatabase.GetMemoDbConnect())
            {
                List<Memo> Memolist = conn.Query<Memo>("select * from Memo");
                List<string> r = new List<string>();
                foreach (var items in Memolist)
                {
                    

                    r.Add(items.Title);
                    

                }

                Searchbox.ItemsSource = r;
            }
        }

        private async void StatusBar_Toggled(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleSwitch;
            if (toggle.IsOn)
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                await statusBar.HideAsync();
            }
            else
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                await statusBar.ShowAsync();
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void Searchbox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            string info = args.SelectedItem as string;
            Frame.Navigate(typeof(EditPage), info);
        }

        private void Searchbox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HomePage.IsSelected)
            {
                Frame.Navigate(typeof(MainPage));
            }
            if (About.IsSelected)
            {
                Frame.Navigate(typeof(AboutPage));
            }
            
            if (Feedback.IsSelected)
            {
                Uri uri = new Uri("mailto: juliencheung @outlook.com");
                var promptOptions = new Windows.System.LauncherOptions();
                //promptOptions.TreatAsUntrusted = true;
                promptOptions.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseLess;
                var success = await Windows.System.Launcher.LaunchUriAsync(uri, promptOptions);
            }
        }
    }
}
