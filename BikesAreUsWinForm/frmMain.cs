using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace BikesAreUsWinForm
{
    sealed public partial class frmMain : Form
    {
        //singleton
        private static readonly frmMain _Instance = new frmMain();

        public frmMain()
        {
            InitializeComponent();
        }

        public static frmMain Instance => _Instance;

        async public void UpdateDisplay()
        {
            try
            {
                lstBrandName.DataSource = null;
                lstBrandName.DataSource = await ServiceClient.GetBrandNamesAsync();
            }
            catch (Exception ex)
            {
                lstBrandName.Text = "Error : " + ex.GetBaseException().Message;
            }

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void Brand_DblClick(object sender, MouseEventArgs e)
        {
            string lcKey = Convert.ToString(lstBrandName.SelectedItem);
            try
            {
                frmBrand.Run(lcKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.GetBaseException().Message);
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            try
            {
                frmOrders lcOrders = new frmOrders();
                lcOrders.Show();
                lcOrders.Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.GetBaseException().Message);
            }
        }
    }
}
