using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BikesAreUsWinForm
{
    static class ServiceClient
    {
        internal async static Task<clsOrder> GetOrder(int prSerial)
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<clsOrder>
            (await lcHttpClient.GetStringAsync("http://localhost:60065/api/bikesareus/GetOrder?Serial=" + prSerial));
        }

        internal async static Task<List<clsOrder>> GetAllOrders()
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<List<clsOrder>>
            (await lcHttpClient.GetStringAsync("http://localhost:60065/api/bikesareus/GetAllOrders/"));
        }

        internal async static Task<List<string>> GetBrandNamesAsync()
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<List<string>>
            (await lcHttpClient.GetStringAsync("http://localhost:60065/api/bikesareus/GetBrandNames/"));
        }

        /*       internal async static Task<clsBrand> GetBrandAsync(string prBrandName)
               {
                   using (HttpClient lcHttpClient = new HttpClient())
                       return JsonConvert.DeserializeObject<clsBrand>
                   (await lcHttpClient.GetStringAsync("http://localhost:60065/api/bikesareus/GetBrand?Name=" + prBrandName));
               }*/

        internal async static Task<clsBrand> GetBrandAllBikesAsync(string prBrandName)
        {
            char lcBikeSearch = 'A';

            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<clsBrand>
            (await lcHttpClient.GetStringAsync($"http://localhost:60065/api/bikesareus/GetBrand?Name={prBrandName}&BikeSearch={lcBikeSearch}"));
        }

        internal async static Task<clsBrand> GetBrandSaleBikesAsync(string prBrandName)
        {
            char lcBikeSearch = 'S';

            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<clsBrand>
            (await lcHttpClient.GetStringAsync($"http://localhost:60065/api/bikesareus/GetBrand?Name={prBrandName}&BikeSearch={lcBikeSearch}"));
        }



        internal async static Task<clsAllBike> GetBikeAsync(int prSerial)
        {
            char lcBikeSearch = 'A';

            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<clsAllBike>
                    (await lcHttpClient.GetStringAsync($"http://localhost:60065/api/bikesareus/GetBikeData?Serial={prSerial}&prSearchType={lcBikeSearch}"));
        }

        private async static Task<string> InsertOrUpdateAsync<TItem>(TItem prItem, string prUrl, string prRequest)
        {
            using (HttpRequestMessage lcReqMessage = new HttpRequestMessage(new HttpMethod(prRequest), prUrl))
            using (lcReqMessage.Content = new StringContent(JsonConvert.SerializeObject(prItem), Encoding.Default, "application/json"))
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.SendAsync(lcReqMessage);
                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }

        internal async static Task<string> InsertBikeAsync(clsAllBike prBike)
        {
            return await InsertOrUpdateAsync(prBike, "http://localhost:60065/api/bikesareus/PostBike", "POST");
        }

        internal async static Task<string> UpdateBikeAsync(clsAllBike prBike)
        {
            return await InsertOrUpdateAsync(prBike, "http://localhost:60065/api/bikesareus/PutBike", "PUT");
        }

        internal async static Task<string> DeleteBikeAsync(clsAllBike prBike)
        {
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.DeleteAsync
                    ($"http://localhost:60065/api/bikesareus/DeleteBike?Serial={prBike.Serial}&Brand={prBike.Brand}");
                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }

        internal async static Task<string> DeleteOrderAsync(clsOrder prOrder, char prType)
        {
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.DeleteAsync
                    ($"http://localhost:60065/api/bikesareus/DeleteOrder?Serial={prOrder.Serial}&Type={prType}");
                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }

    }
}
