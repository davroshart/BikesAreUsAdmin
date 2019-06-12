using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BikesAreUsWinForm
{
    sealed public partial class frmUsedBike : BikesAreUsWinForm.frmBike
    {
        public static readonly frmUsedBike Instance =
            new frmUsedBike();

        public frmUsedBike()
        {
            InitializeComponent();
        }

        public static void Run(clsAllBike prUsedBike)
        {
            Instance.SetDetails(prUsedBike);
        }

        protected override void updateForm()
        {
            base.updateForm();
            if (string.IsNullOrEmpty(_Bike.BikeCondition))//unset ?
                lstCondition.SelectedIndex = 0;
            else
                lstCondition.SelectedItem = _Bike.BikeCondition.ToString();

            if (_Bike.PreviousOwners == null)//unset ?
                txtOwners.Text = "0";
            else
                txtOwners.Text = _Bike.PreviousOwners.ToString();

            lblTitle.Text = "Used " + _Bike.Brand + " Details";
        }

        protected override void pushData()
        {
            base.pushData();
            _Bike.BikeCondition = lstCondition.SelectedItem.ToString();
            _Bike.PreviousOwners = txtOwners.Text;
        }

        protected override bool isValid()
        {
            if (txtOwners.Text.Length > 5)
            {
                MessageBox.Show("Previous owners must contain less than five characters");
                return false;
            }

            if (lstCondition.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Please select a condition for the bike");
                return false;
            }

            return base.isValid();
        }
    }
}
