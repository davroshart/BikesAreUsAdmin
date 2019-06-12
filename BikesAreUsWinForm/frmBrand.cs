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
    public partial class frmBrand : Form
    {
        private clsBrand _Brand;

        private static Dictionary<string, frmBrand> _BrandFormList =
            new Dictionary<string, frmBrand>();

        public frmBrand()
        {
            InitializeComponent();
        }

        public static void Run(string prBrandName)
        {
            frmBrand lcBrandForm;

            if (string.IsNullOrEmpty(prBrandName))
            {
                MessageBox.Show("A brand must be provided");
                return;
            }

            if (!_BrandFormList.TryGetValue(prBrandName, out lcBrandForm))
            {
                lcBrandForm = new frmBrand();
                _BrandFormList.Add(prBrandName, lcBrandForm);
                lcBrandForm.refreshFormFromDB(prBrandName);
            }      
            else
            {
                lcBrandForm.Show();
                lcBrandForm.Activate();
            }
        }

        private async void refreshFormFromDB(string prBrandName)
        {
           SetDetails(await ServiceClient.GetBrandAllBikesAsync(prBrandName));
        }
        
        private void updateForm()
        {
            lblTitle.Text = _Brand.Name;
            lblLogo.Text = _Brand.Logo;
            lblDescription.Text = _Brand.Description;            
        }

        private void updateDisplay()
        {
            lstBikes.DataSource = null;
            if (_Brand.BikeList != null)
                 lstBikes.DataSource = _Brand.BikeList;
        }

        public void SetDetails(clsBrand prBrand)
        {
            _Brand = prBrand;
            updateForm();
            updateDisplay();
            Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string lcReply = new InputBox(clsAllBike.FACTORY_PROMPT).Answer;
                lcReply = lcReply.ToUpper();
                if (!string.IsNullOrEmpty(lcReply)) // not cancelled?
                {
                    clsAllBike lcBike = clsAllBike.NewBike(lcReply[0]);                    
                    if (lcBike != null) // valid bike created?
                    {
                        lcBike.Brand = _Brand.Name;
                        lcBike.SaleState = 'F';
                        lcBike.Type = lcReply[0];
                        frmBike.DispatchBikeForm(lcBike);
                        if (!string.IsNullOrEmpty(lcBike.Serial.ToString())) // not cancelled?
                        {
                            refreshFormFromDB(_Brand.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void lstBikes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int lcIndex = lstBikes.SelectedIndex;
            try
            {
                frmBike.DispatchBikeForm(lstBikes.SelectedValue as clsAllBike);
                updateDisplay();
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry no bike selected #" + Convert.ToString(lcIndex));
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Deleting Bike", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /*needed?
                clsAllBike lcCheckBike = lstBikes.SelectedItem as clsAllBike;
                lcCheckBike = await ServiceClient.GetBikeAsync(lcCheckBike.Serial);

                if (lcCheckBike.SaleState != 'F')//bike has been ordered?
                   if( MessageBox.Show("Bike has been ordered. Continue with delete?", "Deleting Bike", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))*/
                MessageBox.Show(await ServiceClient.DeleteBikeAsync(lstBikes.SelectedItem as clsAllBike));
                refreshFormFromDB(_Brand.Name);
            }
        }
    }
}
