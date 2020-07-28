using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

namespace TSR
    {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
        {
        public App ()
            {
            var culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            FrameworkElement.LanguageProperty.OverrideMetadata (
                typeof (FrameworkElement),
                new FrameworkPropertyMetadata (XmlLanguage.GetLanguage (culture.IetfLanguageTag)));
            }

        void App_DispatcherUnhandledException (object sender, DispatcherUnhandledExceptionEventArgs e)
            {
            // Process unhandled exception

            // Prevent default unhandled exception processing
            e.Handled = true;
            }
        }

    }
