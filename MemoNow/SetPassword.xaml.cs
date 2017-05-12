using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Text.RegularExpressions;
using Windows.UI.Popups;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MemoNow
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SetPassword : Page
    {
        public SetPassword()
        {
            this.InitializeComponent();
        }

        private async void Setting_Button_Click(object sender, RoutedEventArgs e)
        {

            using (var conn = ConnectPasswordDatabase.GetPasswordDbConnection())
            {

                List<Password> query = conn.Query<Password>("select * from Password");

                string query_password = "";
                foreach (var items in query)
                {
                    query_password = items.Psw;
                }
                if (String.IsNullOrEmpty(query_password))
                {
                    if (New_PasswordBox.Password != "" && (New_PasswordBox.Password.Length >= 6 && New_PasswordBox.Password.Length <= 8))
                    {
                        var addPassword = new Password();

                        addPassword.Psw = New_PasswordBox.Password;
                        conn.Insert(addPassword);
                        var dialog = new ContentDialog
                        {
                            Title = "密码设置成功",
                            PrimaryButtonText = "确定"
                        };
                        await new MessageDialog("密码设置成功").ShowAsync();
                        if (Frame.CanGoBack)
                        { Frame.GoBack(); }
                    }

                    else if (New_PasswordBox.Password == "")
                    {
                        var dialog = new ContentDialog
                        {
                            Title = "新密码不能为空，请重新输入。",
                            PrimaryButtonText = "确定"
                        };
                        await new MessageDialog("新密码不能为空，请重新输入。").ShowAsync();
                    }

                    else
                    {
                        var dialog = new ContentDialog
                        {
                            Title = "请输入6-8位的数字密码字符组合",
                            PrimaryButtonText = "确定"
                        };
                        await new MessageDialog("请输入6-8位的数字密码字符组合").ShowAsync();
                    }


                }

                else
                {
                    if (query_password == Old_PasswordBox.Password && Old_PasswordBox.Password != "" && New_PasswordBox.Password != "" && (New_PasswordBox.Password.Length >= 6 && New_PasswordBox.Password.Length <= 8) && (Old_PasswordBox.Password.Length >= 6 && Old_PasswordBox.Password.Length <= 8))
                    {
                        conn.Execute("update Password set Psw = ? where Psw = ?", New_PasswordBox.Password, Old_PasswordBox.Password);
                        var dialog = new ContentDialog
                        {
                            Title = "密码修改成功",
                            PrimaryButtonText = "确定"
                        };
                        await new MessageDialog("密码修改成功").ShowAsync();
                        if (Frame.CanGoBack)
                        { Frame.GoBack(); }
                    }

                    if (New_PasswordBox.Password == "" || Old_PasswordBox.Password == "")
                    {
                        var dialog = new ContentDialog
                        {
                            Title = "密码不能为空，请重新输入。",
                            PrimaryButtonText = "确定"
                        };
                        await new MessageDialog("密码不能为空，请重新输入。").ShowAsync();
                    }

                    if (((New_PasswordBox.Password.Length < 6 || New_PasswordBox.Password.Length > 8) || (Old_PasswordBox.Password.Length < 6 || Old_PasswordBox.Password.Length > 8)) && Old_PasswordBox.Password != "" && New_PasswordBox.Password != "")
                    {
                        var dialog = new ContentDialog
                        {
                            Title = "请输入6-8位的数字密码字符组合",
                            PrimaryButtonText = "确定"
                        };
                        await new MessageDialog("请输入6-8位的数字密码字符组合").ShowAsync();
                    }

                    if (Old_PasswordBox.Password != query_password && Old_PasswordBox.Password != "" && New_PasswordBox.Password != "" && (New_PasswordBox.Password.Length >= 6 && New_PasswordBox.Password.Length <= 8) && (Old_PasswordBox.Password.Length >= 6 && Old_PasswordBox.Password.Length <= 8))
                    {
                        var dialog = new ContentDialog
                        {
                            Title = "原密码错误，请重新输入。",
                            PrimaryButtonText = "确定"
                        };
                        await new MessageDialog("原密码错误，请重新输入。").ShowAsync();
                    }







                }

                //conn.DeleteAll<Password>();
            }
        }
    }
}
