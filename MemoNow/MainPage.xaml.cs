using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MemoNow
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            using (var conn = ConnectMemoDatabase.GetMemoDbConnect())
            {
                List<Memo> Memolist = conn.Query<Memo>("select * from Memo order by Number desc");
                List<string> r = new List<string>();
                foreach (var items in Memolist)
                {
                    Memo_List.Items.Add(items.Title);
                    
                    r.Add(items.Title);
                    Memo_List.SelectionMode = SelectionMode.Single;
                    
                }

                Searchbox.ItemsSource = r;
            }

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddPage));
        }

        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            if(Memo_List.SelectedIndex==-1)
            {
                var dialog = new ContentDialog
                {
                    Title = "未选中日记",
                    PrimaryButtonText = "确定"
                };
                await dialog.ShowAsync();
            }
            else
            {
                string info = Memo_List.SelectedValue.ToString();
                Frame.Navigate(typeof(EditPage), info);
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = ConnectMemoDatabase.GetMemoDbConnect())
            {
                if (Memo_List.SelectedIndex == -1)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "未选中日记",
                        PrimaryButtonText = "确定"
                    };
                    await dialog.ShowAsync();
                }
                else
                {
                    string xxx = Memo_List.SelectedIndex.ToString();
                    string xx = Memo_List.SelectedValue.ToString();
                    for(int i = Memo_List.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        Memo_List.Items.Remove(Memo_List.SelectedItems[i]);
                    }
                    var msg=String.Format("第{0}行已被删除",int.Parse(xxx) + 1);
                    var dialog = new ContentDialog
                    {
                        Title = "提示",
                        Content=msg,
                        IsPrimaryButtonEnabled = true
                    };
                    dialog.ShowAsync();
                    
                    await Task.Delay(1000);
                    dialog.Hide();
                    conn.Execute("delete from Memo where Title = ?", xx);
                }
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
            //if (HomePage.IsSelected)
            //{
            //    Frame.Navigate(typeof(MainPage));
            //}
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
                var success = await Windows.System.Launcher.LaunchUriAsync(uri, promptOptions);
            }
        }

        private void MySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string info = Memo_List.SelectedValue.ToString();
            Frame.Navigate(typeof(EditPage), info);
        }
    }
}
