﻿#pragma checksum "C:\DIY-Platformer\PhoneApp4v3\PhoneApp4\PhoneApp4\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "78238607D2119D255845C777CA07C8C4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace PhoneApp4 {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Button btnStart;
        
        internal System.Windows.Controls.Button btnCreate;
        
        internal System.Windows.Controls.Button btnSelect;
        
        internal System.Windows.Controls.TextBox txtBoxLevel;
        
        internal System.Windows.Controls.TextBlock txtLevel;
        
        internal System.Windows.Controls.TextBlock txtCreator;
        
        internal System.Windows.Controls.Button btnHelp;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/PhoneApp4;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.btnStart = ((System.Windows.Controls.Button)(this.FindName("btnStart")));
            this.btnCreate = ((System.Windows.Controls.Button)(this.FindName("btnCreate")));
            this.btnSelect = ((System.Windows.Controls.Button)(this.FindName("btnSelect")));
            this.txtBoxLevel = ((System.Windows.Controls.TextBox)(this.FindName("txtBoxLevel")));
            this.txtLevel = ((System.Windows.Controls.TextBlock)(this.FindName("txtLevel")));
            this.txtCreator = ((System.Windows.Controls.TextBlock)(this.FindName("txtCreator")));
            this.btnHelp = ((System.Windows.Controls.Button)(this.FindName("btnHelp")));
        }
    }
}

