﻿

#pragma checksum "C:\Code\Rememba\Rememba\Rememba.Windows\Rememba.Windows.Windows\Views\MainView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F3D13297DA2AE0B46BC6CC4691110D67"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rememba.Windows.Views
{
    partial class MainView : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 45 "..\..\Views\MainView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.FullscreenEditor_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 46 "..\..\Views\MainView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.NormalEditor_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 65 "..\..\Views\MainView.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.SwitchMode_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 165 "..\..\Views\MainView.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).LostFocus += this.contentView_LostFocus;
                 #line default
                 #line hidden
                #line 165 "..\..\Views\MainView.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).KeyDown += this.contentView_KeyDown;
                 #line default
                 #line hidden
                #line 165 "..\..\Views\MainView.xaml"
                ((global::Windows.UI.Xaml.Controls.WebView)(target)).ScriptNotify += this.contentView_ScriptNotify;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


