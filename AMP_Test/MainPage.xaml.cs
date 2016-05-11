using Anfema.Ion;
using Anfema.Ion.DataModel;
using Anfema.Ion.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
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
        IonConfig _ampConfig;


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Get all the login informations needed for later communication            
            try
            {
                _ampConfig = await AppController.instance.loginAsync();

                await this.getPageNames();
            }
            catch( Exception exception )
            {
                Debug.WriteLine("Error occured logging in: " + exception.Message);

                disableProgressIndicator();
            }
        }


        private async Task<bool> getPageNames()
        {
            // Get all pagePreviews of the collection
            List<IonPagePreview> pagePreviews = await Ion.getInstance( _ampConfig ).getPagePreviewsAsync( PageFilter.all );

            // Extract the page names from the pages list
            List<string> pageNames = DataConverters.getPageIdentifier( pagePreviews );

            allPagesList.DataContext = pageNames;

            disableProgressIndicator();            

            return true;
        }


        private void pageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AllData), sender);
        }


        private void disableProgressIndicator()
        {
            // Disable the progress ring
            allPagesProgressRing.IsActive = false;
            allPagesProgressRing.Visibility = Visibility.Collapsed;
        }


        private async void archiveDownloadButton_Click( object sender, RoutedEventArgs e )
        {
            Debug.WriteLine( "Starting extraction" );
            archiveDownloadProgressRing.IsActive = true;
            await Ion.getInstance( _ampConfig ).loadArchive( "https://lookbook-dev.anfema.com/client/v1/de_DE/lookbook.tar?variation=default" );
            archiveDownloadProgressRing.IsActive = false;
            Debug.WriteLine( "Extraction finished" );
        }
    }
}
