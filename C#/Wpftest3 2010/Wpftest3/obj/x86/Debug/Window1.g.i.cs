﻿#pragma checksum "..\..\..\Window1.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3AAA01D27A6A54C069DED2E54A9C92D7"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18408
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Wpftest3 {
    
    
    /// <summary>
    /// Realtime_track_Window
    /// </summary>
    public partial class Realtime_track_Window : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\Window1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.Integration.WindowsFormsHost SceneHost;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Window1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContextMenu scenemenu;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Window1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.Integration.WindowsFormsHost tocHost;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Window1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.Integration.WindowsFormsHost toolbarHost;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\Window1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button1;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Window1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button2;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Window1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button3;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Window1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button4;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\Window1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button5;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\Window1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button6;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Wpftest3;component/window1.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Window1.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 5 "..\..\..\Window1.xaml"
            ((Wpftest3.Realtime_track_Window)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SceneHost = ((System.Windows.Forms.Integration.WindowsFormsHost)(target));
            return;
            case 3:
            this.scenemenu = ((System.Windows.Controls.ContextMenu)(target));
            return;
            case 4:
            
            #line 27 "..\..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MyPosition);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 28 "..\..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.EnterPipe);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 29 "..\..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ExitPipe);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 30 "..\..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.BackToFull);
            
            #line default
            #line hidden
            return;
            case 8:
            this.tocHost = ((System.Windows.Forms.Integration.WindowsFormsHost)(target));
            return;
            case 9:
            this.toolbarHost = ((System.Windows.Forms.Integration.WindowsFormsHost)(target));
            return;
            case 10:
            this.button1 = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\..\Window1.xaml"
            this.button1.Click += new System.Windows.RoutedEventHandler(this.Button1_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.button2 = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\Window1.xaml"
            this.button2.Click += new System.Windows.RoutedEventHandler(this.Button2_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.button3 = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\..\Window1.xaml"
            this.button3.Click += new System.Windows.RoutedEventHandler(this.RealTimeDetect_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.button4 = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\..\Window1.xaml"
            this.button4.Click += new System.Windows.RoutedEventHandler(this.button4_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.button5 = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\..\Window1.xaml"
            this.button5.Click += new System.Windows.RoutedEventHandler(this.button5_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            this.button6 = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\..\Window1.xaml"
            this.button6.Click += new System.Windows.RoutedEventHandler(this.button6_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 49 "..\..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Cable_Setting_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 50 "..\..\..\Window1.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Database_Setting_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 53 "..\..\..\Window1.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Start_Click);
            
            #line default
            #line hidden
            return;
            case 19:
            
            #line 54 "..\..\..\Window1.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Pause_Click);
            
            #line default
            #line hidden
            return;
            case 20:
            
            #line 55 "..\..\..\Window1.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.End_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
