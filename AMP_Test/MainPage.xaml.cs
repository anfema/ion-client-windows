using Anfema.Amp;
using Anfema.Amp.DataModel;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

        // Config file for the current data context
        AmpConfig _ampConfig;

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Get all the login informations needed for later communication            
            _ampConfig = await AppController.instance.loginAsync();

            this.allDataButton.IsEnabled = true;
            this.dataTypesButton.IsEnabled = true;

            // Debug loading of all the data at startup
            await Amp.getInstance(_ampConfig).LoadDataAsync();

            this.getPageNames();
        }

        private void getPageNames()
        {
            List<string> pageNames = Amp.getInstance(_ampConfig).getPageNames();

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
