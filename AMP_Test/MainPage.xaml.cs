using Anfema.Amp;
using Anfema.Amp.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace AMP_Test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: this is for initializing the singleton parser
            await Amp.getInstance(AppController.instance.ampConfig).LoginAsync("admin@anfe.ma", "test");

            this.allDataButton.IsEnabled = true;
            this.dataTypesButton.IsEnabled = true;

            await Amp.getInstance(AppController.instance.ampConfig).LoadDataAsync();

            this.getPageNames();
        }

        private void getPageNames()
        {
            List<string> pageNames = Amp.getInstance(AppController.instance.ampConfig).getPageNames();

            this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                allPagesList.DataContext = pageNames;

                // Disable the progress ring
                allPagesProgressRing.IsActive = false;
                allPagesProgressRing.Visibility = Visibility.Collapsed;
            });
        }


        private void dataTypesButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DataTypes));
        }

        private void allData_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AllData));
        }

        private void pageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AllData), sender);
        }
    }
}
