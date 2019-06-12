using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BikesAreUsUniversal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class pgMainPage : Page
    {
        public pgMainPage()
        {
            this.InitializeComponent();
        }

        private void viewBrand()
        {
            if (lstBrand.SelectedItem != null)
                Frame.Navigate(typeof(pgBikeBrand), lstBrand.SelectedItem);
        }

        private void lstDoubleTap(object sender, DoubleTappedRoutedEventArgs e)
        {
            viewBrand();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lstBrand.ItemsSource = await ServiceClient.GetBrandNamesAsync();
            }
            catch (Exception ex)
            {
                txbMessage.Text = "Error loading brand names: " + ex.GetBaseException().Message;
            }
        }

        private void btnOpenClick(object sender, RoutedEventArgs e)
        {
            viewBrand();
        }
    }
}
