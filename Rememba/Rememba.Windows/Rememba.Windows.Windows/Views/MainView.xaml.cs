using Rememba.Contracts.ViewModels;
using Rememba.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
            SwitchMode();
        }

        private void SwitchMode()
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

        bool isCtrlKeyPressed = false;
        //bool isShiftKeyPressed = false;

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Control) isCtrlKeyPressed = true;
            else if (isCtrlKeyPressed)
            {
                switch (e.Key)
                {
                    case VirtualKey.A:
                        (this.DataContext as MainViewViewModel).AddChildNodeCommand.Execute(null);
                        break;
                    case VirtualKey.S: (this.DataContext as MainViewViewModel).AddSiblingNodeCommand.Execute(null); break;
                    case VirtualKey.Delete: (this.DataContext as MainViewViewModel).DeleteNodeCommand.Execute(null); break;
                    case VirtualKey.Down: (this.DataContext as MainViewViewModel).MoveDownOrder.Execute(null); break;
                    case VirtualKey.Up: (this.DataContext as MainViewViewModel).MoveUpOrder.Execute(null); break;
                    //case VirtualKey.U: (this.DataContext as MainViewViewModel).GoUpTree.Execute(null); break;

                    case VirtualKey.E: SwitchMode(); break;
                    case VirtualKey.N: (this.DataContext as MainViewViewModel).EditNodeCommand.Execute(null); break;
                    //case VirtualKey.C: (this.DataContext as MainViewViewModel).Copy.Execute(null); break;
                    //case VirtualKey.V: (this.DataContext as MainViewViewModel).PasteSibling.Execute(null); break;
                    //case VirtualKey.L:
                    //    (this.DataContext as MainViewViewModel).LoadGraphCommand.Execute(null);
                    //    break;
                    //case VirtualKey.X: (this.DataContext as MainViewViewModel).DeleteGraphCommand.Execute(null); break;

                }
            }

            // isCtrlKeyPressed =  (e.Key == VirtualKey.Control)   ;         //if (e.Key == VirtualKey.Shift) isShiftKeyPressed = true;

            // else if (isCtrlKeyPressed)
            // {
            //     Debug.WriteLine(e.Key + " : " + isCtrlKeyPressed );
            //     switch (e.Key)
            //     {
            //         case VirtualKey.A:
            //             (this.DataContext as MainViewViewModel).AddChildNodeCommand.Execute(null);
            //             break;
            //         case VirtualKey.S: (this.DataContext as MainViewViewModel).AddSiblingNodeCommand.Execute(null); break;
            //         case VirtualKey.Delete: (this.DataContext as MainViewViewModel).DeleteNodeCommand.Execute(null); break;
            //         case VirtualKey.Down: (this.DataContext as MainViewViewModel).MoveDownOrder.Execute(null); break;
            //         case VirtualKey.Up: (this.DataContext as MainViewViewModel).MoveUpOrder.Execute(null); break;
            //         case VirtualKey.U: (this.DataContext as MainViewViewModel).GoUpTree.Execute(null); break;

            //         case VirtualKey.E: (this.DataContext as MainViewViewModel).EditContent.Execute(null); break;
            //         case VirtualKey.N: (this.DataContext as MainViewViewModel).EditNodeCommand.Execute(null); break;
            //         case VirtualKey.C: (this.DataContext as MainViewViewModel).Copy.Execute(null); break;
            //         case VirtualKey.V: (this.DataContext as MainViewViewModel).PasteSibling.Execute(null); break;
            //         case VirtualKey.L:
            //             (this.DataContext as MainViewViewModel).LoadGraphCommand.Execute(null);
            //             break;


            //     }
            //     //isCtrlKeyPressed = false;
            //     //isShiftKeyPressed = false;

            // }
            // //else if (isCtrlKeyPressed && isShiftKeyPressed)
            // //{
            // //    Debug.WriteLine(e.Key + " : " + isCtrlKeyPressed + " : " + isShiftKeyPressed);
            // //    switch (e.Key)
            // //    {
            // //        case VirtualKey.V: (this.DataContext as MainViewViewModel).PasteChild.Execute(null); break;
            // //        case VirtualKey.S: (this.DataContext as MainViewViewModel).Save.Execute(null); break;
            // //        case VirtualKey.L:
            // //            (this.DataContext as MainViewViewModel).LoadGraphCommand.Execute(null);
            // //            break;
            // //        case VirtualKey.C: (this.DataContext as MainViewViewModel).CreateGraphCommand.Execute(null); break;
            // //        case VirtualKey.Delete: (this.DataContext as MainViewViewModel).DeleteContent.Execute(null); break;
            // //        case VirtualKey.X: (this.DataContext as MainViewViewModel).DeleteGraphCommand.Execute(null); break;
            // //        case VirtualKey.U: (this.DataContext as MainViewViewModel).UpdateContent.Execute(null); break;
            // //    }

            // //    isCtrlKeyPressed = false;
            // //    isShiftKeyPressed = false;
            // //}

            //// isCtrlKeyPressed = false;
        }

        private void Page_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Control) isCtrlKeyPressed = false;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 1) return;

            (this.DataContext as MainViewViewModel).SelectParent(e.AddedItems[0]);

        }
    }
}
