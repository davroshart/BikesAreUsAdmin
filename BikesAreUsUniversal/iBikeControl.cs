using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikesAreUsUniversal
{
    interface iBikeControl
    {
        void PushData(clsAllBike prBike);
        void UpdateControl(clsAllBike prBike);
    }
}
