using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikesAreUsUniversal
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

        public override string ToString()
        {
            string lcString = ""; //Serial.ToString() + "\t" + ModelName + "\t\t" + Gears.ToString() + "\t";
            if (Type == 'U')
                lcString = lcString + "Used";
            else
                lcString = lcString + "New";
            lcString = lcString + "\t" + ModelName.PadRight(40 - ModelName.Length) + "\t" + Gears + "\t $" +  Price.ToString();

            return lcString;
        }
    }

    /*  public class clsOrder
      {
          public string Customer { get; set; }
          public int Serial { get; set; }
          public DateTime DateOfOrder { get; set; }
          public string DeliveryAddress { get; set; }
          public string ContactPhone { get; set; }
          public decimal PriceAtOrder { get; set; }
      }*/
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
            string lcString = Bike.Serial.ToString() + "\t" + DateOfOrder.ToString() + "\t";
            lcString = lcString + "\t" + Bike.Brand + "\t";
            lcString = lcString + Bike.ModelName.PadRight(40 - Bike.ModelName.Length) + "\t";
            lcString = lcString + PriceAtOrder.ToString();

            return lcString;
        }
    }
}
