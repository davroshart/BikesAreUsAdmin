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
    /*sealed */public partial class frmOrders : Form
    {
      /*  private static readonly frmOrders _Instance =
            new frmOrders();*/

        private static Dictionary<string, frmOrderDetails> _OrderDetailsFormList =
            new Dictionary<string, frmOrderDetails>();

        private List<clsOrder> _OrderList;

        public frmOrders()
        {
            InitializeComponent();
            UpdateDisplay();
        }

        //public static frmOrders Instance => _Instance;

        async public void UpdateDisplay()
        {
            try
            {
                _OrderList = await ServiceClient.GetAllOrders();
                decimal lcTotal = 0;
                lstOrders.DataSource = null;
                foreach (clsOrder lcOrder in _OrderList)
                {
                    lstOrders.Items.Add(lcOrder);
                    lcTotal = lcTotal + lcOrder.PriceAtOrder;
                }
                lblTotal.Text = "$" + lcTotal.ToString();
            }
            catch (Exception ex)
            {
                lstOrders.Text = "Error : " + ex.GetBaseException().Message;
            }
        }

        private async void lstOrders_DoubleClick(object sender, EventArgs e)
        {
            int lcKey = Convert.ToInt32(_OrderList[lstOrders.SelectedIndex].Bike.Serial);

            try
            {
                clsOrder lcOrder = await ServiceClient.GetOrder(lcKey);
                frmOrderDetails lcOrderDetails = new frmOrderDetails();
                lcOrderDetails.Show();
                lcOrderDetails.Run(lcOrder);
                //_OrderDetailsFormList.Add(lcKey, lcOrderDetails);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Edit Error");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
