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

        private async void validateData()
        {
            if (string.IsNullOrEmpty(txtCustomer.Text))
            {
                txbMessage.Text = "Customer name cannot be left empty";
                return;
            }
            if (txtCustomer.Text.Length > 50)
            {
                txbMessage.Text = "Customer name cannot be more than 50 characters";
                return;
            }

            if (string.IsNullOrEmpty(txtNumber.Text))
            {
                txbMessage.Text = "Contact number cannot be left empty";
                return;
            }
            if (txtNumber.Text.Length > 20)
            {
                txbMessage.Text = "Contact number cannot be more than 20 characters";
                return;
            }

            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                txbMessage.Text = "Address cannot be left empty";
                return;
            }
            if (txtAddress.Text.Length > 60)
            {
                txbMessage.Text = "Address cannot be more than 60 characters";
                return;
            }

            clsOrder lcOrderTest = new clsOrder();
            lcOrderTest = await ServiceClient.GetOrderAsync(_Order.Customer, _Order.Serial);
            if (lcOrderTest != null)//customer name used with bike before?
            {
                txbMessage.Text = "This name has been used with this bike before. Please enter a different name";
                return;
            }

            clsOrder lcCustomerCheck = new clsOrder();
            lcCustomerCheck = await ServiceClient.GetOrderAsync(_Order.Customer);
            if (lcCustomerCheck != null && lcCustomerCheck.Customer != _ExistingCustomer)//not checked customer name used before?
            {
                txbMessage.Text = "Did this customer order a " + lcCustomerCheck.Bike.Brand + " on " + lcCustomerCheck.DateOfOrder.ToString("dd-MM-yyyy") + "? If so reclick Order button, otherwise please alter name";

                _ExistingCustomer = lcCustomerCheck.Customer;
                return;
            }
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
            try
            {
                txbMessage.Text = "";
                validateData();
                if (txbMessage.Text == "") //no validation errors?
                {
                    pushData();
                    lcBikeTest = await ServiceClient.GetBikeAsync(_Order.Serial);
                    if (lcBikeTest.LastModified != _Order.Bike.LastModified)//bike data modified?
                    {
                        if (lcBikeTest.SaleState == 'O' || lcBikeTest == null)//since been ordered or deleted?
                        {
                            txbMessage.Text = "Sorry, this bike is no longer for sale. Please close your order";
                            return;
                        }
                        if (lcBikeTest.Price != _Order.Bike.Price)
                        {
                            txbMessage.Text = "Warning: The price for this bike has been modified. The price displayed has been " +
                                "updated, so please continue with your order if willing";
                            updatePage(lcBikeTest);
                            return;
                        }                        
                    }                
                    else
                    {
                        _Order.PriceAtOrder = _Order.Bike.Price;
                        _Order.DateOfOrder = DateTime.Now.Date;
                        txbMessage.Text = await ServiceClient.InsertOrderAsync(_Order);                    
                    }
                }
            }
            catch (Exception ex)
            {
                txbMessage.Text = "Error: " + ex.GetBaseException().Message;
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
