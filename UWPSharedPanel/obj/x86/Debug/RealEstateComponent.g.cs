﻿#pragma checksum "D:\repos\UWPSharedPanel\UWPSharedPanel\RealEstateComponent.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "63A42A137281362B2CED6AC9FB66863E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UWPSharedPanel
{
    partial class RealEstateComponent : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // RealEstateComponent.xaml line 28
                {
                    this.AnalyzeButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.AnalyzeButton).Click += this.AnalyzeButton_Click;
                }
                break;
            case 3: // RealEstateComponent.xaml line 29
                {
                    this.recognitionResult = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 4: // RealEstateComponent.xaml line 30
                {
                    this.outputGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 5: // RealEstateComponent.xaml line 31
                {
                    this.RecognitionCanvas = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 6: // RealEstateComponent.xaml line 32
                {
                    this.InputCanvas = (global::Windows.UI.Xaml.Controls.InkCanvas)(target);
                }
                break;
            case 7: // RealEstateComponent.xaml line 22
                {
                    this.Home = (global::Windows.UI.Xaml.Controls.RatingControl)(target);
                }
                break;
            case 8: // RealEstateComponent.xaml line 23
                {
                    this.Location = (global::Windows.UI.Xaml.Controls.RatingControl)(target);
                }
                break;
            case 9: // RealEstateComponent.xaml line 24
                {
                    this.Convenience = (global::Windows.UI.Xaml.Controls.RatingControl)(target);
                }
                break;
            case 10: // RealEstateComponent.xaml line 25
                {
                    this.Total = (global::Windows.UI.Xaml.Controls.RatingControl)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
