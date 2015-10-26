using Anfema.Amp;
using Anfema.Amp.DataModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AMP_Test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DataTypes : Page
    {
        public DataTypes()
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
            ObservableCollection<DataTypeModel> oc = new ObservableCollection<DataTypeModel>();

            Dictionary<string, string> dataTypes = Amp.Instance.ApiCalls;

            for (int i = 0; i < dataTypes.Count; i++)
            {
                oc.Add( new DataTypeModel( dataTypes.ElementAt(i).Key.ToString(), dataTypes.ElementAt(i).Value.ToString() ) );
            }

            this.DataContext = oc;

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


        private void dataTypeItem_Click(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(DataTypeDetails), e.ClickedItem);
        }
    }
}
