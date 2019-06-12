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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BikesAreUsUniversal
{
    public sealed partial class ucNew : UserControl, iBikeControl
    {
        public ucNew()
        {
            this.InitializeComponent();
        }

        public void PushData(clsAllBike prBike)
        {
            prBike.Warranty = int.Parse(txbWarranty.Text);
        }

        public void UpdateControl(clsAllBike prBike)
        {
            txbWarranty.Text = prBike.Warranty.ToString();
        }
    }
}
