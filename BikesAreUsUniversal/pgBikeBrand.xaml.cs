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
    public sealed partial class pgBikeBrand : Page
    {
        private clsBrand _Brand;

        public pgBikeBrand()
        {
            this.InitializeComponent();
        }

        private void updateDisplay()
        {
            lstBrandBikes.ItemsSource = null;
            if (_Brand.BikeList != null)
                lstBrandBikes.ItemsSource = _Brand.BikeList;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                string lcBrandName = e.Parameter.ToString();
                _Brand = await ServiceClient.GetBrandSaleBikesAsync(lcBrandName);
                //await ServiceClient.GetSaleBikesAsync(_Brand.Name);
                txbTitle.Text = _Brand.Name + " Bikes";
                updateDisplay();
            }
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            viewBike(lstBrandBikes.SelectedItem as clsAllBike);
        }

        private void viewBike(clsAllBike prBike)
        {
            if (prBike != null)
                Frame.Navigate(typeof(pgBikeOrder), prBike);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void dblTapBike(object sender, DoubleTappedRoutedEventArgs e)
        {
            viewBike(lstBrandBikes.SelectedItem as clsAllBike);
        }
    }
}
