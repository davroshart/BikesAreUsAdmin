using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikesAreUsWinForm
{
    public class clsBrand
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }

        public List<clsAllBike> BikeList { get; set; }

        public clsAllBike GetBike(int prIndex)
        {
            return BikeList[prIndex];
        }
    }

    public class clsAllBike
    {
        public int Serial { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public decimal Price { get; set; }
        public string Gears { get; set; }
        public char SaleState { get; set; }
        public DateTime LastModified { get; set; }
        public char Type { get; set; }
        public int? Warranty { get; set; }
        public string BikeCondition { get; set; }
        public string PreviousOwners { get; set; }

        public static readonly string FACTORY_PROMPT = "Enter N for new bike, or U for used bike";

        public static clsAllBike NewBike(char prBikeType)
        {
            return new clsAllBike() { Type = Char.ToUpper(prBikeType) };
        }

        public override string ToString()
        {
            string lcString = Serial.ToString() + "\t" + 
                ModelName.PadRight(40 - ModelName.Length) + "\t" + Gears.ToString() + "\t";
            if (Type == 'U')
                lcString = lcString + "Used";
            else
                lcString = lcString + "New";
            lcString = lcString + "\t" + Price.ToString() + "\t";
            if (SaleState == 'F')
                lcString = lcString + "Available";
            else
                lcString = lcString + "Ordered";
            return lcString;
        }
    }

    public class clsOrder
    {
        public string Customer { get; set; }
        public int Serial { get; set; }
        public DateTime DateOfOrder { get; set; }
        public string DeliveryAddress { get; set; }
        public string ContactPhone { get; set; }
        public decimal PriceAtOrder { get; set; }

        public clsAllBike Bike { get; set; }

        public override string ToString()
        {
            string lcString = Bike.Serial.ToString() + "    " + DateOfOrder.ToShortDateString() + "\t";
            if (Bike.Brand.Length >20)
                lcString = lcString + Bike.Brand.Substring(0, 20) + "\t";  
            else
                lcString = lcString + Bike.Brand.PadRight(20) + "\t";
            if (Bike.ModelName.Length > 20)
                lcString = lcString + Bike.ModelName.Substring(0, 20) + "\t";
            else
                lcString = lcString + Bike.ModelName.PadRight(20) + "\t";
            lcString = lcString + PriceAtOrder.ToString();

            return lcString;
        }
    }
}
