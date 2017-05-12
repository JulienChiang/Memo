using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Text;
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
    public sealed partial class AddPage : Page
    {
        public AddPage()
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

        ShareOperation shrOpration = null;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            shrOpration = e.Parameter as ShareOperation;

            if (shrOpration == null)
            {
                return;
            }

            // 获取数据
            if (shrOpration.Data.Contains(StandardDataFormats.Text))
            {
                //var bmpstream = await shrOpration.Data.GetBitmapAsync();
                //BitmapImage bmp = new BitmapImage();
                //bmp.SetSource(await bmpstream.OpenReadAsync());
                //this.img.Source = bmp;
                var jieshou = await shrOpration.Data.GetTextAsync();
                Content_TextBox.Text = jieshou;
            }
        }

        //private void btnItalic_Click_1(object sender, RoutedEventArgs e)
        //{
        //    // 获取选中的文本
        //    ITextSelection selectedText = txtEditor.Document.Selection;
        //    if (selectedText != null)
        //    {
        //        // 实体化一个 ITextCharacterFormat，指定字符格式为斜体
        //        ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
        //        charFormatting.Italic = FormatEffect.Toggle;

        //        // 设置选中文本的字符格式
        //        selectedText.CharacterFormat = charFormatting;
        //    }
        //}

        //// 使选中的文字加粗
        //private void btnBold_Click_1(object sender, RoutedEventArgs e)
        //{
        //    // 获取选中的文本
        //    ITextSelection selectedText = txtEditor.Document.Selection;
        //    if (selectedText != null)
        //    {
        //        // 实体化一个 ITextCharacterFormat，指定字符格式为加粗
        //        ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
        //        charFormatting.Bold = FormatEffect.Toggle;

        //        // 设置选中文本的字符格式
        //        selectedText.CharacterFormat = charFormatting;
        //    }
        //}

        //// 保存已经被高亮的 ITextRange
        //List<ITextRange> _highlightedWords = new List<ITextRange>();
        //// 高亮显示用户搜索的字符
        //private void btnSearch_Click_1(object sender, RoutedEventArgs e)
        //{
        //    // 清除高亮字符的高亮效果
        //    ITextCharacterFormat charFormat;
        //    for (int i = 0; i < _highlightedWords.Count; i++)
        //    {
        //        charFormat = _highlightedWords[i].CharacterFormat;
        //        charFormat.BackgroundColor = Colors.Transparent;
        //        _highlightedWords[i].CharacterFormat = charFormat;
        //    }
        //    _highlightedWords.Clear();

        //    // 获取全部文本，并将操作点移动到文本的起点
        //    ITextRange searchRange = txtEditor.Document.GetRange(0, TextConstants.MaxUnitCount);
        //    searchRange.Move(0, 0);

        //    bool textFound = true;
        //    do
        //    {
        //        // 在全部文本中搜索指定的字符串
        //        if (searchRange.FindText(txtSearch.Text, TextConstants.MaxUnitCount, FindOptions.None) < 1)
        //        {
        //            textFound = false;
        //        }
        //        else
        //        {
        //            _highlightedWords.Add(searchRange.GetClone());

        //            // 实体化一个 ITextCharacterFormat，指定字符背景颜色为黄色
        //            ITextCharacterFormat charFormatting = searchRange.CharacterFormat;
        //            charFormatting.BackgroundColor = Colors.Yellow;

        //            // 设置指定文本的字符格式（高亮效果）
        //            searchRange.CharacterFormat = charFormatting;
        //        }
        //    } while (textFound);
        //}

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

        

        private void DataPicked_Event(DatePickerFlyout sender, DatePickedEventArgs args)
        {
            var Memo_Date = sender.Date.Year.ToString() + "年" + sender.Date.Month.ToString() + "月" + sender.Date.Day.ToString() + "日";
            Calendar_TextBlock.Text = Memo_Date;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrWhiteSpace(Title_TextBox.Text))
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
                    var AddMemo = new Memo();
                    AddMemo.Title = Title_TextBox.Text;
                    AddMemo.Memo_Date = Calendar_TextBlock.Text;
                    AddMemo.Body = Content_TextBox.Text;
                    conn.Insert(AddMemo);
                    shrOpration?.ReportCompleted();
                    if (Frame.CanGoBack)
                    { Frame.GoBack(); }
                }
            }
            
        }


        //private async void Insert_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    FileOpenPicker picker = new FileOpenPicker();
        //    picker.ViewMode = PickerViewMode.Thumbnail;
        //    picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        //    picker.FileTypeFilter.Add(".jpg");
        //    picker.FileTypeFilter.Add(".jpeg");
        //    picker.FileTypeFilter.Add(".png");
        //    picker.FileTypeFilter.Add(".gif");
        //    StorageFile file = await picker.PickSingleFileAsync();
        //    if (file != null)
        //    {
        //        IRandomAccessStream stream = await file?.OpenReadAsync();

        //        //mytb.Document.Selection.Text = "你没选中任何文件";

        //        txtEditor.Document.Selection.InsertImage(200, 100, 0, Windows.UI.Text.VerticalCharacterAlignment.Baseline, "图片", stream);


        //    }
        //}

        //private void Save_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    using (var conn = ConnectMemoDatabase.GetMemoDbConnect())
        //    {

        //    }
        //}
    }
}
