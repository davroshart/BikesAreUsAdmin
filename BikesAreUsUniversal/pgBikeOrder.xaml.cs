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
        private string _ExistingCustomer;

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
            _ExistingCustomer = "";
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

        private string validateData()
        {
            if (string.IsNullOrEmpty(txtCustomer.Text))
                return "Customer name cannot be left empty";

            if (txtCustomer.Text.Length > 50)
                return "Customer name cannot be more than 50 characters";
            
            if (string.IsNullOrEmpty(txtNumber.Text))
                return "Contact number cannot be left empty";

            if (txtNumber.Text.Length > 20)
                return "Contact number cannot be more than 20 characters";

            if (string.IsNullOrEmpty(txtAddress.Text))
                return "Address cannot be left empty";

            if (txtAddress.Text.Length > 60)
                return "Address cannot be more than 60 characters";
            return "";
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
            clsAllBike lcBikeTest = new clsAllBike();
            clsOrder lcCustomerCheck = new clsOrder();

            try
            {
                txbMessage.Text = validateData();
                if (txbMessage.Text != "")//data error found?
                    throw new ArgumentException(txbMessage.Text);
                lcCustomerCheck = await ServiceClient.GetOrderAsync(txtCustomer.Text);
                if (lcCustomerCheck != null && lcCustomerCheck.Customer != _ExistingCustomer)//not checked customer name used before?
                {
                    _ExistingCustomer = lcCustomerCheck.Customer;
                    throw new ArgumentException("Did you order a " + lcCustomerCheck.Bike.Brand +
                        " on " + lcCustomerCheck.DateOfOrder.ToString("dd-MM-yyyy") + "?\n" +
                        "If so reclick Order button, otherwise please alter name");
                }

                if (txbMessage.Text == "") //no validation errors?
                {
                    pushData();
                    lcBikeTest = await ServiceClient.GetBikeAsync(_Order.Serial);
                    if (lcBikeTest == null) //bike since been deleted?
                        throw new ArgumentException("Sorry, this bike is no longer for sale. Please close your order");

                    if (lcBikeTest.LastModified != _Order.Bike.LastModified)//bike data modified?
                    {
                        if (lcBikeTest.SaleState == 'O')//bike since been ordered?
                        {
                            throw new ArgumentException("Sorry, this bike is no longer for sale. Please close your order");
                        }
                        if (lcBikeTest.Price != _Order.Bike.Price)
                        {
                            updatePage(lcBikeTest);
                            throw new ArgumentException("Warning: The price for this bike has been modified and " +
                                "updated. Please continue with your order if willing");
                        }                        
                    }                                
                    _Order.PriceAtOrder = _Order.Bike.Price;
                    _Order.DateOfOrder = DateTime.Now.Date;
                    txbMessage.Text = await ServiceClient.InsertOrderAsync(_Order);                                      
                }
            }
            catch (Exception ex)
            {
                txbMessage.Text = ex.GetBaseException().Message;
            }
        }

        private void btnCloseClick(object sender, RoutedEventArgs e)
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
