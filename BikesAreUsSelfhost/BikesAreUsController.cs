using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikesAreUsSelfhost
{
    public class BikesAreUsController : System.Web.Http.ApiController
    {
        public List<string> GetBrandNames()

        {
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT Name FROM Brand", null);
            List<string> lcNames = new List<string>();
            foreach (DataRow dr in lcResult.Rows)
                lcNames.Add((string)dr[0]);
            return lcNames;
        }

        public clsBrand GetBrand(string Name, char BikeSearch)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(1);
            par.Add("Name", Name);
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT * FROM Brand WHERE Name = @Name", par);
            if (lcResult.Rows.Count > 0)
                if (BikeSearch == 'S')//find only bikes for sale ?
                {
                    return new clsBrand()
                    {
                        Name = (string)lcResult.Rows[0]["Name"],
                        Logo = (string)lcResult.Rows[0]["Logo"],
                        Description = (string)lcResult.Rows[0]["Description"],
                        BikeList = getBrandBike(Name, 'S')
                    };
                }
                else
                {
                    return new clsBrand()
                    {
                        Name = (string)lcResult.Rows[0]["Name"],
                        Logo = (string)lcResult.Rows[0]["Logo"],
                        Description = (string)lcResult.Rows[0]["Description"],
                        BikeList = getBrandBike(Name, 'A')
                    };
                }
            else
                return null;
        }

        private List<clsAllBike> getBrandBike(string Name, char prSearchType)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(1);
            par.Add("Brand", Name);
            string lcSearch = "SELECT * FROM Bike WHERE Brand = @Brand";
            if (prSearchType == 'S') //only find bikes not ordered?
                lcSearch = lcSearch + " AND SaleState = 'F'";
            DataTable lcResult = clsDbConnection.GetDataTable(lcSearch, par);//"SELECT * FROM Bike WHERE Brand = @Brand", par);
            List<clsAllBike> lcBikes = new List<clsAllBike>();
            foreach (DataRow dr in lcResult.Rows)
                lcBikes.Add(dataRow2AllBike(dr));
            return lcBikes;
        }


        public clsAllBike GetBikeData(int Serial)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(1);
            par.Add("Serial", Serial);
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT * FROM Bike WHERE Serial = @Serial", par);
            if (lcResult.Rows.Count == 1)
            {
                clsAllBike lcBike = dataRow2AllBike(lcResult.Rows[0]);
                return lcBike;
            }
            else
                return null;
        }

        private clsAllBike dataRow2AllBike(DataRow prDataRow)
        {
            return new clsAllBike()
            {
                Serial = Convert.ToInt32(prDataRow["Serial"]),
                Brand = Convert.ToString(prDataRow["Brand"]),
                ModelName = Convert.ToString(prDataRow["ModelName"]),
                Price = Convert.ToDecimal(prDataRow["Price"]),
                Gears = Convert.ToString(prDataRow["Gears"]),
                SaleState = Convert.ToChar(prDataRow["SaleState"]),
                LastModified = Convert.ToDateTime(prDataRow["LastModified"]),
                Type = Convert.ToChar(prDataRow["Type"]),
                Warranty = prDataRow["Warranty"] is DBNull ? (int?)null : Convert.ToInt32(prDataRow["Warranty"]),
                BikeCondition = Convert.ToString(prDataRow["BikeCondition"]),
                PreviousOwners = Convert.ToString(prDataRow["PreviousOwners"])
            };
        }

        /*?brand not needed for filter due to serial's uniqueness?*/
        public string DeleteBike(int Serial, string Brand)
        {
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                    "DELETE FROM Bike WHERE Serial = @Serial AND Brand = @Brand",
                    prepareBikeParameters(Serial, Brand));
                if (lcRecCount == 1)
                    return "One bike deleted";
                else
                    return "Unexpected bike delete count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

        public string PostBike(clsAllBike prBike)
        {   //insert
            try
            {
                prBike.LastModified = DateTime.Now;
                int lcRecCount = clsDbConnection.Execute("INSERT INTO Bike " +
                    "(Serial, Brand, ModelName, Price, Gears, SaleState, LastModified, Type, Warranty, BikeCondition, PreviousOwners) " +
                    "VALUES (@Serial, @Brand, @ModelName, @Price, @Gears, @SaleState, @LastModified, @Type, " +
                    "@Warranty, @BikeCondition, @PreviousOwners)", prepareBikeParameters(prBike));
                if (lcRecCount == 1)
                    return "One bike inserted";
                else
                    return "Unexpected bike insert count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

        public string PutBike(clsAllBike prBike)
        {  //update
            try
            {
                prBike.LastModified = DateTime.Now;
                int lcRecCount = clsDbConnection.Execute("UPDATE Bike " +
                    "SET ModelName = @ModelName, Price = @Price, Gears = @Gears, SaleState = @SaleState, LastModified = @LastModified, " +
                    "Type = @Type, Warranty = @Warranty, BikeCondition = @BikeCondition, PreviousOwners = @PreviousOwners " +
                    "WHERE Serial = @Serial AND Brand = @Brand", prepareBikeParameters(prBike));
                if (lcRecCount == 1)
                    return "One bike updated";
                else
                    return "Unexpected bike update count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

        private Dictionary<string, object> prepareBikeParameters(int prSerial, string prBrand)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(2);
            par.Add("Serial", prSerial);
            par.Add("Brand", prBrand);

            return par;
        }

        private Dictionary<string, object> prepareBikeParameters(clsAllBike prBike)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(11);
            par.Add("Serial", prBike.Serial);
            par.Add("Brand", prBike.Brand);
            par.Add("ModelName", prBike.ModelName);
            par.Add("Price", prBike.Price);
            par.Add("Gears", prBike.Gears);
            par.Add("SaleState", prBike.SaleState);
            par.Add("LastModified", prBike.LastModified.ToString("yyy-MM-dd H:mm:ss"));
            par.Add("Type", prBike.Type);
            par.Add("Warranty", prBike.Warranty);
            par.Add("BikeCondition", prBike.BikeCondition);
            par.Add("PreviousOwners", prBike.PreviousOwners);

            return par;
        }

        public List<clsOrder> GetAllOrders()
        {
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT * FROM BikeOrder", null);
            List<clsOrder> lcOrders = new List<clsOrder>();
            foreach (DataRow dr in lcResult.Rows)
                lcOrders.Add(dataRow2Order(dr));
            return lcOrders;
        }

        public clsOrder GetOrder(int Serial)
        {
            return GetOrder(Serial, "");
        }

        public clsOrder GetOrder(string prCustomer)
        {
            return GetOrder(0, prCustomer);
        }

        public clsOrder GetOrder(int Serial, string Customer)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(1);

            par.Add("Customer", Customer);
            par.Add("Serial", Serial);
            DataTable lcResult = new DataTable();
            if (Customer != "" && Serial != 0) // search using serial and customer?
                lcResult = clsDbConnection.GetDataTable("SELECT * FROM BikeOrder WHERE Serial = @Serial AND Customer = @Customer", par);
            else
            {
                if (Customer == "")// search only by serial?
                    lcResult = clsDbConnection.GetDataTable("SELECT * FROM BikeOrder WHERE Serial = @Serial", par);
                else
                    lcResult = clsDbConnection.GetDataTable("SELECT * FROM BikeOrder WHERE Customer = @Customer", par);
            }

            if (lcResult.Rows.Count == 1 || (lcResult.Rows.Count > 1 && Customer != ""))// maybe checking for other orders by given customer?
            {
                clsOrder lcOrder = dataRow2Order(lcResult.Rows[0]);
                return lcOrder;
            }
            else
                return null;
        }

        public string DeleteOrder(clsOrder Order, char Type)
        {
            int Serial = Order.Serial;
            string Customer = Order.Customer;

            try
            {
                int lcRecCount = clsDbConnection.Execute(
                    "DELETE FROM BikeOrder WHERE Serial = @Serial AND Customer = @Customer",
                    prepareOrderParameters(Customer, Serial));
                if (lcRecCount == 1)
                {
                    if (Type == 'D')//delete bike?
                    {
                        DeleteBike(Serial, Order.Bike.Brand);
                        return "One order completed";
                    }
                    else//cancel order?
                    {
                        int lcBikeRecCount = clsDbConnection.Execute(
                            "UPDATE Bike SET SaleState = 'F' WHERE Serial = @Serial",
                            prepareBikeParameters(Serial, Order.Bike.Brand));

                        return "One order cancelled";
                    }
                }
                else
                    return "Unexpected order delete count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

        private clsOrder dataRow2Order(DataRow prDataRow)
        {
            clsOrder lcNewOrder = new clsOrder()
            {
                Customer = Convert.ToString(prDataRow["Customer"]),
                Serial = Convert.ToInt32(prDataRow["Serial"]),
                DateOfOrder = Convert.ToDateTime(prDataRow["DateOfOrder"]),
                DeliveryAddress = Convert.ToString(prDataRow["DeliveryAddress"]),
                ContactPhone = Convert.ToString(prDataRow["ContactPhone"]),
                PriceAtOrder = Convert.ToDecimal(prDataRow["PriceAtOrder"])
            };
            lcNewOrder.Bike = GetBikeData(lcNewOrder.Serial);
            return lcNewOrder;
        }

  /*      public string PutOrder(clsOrder prOrder)
        {   //update
            try
            {
                int lcBikeRecCount = 0;
                int lcRecCount = clsDbConnection.Execute(
                    "UPDATE BikeOrder SET DateOfOrder = @DateOfOrder, DeliveryAddress = @DeliveryAddress, " +
                    "ContactPhone = @ContactPhone, PriceAtOrder = @PriceAtOrder " +
                    "WHERE Customer = @Customer AND Serial = @Serial", prepareOrderParameters(prOrder));
                if (lcRecCount == 1)
                {
                    lcBikeRecCount = clsDbConnection.Execute(
                        "UPDATE Bike SET SaleState = 'O' " +
                        "WHERE Brand = @Brand AND Serial = @Serial", prepareBikeParameters(prOrder.Bike.Serial, prOrder.Bike.Brand));
                }
                if (lcRecCount == 1 && lcBikeRecCount == 1)   
                    return "Order and bike updated";
                else
                    if (lcRecCount != 1)
                        return "Unexpected Order update count: " + lcRecCount;
                    else
                        return "Unexpected Bike update count: " + lcBikeRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }*/

        public string PostOrder(clsOrder prOrder)
        {   //insert
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                    "INSERT INTO BikeOrder (Customer, Serial, DateOfOrder, DeliveryAddress, ContactPhone, PriceAtOrder) " +
                    "VALUES (@Customer, @Serial, @DateOfOrder, @DeliveryAddress, @ContactPhone, @PriceAtOrder)", 
                    prepareOrderParameters(prOrder));
                if (lcRecCount == 1)
                {
                    prOrder.Bike.SaleState = 'O';
                    prOrder.Bike.LastModified = DateTime.Now; 
                    clsDbConnection.Execute(
                        "UPDATE Bike SET SaleState = @SaleState, LastModified = @LastModified " +
                        "WHERE Brand = @Brand AND Serial = @Serial", prepareBikeParameters(prOrder.Bike));
                    return "Your bike order has been completed!";
                }
                else
                    return "Unexpected order insert count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

        private Dictionary<string, object> prepareOrderParameters(clsOrder prOrder)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(6);
            par.Add("Customer", prOrder.Customer);
            par.Add("Serial", prOrder.Serial);
            par.Add("DateOfOrder", prOrder.DateOfOrder.ToString("yyy-MM-dd H:mm:ss"));
            par.Add("DeliveryAddress", prOrder.DeliveryAddress);
            par.Add("ContactPhone", prOrder.ContactPhone);
            par.Add("PriceAtOrder", prOrder.PriceAtOrder);
            return par;
        }

        private Dictionary<string, object> prepareOrderParameters(string prName, int prSerial)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(2);
            par.Add("Customer", prName);
            par.Add("Serial", prSerial);
            return par;
        }
    }
}
