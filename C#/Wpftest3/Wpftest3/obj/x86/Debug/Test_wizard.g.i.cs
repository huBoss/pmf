﻿#pragma checksum "..\..\..\Test_wizard.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "ED3AA40AFCD352E14C6CD633D5BE99A3"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18408
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using ActiproSoftware.Windows.Controls.Wizard;
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
    /// Test_wizard
    /// </summary>
    public partial class Test_wizard : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\Test_wizard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton Ishand;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\Test_wizard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel Panel_nohand;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\Test_wizard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox isUse_p;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Test_wizard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox isUse_d;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Test_wizard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox pnum;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Test_wizard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox dnum;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\Test_wizard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Linetype;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\Test_wizard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox Remain_point;
        
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
            System.Uri resourceLocater = new System.Uri("/Wpftest3;component/test_wizard.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Test_wizard.xaml"
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
            this.Ishand = ((System.Windows.Controls.RadioButton)(target));
            
            #line 16 "..\..\..\Test_wizard.xaml"
            this.Ishand.Checked += new System.Windows.RoutedEventHandler(this.RadioButton_Checked);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 17 "..\..\..\Test_wizard.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.RadioButton_Checked_1);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Panel_nohand = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 4:
            this.isUse_p = ((System.Windows.Controls.CheckBox)(target));
            
            #line 19 "..\..\..\Test_wizard.xaml"
            this.isUse_p.Checked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.isUse_d = ((System.Windows.Controls.CheckBox)(target));
            
            #line 20 "..\..\..\Test_wizard.xaml"
            this.isUse_d.Checked += new System.Windows.RoutedEventHandler(this.isUse_d_Checked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.pnum = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.dnum = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.Linetype = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 9:
            this.Remain_point = ((System.Windows.Controls.CheckBox)(target));
            
            #line 33 "..\..\..\Test_wizard.xaml"
            this.Remain_point.Checked += new System.Windows.RoutedEventHandler(this.Remain_point_Checked);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 35 "..\..\..\Test_wizard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 36 "..\..\..\Test_wizard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_2);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 37 "..\..\..\Test_wizard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
