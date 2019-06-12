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
    public partial class frmBike : Form
    {
        protected clsAllBike _Bike;

        public delegate void LoadBikeFormDelegate(clsAllBike prBike);
        public static Dictionary<char, Delegate> _BikeForm = new Dictionary<char, Delegate>
        {
            {'N', new LoadBikeFormDelegate(frmNewBike.Run)},
            {'U', new LoadBikeFormDelegate(frmUsedBike.Run)}
        };

        public static void DispatchBikeForm(clsAllBike prBike)
        {            
            _BikeForm[prBike.Type].DynamicInvoke(prBike);           
        }

        public frmBike()
        {
            InitializeComponent();
        }

        protected virtual void updateForm()
        {
            txtSerial.Enabled = (string.IsNullOrEmpty(_Bike.Serial.ToString())||_Bike.Serial==0);
            txtSerial.Text = _Bike.Serial.ToString();
            
            txtGears.Text = _Bike.Gears;
            txtModel.Text = _Bike.ModelName;
            txtPrice.Text = _Bike.Price.ToString();
            txtModified.Text = _Bike.LastModified.ToString();
        }

        protected virtual void pushData()
        {
            _Bike.Serial = int.Parse(txtSerial.Text);
            _Bike.Gears = txtGears.Text;
            _Bike.ModelName = txtModel.Text;
            _Bike.Price = decimal.Parse(txtPrice.Text);
        }

        public void SetDetails(clsAllBike prBike)
        {
            _Bike = prBike;
            updateForm();
            ShowDialog();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (isValid() == true)
            {                
                pushData();
                if (txtSerial.Enabled)//new bike?
                {
                    clsAllBike lcBikeCheck = await ServiceClient.GetBikeAsync(int.Parse(txtSerial.Text));

                    if (lcBikeCheck == null)//serial not used?
                    {
                        MessageBox.Show(await ServiceClient.InsertBikeAsync(_Bike));
                        Close();
                    }
                    else
                        MessageBox.Show("Serial number already used. Please enter a different number");
                }
                else
                {
                    clsAllBike lcCheckBike = await ServiceClient.GetBikeAsync(_Bike.Serial);

                    if (lcCheckBike.LastModified == _Bike.LastModified)//no alteration since download?
                    {
                        MessageBox.Show(await ServiceClient.UpdateBikeAsync(_Bike));
                        Close();
                    }

                    if (lcCheckBike.SaleState != 'F')//bike has been ordered?
                        MessageBox.Show("Bike has been ordered since edit began. Take any alternative action required");

                    if (lcCheckBike == null)//bike deleted elsewhere?
                        MessageBox.Show("Bike has been deleted since edit began. Edit must be abandoned");
                    
                }                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected virtual bool isValid()
        {
            int lcIntTest = 0;
            decimal lcDecTest = 0;

            if (!(int.TryParse(txtSerial.Text, out lcIntTest) && lcIntTest > 0))
            {
                MessageBox.Show("Serial number contains unexpected characters");
                return false;
            }

            if (txtModel.Text.Length > 60)
            {
                MessageBox.Show("Model/Description contains over 60 characters");
                return false;
            }

            if (txtGears.Text.Length > 5)
            {
                MessageBox.Show("Model/Description contains over five characters");
                return false;
            }

            if (!(decimal.TryParse(txtPrice.Text, out lcDecTest) && lcDecTest > 0))
            {
                MessageBox.Show("Price contains unacceptable bike price");
                return false;
            }

            return true;
        }

    }
}
