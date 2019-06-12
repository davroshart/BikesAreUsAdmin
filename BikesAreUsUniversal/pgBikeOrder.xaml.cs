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
    public sealed partial class pgBikeOrder : Page
    {
        private delegate void LoadBikeControlDelegate(clsAllBike prBike);
        private Dictionary<char, Delegate> _BikeContent;
        private clsOrder _Order;

        private void dipatchBikeContent(clsAllBike prBike)
        {
            _BikeContent[prBike.Type].DynamicInvoke(prBike);
            updatePage(prBike);
        }

        public pgBikeOrder()
        {
            this.InitializeComponent();
            _Order = new clsOrder();
            _BikeContent = new Dictionary<char, Delegate>
            {
                {'N', new LoadBikeControlDelegate(RunNew)},
                {'U', new LoadBikeControlDelegate(RunUsed)}
            };
        }

        private void updatePage(clsAllBike prBike)
        {
            _Order.Bike = prBike;
            _Order.Serial = prBike.Serial;
            txbBrand.Text = _Order.Bike.Brand;
            txbModel.Text = _Order.Bike.ModelName;
            txbSerial.Text = _Order.Bike.Serial.ToString();
            txbGears.Text = _Order.Bike.Gears;
            if (_Order.Bike.Type == 'U')
                txbType.Text = "Used";
            else
                txbType.Text = "New";
            txbPrice.Text = "$" + _Order.Bike.Price.ToString();
            (ctcBikeSpecs.Content as iBikeControl).UpdateControl(prBike);
        }

        private void pushData()
        {
            _Order.Customer = txtCustomer.Text;
            _Order.ContactPhone = txtNumber.Text;
            _Order.DeliveryAddress = txtAddress.Text;
            (ctcBikeSpecs.Content as iBikeControl).PushData(_Order.Bike);
        }

        private void RunNew(clsAllBike prBike)
        {
            ctcBikeSpecs.Content = new ucNew();
        }

        private void RunUsed(clsAllBike prBike)
        {
            ctcBikeSpecs.Content = new ucUsed();
        }

        private async void btnOrder_Click(object sender, RoutedEventArgs e)
        {
 /*           clsAllBike lcBikeTest = new clsAllBike();
            try
            {
                pushData();
                lcBikeTest = await ServiceClient.GetBikeAsync(_Order.Serial);
                if ()
            }*/
        }

        private void btnCancelClick(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dipatchBikeContent(e.Parameter as clsAllBike);
        }
    }
}
