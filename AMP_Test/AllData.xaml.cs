using Anfema.Ion;
using Anfema.Ion.DataModel;
using Anfema.Ion.FullTextSearch;
using Anfema.Ion.Parsing;
using Anfema.Ion.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.System;
using System.Reflection;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AMP_Test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllData : Page
    {
        private IonPageObservableCollection _allContent = new IonPageObservableCollection();

        public AllData()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Button pressedButton = (Button)e.Parameter;
            showData((string)pressedButton.DataContext);

            if( Windows.Foundation.Metadata.ApiInformation.IsTypePresent( "Windows.Phone.UI.Input.HardwareButtons" ) )
            {
                HardwareButtons.BackPressed += this.HardwareButtons_BackPressed;
            }

        }

        private async void fileContent_Click(object sender, RoutedEventArgs e)
        {
            IonFileContent fileContent = ((IonFileContent)((Button)sender).DataContext);
            bool success = await Windows.System.Launcher.LaunchFileAsync(fileContent.storageFile);
        }

        private async Task showData(string pageName)
        {
            try
            {
                IonPage page = await Ion.getInstance(AppController.instance.ampConfig).getPageAsync(pageName, null);

                _allContent = DataConverters.convertContent(page.getContent());

                // Load files for page content
                await Ion.getInstance(AppController.instance.ampConfig).LoadContentFiles(_allContent);

                //await Ion.getInstance( AppController.instance.ampConfig ).DownloadSearchDatabase();
                //List<SearchResult> results = await Ion.getInstance( AppController.instance.ampConfig ).FullTextSearch("test", "de_DE");

                // Set the data context of the lists
                imageContentList.DataContext = _allContent.imageContent;
                textContentList.DataContext = _allContent.textContent;
                colorContentList.DataContext = _allContent.colorContent;
                flagContentList.DataContext = _allContent.flagContent;
                fileContentList.DataContext = _allContent.fileContent;
                mediaContentList.DataContext = _allContent.mediaContent;
                dateTimeContentList.DataContext = _allContent.dateTimeContent;
                optionContentList.DataContext = _allContent.optionContent;
            }
            catch (Exception exception)
            {
                // Error handling, if no data could be loaded  
                Debug.WriteLine("Error loading page: " + exception.Message);
            }

            setDataLoaded();

            initPage();
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if( Windows.Foundation.Metadata.ApiInformation.IsTypePresent( "Windows.Phone.UI.Input.HardwareButtons" ) )
            {
                HardwareButtons.BackPressed -= this.HardwareButtons_BackPressed;
            }
        }


        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }


        // Called when the json data ist completely loaded and parsed
        private void setDataLoaded()
        {
            imageContentProgressRing.Visibility = Visibility.Collapsed;
            imageContentProgressRing.IsActive = false;

            textContentProgressRing.Visibility = Visibility.Collapsed;
            textContentProgressRing.IsActive = false;

            colorContentProgressRing.Visibility = Visibility.Collapsed;
            colorContentProgressRing.IsActive = false;

            flagContentProgressRing.Visibility = Visibility.Collapsed;
            flagContentProgressRing.IsActive = false;

            fileContentProgressRing.Visibility = Visibility.Collapsed;
            fileContentProgressRing.IsActive = false;

            mediaContentProgressRing.Visibility = Visibility.Collapsed;
            mediaContentProgressRing.IsActive = false;

            dateTimeContentProgressRing.Visibility = Visibility.Collapsed;
            dateTimeContentProgressRing.IsActive = false;

            optionContentProgressRing.Visibility = Visibility.Collapsed;
            optionContentProgressRing.IsActive = false;

            keyValueContentProgressRing.Visibility = Visibility.Collapsed;
            keyValueContentProgressRing.IsActive = false;
        }


        // Shows the image content or hides it
        private void imageContentTextBlockClicked(object sender, RoutedEventArgs e)
        {
            Visibility currentVisibility = imageContentList.Visibility;

            if (currentVisibility == Visibility.Collapsed)
            {
                imageContentList.Visibility = Visibility.Visible;
                imageContentChevron.Symbol = Symbol.Remove;
            }
            else
            {
                imageContentList.Visibility = Visibility.Collapsed;
                imageContentChevron.Symbol = Symbol.Add;
            }
        }


        // Switches the text content section on and off
        private void textContentList_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch ts = (ToggleSwitch)sender;

            if (ts.IsOn)
            {
                textContentList.Visibility = Visibility.Visible;
            }
            else
            {
                textContentList.Visibility = Visibility.Collapsed;
            }
        }


        // Sets the UI to the right states
        private void initPage()
        {
            textContentToggleSwitch.IsOn = textContentList.Visibility == Visibility.Visible ? true : false;
            colorContentRadioButton.IsChecked = colorContentList.Visibility == Visibility.Visible ? true : false;
        }


        // Turns the color section on and off
        private void colorContentRadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            Visibility currentVisibility = colorContentList.Visibility;

            if (currentVisibility == Visibility.Collapsed)
            {
                colorContentList.Visibility = Visibility.Visible;
                rb.IsChecked = true;
            }
            else
            {
                colorContentList.Visibility = Visibility.Collapsed;
                rb.IsChecked = false;
            }
        }


        // When the media element is tabbed
        private void mediaContentElement_Tabbed(object sender, TappedRoutedEventArgs e)
        {
            MediaElement me = (MediaElement)sender;

            TextBlock playButton = getSibling<TextBlock>(me, "playButtonTextBlock");


            // Change the state of the media element
            switch (me.CurrentState)
            {
                case MediaElementState.Playing:
                    {
                        me.Pause();
                        if (playButton != null)
                        {
                            playButton.Visibility = Visibility.Visible;
                        }
                        break;
                    }
                case MediaElementState.Paused:
                    {
                        me.Play();
                        if (playButton != null)
                        {
                            playButton.Visibility = Visibility.Collapsed;
                        }
                        break;
                    }
            }
        }


        // Is used when the loading of the media didn't work
        private void mediaContent_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MediaElement me = (MediaElement)sender;
            IonMediaContent mc = (IonMediaContent)me.DataContext;

            // This is the fallback in case the media file is simply an image
            if (mc.mimeType.Contains("image/") && mc.mediaURI != null)
            {
                // Set image and set it visible
                Image image = getSibling<Image>(me, "mediaContentImage");
                image.Source = new BitmapImage(mc.mediaURI);
                image.Visibility = Visibility.Visible;

                // Hide media element
                me.Visibility = Visibility.Collapsed;

                // Hide the play button
                getSibling<TextBlock>(me, "playButtonTextBlock").Visibility = Visibility.Collapsed;
            }

            //Debug.WriteLine("Media content of type " + mc.mime_type + " failed! " + me.Source);
        }


        // Gets a sibling of the overhanded element with the name specified
        private static T getSibling<T>(DependencyObject reference, string name) where T : FrameworkElement
        {
            // Get parent of the actual element to finally get the siblings of the element
            DependencyObject parent = VisualTreeHelper.GetParent(reference);

            // Iterate through all siblings
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                // Search for the desired sibling of type T
                if (child.GetType() == typeof(T))
                {
                    T convertedChild = (T)Convert.ChangeType(child, typeof(T));

                    // Check the searched name
                    if (convertedChild.Name.Equals(name))
                    {
                        return convertedChild;
                    }
                }
            }

            return null;
        }



        private void keyboardListener( object sender, KeyRoutedEventArgs e )
        {
            if(e.Key == VirtualKey.Space )
            {
                Debug.WriteLine( "Button pressed" );
                Frame frame = Window.Current.Content as Frame;
                if( frame.CanGoBack )
                {
                    frame.GoBack();
                    e.Handled = true;
                }
            }
        }
    }
}
