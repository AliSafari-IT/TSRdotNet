﻿#pragma checksum "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "A8EFDE0BEC1D41D9B9DB836701657203B6743E6FEB386758E2231C6EF422D83F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
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
using TSR.Pages.CSV;


namespace TSR.Pages.CSV {
    
    
    /// <summary>
    /// BrowseCSV_Page
    /// </summary>
    public partial class BrowseCSV_Page : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 136 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Path next_open_csv;
        
        #line default
        #line hidden
        
        
        #line 148 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBox_Browse;
        
        #line default
        #line hidden
        
        
        #line 193 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Path next_list_delimiter;
        
        #line default
        #line hidden
        
        
        #line 203 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel delimiterSPanel;
        
        #line default
        #line hidden
        
        
        #line 225 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton commaDelimiter;
        
        #line default
        #line hidden
        
        
        #line 235 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton semicolonDelimiter;
        
        #line default
        #line hidden
        
        
        #line 245 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton tabDelimiter;
        
        #line default
        #line hidden
        
        
        #line 254 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton spaceDelimiter;
        
        #line default
        #line hidden
        
        
        #line 268 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox OtherDelimiter;
        
        #line default
        #line hidden
        
        
        #line 285 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button importCSVFile;
        
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
            System.Uri resourceLocater = new System.Uri("/TSR;component/pages/csv/browsecsv_page.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
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
            this.next_open_csv = ((System.Windows.Shapes.Path)(target));
            return;
            case 2:
            this.TextBox_Browse = ((System.Windows.Controls.TextBox)(target));
            
            #line 159 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
            this.TextBox_Browse.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.TextBox_Browse_PreviewMouseDown);
            
            #line default
            #line hidden
            
            #line 162 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
            this.TextBox_Browse.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBox_Browse_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.next_list_delimiter = ((System.Windows.Shapes.Path)(target));
            return;
            case 4:
            this.delimiterSPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 5:
            this.commaDelimiter = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 6:
            this.semicolonDelimiter = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 7:
            this.tabDelimiter = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 8:
            this.spaceDelimiter = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 9:
            this.OtherDelimiter = ((System.Windows.Controls.TextBox)(target));
            
            #line 273 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
            this.OtherDelimiter.LostFocus += new System.Windows.RoutedEventHandler(this.OtherDelimiter_LostFocus);
            
            #line default
            #line hidden
            return;
            case 10:
            this.importCSVFile = ((System.Windows.Controls.Button)(target));
            
            #line 290 "..\..\..\..\Pages\CSV\BrowseCSV_Page.xaml"
            this.importCSVFile.Click += new System.Windows.RoutedEventHandler(this.importCSVFile_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

