﻿#pragma checksum "..\..\..\..\Pages\InputFormula.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "BC8B49DFD1AB86750DE49A24212CB8445DA0C144"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ArchimedeFront.Pages;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace ArchimedeFront.Pages {
    
    
    /// <summary>
    /// InputFormula
    /// </summary>
    public partial class InputFormula : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 33 "..\..\..\..\Pages\InputFormula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox expression;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\Pages\InputFormula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button et;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\Pages\InputFormula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ou;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\Pages\InputFormula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button non;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\Pages\InputFormula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button nand;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Pages\InputFormula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button nor;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\Pages\InputFormula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button xor;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\Pages\InputFormula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button xnor;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\Pages\InputFormula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button paranthese;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\Pages\InputFormula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button simplifyButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.4.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ArchimedeFront;component/pages/inputformula.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\InputFormula.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.4.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.expression = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.et = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\..\Pages\InputFormula.xaml"
            this.et.Click += new System.Windows.RoutedEventHandler(this.operator_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ou = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\..\..\Pages\InputFormula.xaml"
            this.ou.Click += new System.Windows.RoutedEventHandler(this.operator_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.non = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\..\..\Pages\InputFormula.xaml"
            this.non.Click += new System.Windows.RoutedEventHandler(this.operator_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.nand = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\..\..\Pages\InputFormula.xaml"
            this.nand.Click += new System.Windows.RoutedEventHandler(this.operator_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.nor = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\..\..\Pages\InputFormula.xaml"
            this.nor.Click += new System.Windows.RoutedEventHandler(this.operator_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.xor = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\..\..\Pages\InputFormula.xaml"
            this.xor.Click += new System.Windows.RoutedEventHandler(this.operator_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.xnor = ((System.Windows.Controls.Button)(target));
            
            #line 45 "..\..\..\..\Pages\InputFormula.xaml"
            this.xnor.Click += new System.Windows.RoutedEventHandler(this.operator_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.paranthese = ((System.Windows.Controls.Button)(target));
            
            #line 46 "..\..\..\..\Pages\InputFormula.xaml"
            this.paranthese.Click += new System.Windows.RoutedEventHandler(this.operator_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.simplifyButton = ((System.Windows.Controls.Button)(target));
            
            #line 51 "..\..\..\..\Pages\InputFormula.xaml"
            this.simplifyButton.Click += new System.Windows.RoutedEventHandler(this.simplifyButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
