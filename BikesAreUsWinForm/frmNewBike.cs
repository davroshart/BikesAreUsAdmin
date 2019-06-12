using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BikesAreUsWinForm
{
    sealed public partial class frmNewBike : BikesAreUsWinForm.frmBike
    {
        public static readonly frmNewBike Instance =
            new frmNewBike();

        public frmNewBike()
        {
            InitializeComponent();
        }

        public static void Run(clsAllBike prNewBike)
        {
            Instance.SetDetails(prNewBike);            
        }

        protected override void updateForm()
        {
            base.updateForm();
            txtWarranty.Text = _Bike.Warranty.ToString();
            lblTitle.Text = "New " + _Bike.Brand + " Details";
        }

        protected override void pushData()
        {
            base.pushData();
            _Bike.Warranty = int.Parse(txtWarranty.Text);
        }

        protected override bool isValid()
        {
            int lcIntTest = 0;
            
            if (!(int.TryParse(txtWarranty.Text, out lcIntTest))&& lcIntTest >= 0)
            {
                MessageBox.Show("Warranty must be numeric and 0 or more months");
                return false;
            }

            return base.isValid();
        }
    }
}
