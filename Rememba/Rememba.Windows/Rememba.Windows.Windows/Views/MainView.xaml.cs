using Rememba.Contracts.ViewModels;
using Rememba.Contracts.Views;
using Rememba.ViewModels.Windows;
using Rememba.Windows.Extensions;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rememba.Windows.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Page, IMainView
    {
        public MainView()
        {
            this.InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get { return this.DataContext as IViewModel; }
        }

        private void contentView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            (this.DataContext as MainViewViewModel).UpdateContentFromWebView(e.Value);
        }

        private void SwitchMode_Click(object sender, RoutedEventArgs e)
        {
            if (switchModeButton.Content.ToString() == "Edit")
            {
                switchModeButton.Content = "Preview";
                contentView.InvokeScriptAsync("Edit", null);
            }
            else
            {
                switchModeButton.Content = "Edit";
                contentView.InvokeScriptAsync("Preview", null);
            }
        }

        private void contentView_KeyDown(object sender, KeyRoutedEventArgs e)
        {

        }

        private async void contentView_LostFocus(object sender, RoutedEventArgs e)
        {
            //var content = await contentView.InvokeScriptAsync("GetContent", null);
            //(this.DataContext as MainViewViewModel).UpdateContentFromWebView(content);
        }
    }
}
