using Rememba.Contracts.ViewModels;
using Rememba.ViewModels.Windows;
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
    public sealed partial class MainView : Page
    {
        public MainView()
        {
            this.InitializeComponent();
            this.SizeChanged += MainView_SizeChanged;
        }

        void MainView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 500)
            {
                VisualStateManager.GoToState(this, "MinimalLayout", true);
            }
            //else if (e.NewSize.Width < e.NewSize.Height)
            //{
            //    VisualStateManager.GoToState(this, "PortraitLayout", true);
            //}
            else
            {
                VisualStateManager.GoToState(this, "DefaultLayout", true);
            }
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
            if (switchModeButton.Label == "Edit")
            {
                switchModeButton.Label = "Preview";
                contentView.InvokeScriptAsync("Edit", null);
            }
            else
            {
                switchModeButton.Label = "Edit";
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

        private void FullscreenEditor_Click(object sender, RoutedEventArgs e)
        {
            parentListView.Visibility = global::Windows.UI.Xaml.Visibility.Collapsed;
            childListView.Visibility = global::Windows.UI.Xaml.Visibility.Collapsed;
            subChildListView.Visibility = global::Windows.UI.Xaml.Visibility.Collapsed;
            contentView.SetValue(Grid.ColumnProperty, 0);
            contentView.SetValue(Grid.ColumnSpanProperty, 4);

        }
        private void NormalEditor_Click(object sender, RoutedEventArgs e)
        {
            parentListView.Visibility = global::Windows.UI.Xaml.Visibility.Visible;
            childListView.Visibility = global::Windows.UI.Xaml.Visibility.Visible;
            subChildListView.Visibility = global::Windows.UI.Xaml.Visibility.Visible;
            contentView.SetValue(Grid.ColumnProperty, 3);
            contentView.SetValue(Grid.ColumnSpanProperty, 1);
        }
    }
}
