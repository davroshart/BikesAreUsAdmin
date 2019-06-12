using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BikesAreUsWinForm
{
    public partial class frmOrderDetails : Form
    {
        private clsOrder _Order;

        public frmOrderDetails()
        {
            InitializeComponent();
        }

        public void Run(clsOrder prOrder)
        {
            _Order = prOrder;
            Update();
        }

        private async void btnComplete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Order completed?", "Complete Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show(await ServiceClient.DeleteOrderAsync(_Order, 'D'));
                Close();
            }
        }

        private async void btnCancelled_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Cancel order?", "Cancel Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show(await ServiceClient.DeleteOrderAsync(_Order, 'C'));
                Close();
            }
        }

        public void Update()
        {
            txtSerial.Text = _Order.Serial.ToString();
            txtBrand.Text = _Order.Bike.Brand;
            txtModel.Text = _Order.Bike.ModelName;
            txtCustomer.Text = _Order.Customer;
            txtContact.Text = _Order.ContactPhone;
            txtAddress.Text = _Order.DeliveryAddress;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
