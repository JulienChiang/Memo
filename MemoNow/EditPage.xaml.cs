using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class EditPage : Page
    {
        public EditPage()
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

        public string Receive_Title;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Receive_Title = e.Parameter.ToString();
            Title_TextBox.Text = e.Parameter.ToString();
            using (var conn = ConnectMemoDatabase.GetMemoDbConnect())
            {
                List<Memo> Memolist = conn.Query<Memo>("select * from Memo where Title = ?",e.Parameter.ToString());
                foreach (var items in Memolist)
                {
                    Calendar_TextBlock.Text = items.Memo_Date;
                    Content_TextBox.Text = items.Body;
                }
                
            }

        }

        private void DataPicked_Event(DatePickerFlyout sender, DatePickedEventArgs args)
        {
            var Memo_Date = sender.Date.Year.ToString() + "年" + sender.Date.Month.ToString() + "月" + sender.Date.Day.ToString() + "日";
            Calendar_TextBlock.Text = Memo_Date;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Title_TextBox.Text))
            {
                var dialog = new ContentDialog
                {
                    Title = "请输入标题",
                    PrimaryButtonText = "确定"
                };
                await dialog.ShowAsync();
            }
            else
            {
                using (var conn = ConnectMemoDatabase.GetMemoDbConnect())
                {
                    conn.Execute("update Memo set Title = ?, Memo_Date = ?,Body = ? where Title= ?",Title_TextBox.Text, Calendar_TextBlock.Text,Content_TextBox.Text,Receive_Title);
                    
                }
                var dialog = new ContentDialog
                {
                    Content = "更新成功",
                    
                    IsPrimaryButtonEnabled = true
                };
                dialog.ShowAsync();

                await Task.Delay(1000);
                dialog.Hide();
                if (Frame.CanGoBack)
                { Frame.GoBack(); }
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

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HomePage.IsSelected)
            {
                Frame.Navigate(typeof(MainPage));
            }
            if (About.IsSelected)
            {
                Frame.Navigate(typeof(AboutPage));
            }
            if (Setting.IsSelected)
            {
                Frame.Navigate(typeof(SettingPage));
            }
            if (Feedback.IsSelected)
            {
                Uri uri = new Uri("mailto: juliencheung @outlook.com");
                var promptOptions = new Windows.System.LauncherOptions();
                //promptOptions.TreatAsUntrusted = true;
                promptOptions.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseLess;
                var success = Windows.System.Launcher.LaunchUriAsync(uri, promptOptions);
            }
        }

        

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = ConnectMemoDatabase.GetMemoDbConnect())
            {
                conn.Execute("delete from Memo where Title = ?", Receive_Title);
            }
            if (Frame.CanGoBack)
            { Frame.GoBack(); }
        }

        

        private async  void Share1_Click(object sender, RoutedEventArgs e)
        {
            var url = string.Format("mailto:?body={0}", Content_TextBox.Text);
            var uriBing = new Uri(url);
            var promptOptions = new Windows.System.LauncherOptions();
            
            //promptOptions.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseLess;
            var success =await  Windows.System.Launcher.LaunchUriAsync(uriBing);
        }

        private  void Share2_Click(object sender, RoutedEventArgs e)
        {
            var url = string.Format("ms-chat:?Body={0}", Content_TextBox.Text);
            var uriBing = new Uri(url);
            var promptOptions = new Windows.System.LauncherOptions();
            
            //promptOptions.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseLess;
            var success =  Windows.System.Launcher.LaunchUriAsync(uriBing);
        }
    }
}
