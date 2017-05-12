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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MemoNow
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async  void  Login_Button_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrWhiteSpace(Login_PasswordBox.Password.ToString()))
            {
                var msg = new ContentDialog
                {
                    Title = "输入密码为空，请重新输入。",
                    PrimaryButtonText = "确定"
                };
                await new MessageDialog("输入密码为空，请重新输入。").ShowAsync();
            }
            else
            {
                using (var conn = ConnectPasswordDatabase.GetPasswordDbConnection())
                {
                    List<Password> query = conn.Query<Password>("select * from Password");
                    
                    string query_password="";
                    foreach(var items in query)
                    {
                        query_password = items.Psw;
                    }
                    if (String.IsNullOrEmpty(query_password))
                    {
                        var dialog = new ContentDialog
                        {
                            Title = "您还没有设置密码，请点击下方设置密码",
                            PrimaryButtonText = "确定"
                        };
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        if(Login_PasswordBox.Password==query_password)
                        {
                            await ShowLoading();
                            Frame.Navigate(typeof(MainPage));
                        }
                        else
                        {
                            var dialog = new ContentDialog
                            {
                                Title = "密码错误，请重新输入。",
                                PrimaryButtonText = "确定"
                            };
                            //await dialog.ShowAsync();
                            await new MessageDialog("密码错误，请重新输入。").ShowAsync();
                        }
                        
                    }
                    
                }
      
            }
            
        }

        private async Task ShowLoading()
        {
            MyContentDialog.ShowAsync();
            await Task.Delay(2000);
            cdtb.Text = "登陆成功";
            await Task.Delay(1000);
            MyContentDialog.Hide();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SetPassword));
        }
    }
}
