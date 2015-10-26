using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AMP_Test
{
    public sealed partial class DataTypeDetails : Page
    {
        public DataTypeDetails()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            /*
            DataTypeModel dtm = (DataTypeModel)e.Parameter;
            dataTypeTextBlock.Text = dtm.name + " details";

            string response = await AppModel.instance.parser.getData(dtm.name);
            outputTextBlock.Text = response;
            */
            // Change the UI to data loaded
            dataLoaded();
            

            HardwareButtons.BackPressed += this.HardwareButtons_BackPressed;
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= this.HardwareButtons_BackPressed;
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


        private void dataLoaded()
        {
            // Disable the loading animation, when the content is ready
            progressRingContent.IsActive = false;
            progressRingContent.Visibility = Visibility.Collapsed;
            outputTextBlock.Visibility = Visibility.Visible;
        }
    }
}
