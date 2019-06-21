using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BikesAreUsUniversal
{
    static class ServiceClient
    {
        internal async static Task<List<string>> GetBrandNamesAsync()
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<List<string>>
            (await lcHttpClient.GetStringAsync("http://localhost:60065/api/bikesareus/GetBrandNames/"));
        }

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

        internal async static Task<clsOrder> GetOrderAsync(string prCustomer, int prSerial)
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<clsOrder>
                    (await lcHttpClient.GetStringAsync($"http://localhost:60065/api/bikesareus/GetOrder?Serial={prSerial}&Customer={prCustomer}"));
        }

        internal async static Task<clsOrder> GetOrderAsync(string prCustomer)
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<clsOrder>
                    (await lcHttpClient.GetStringAsync($"http://localhost:60065/api/bikesareus/GetOrder?Customer={prCustomer}"));
        }

        private async static Task<string> InsertOrUpdateAsync<TItem>(TItem prItem, string prUrl, string prRequest)
        {
            using (HttpRequestMessage lcReqMessage = new HttpRequestMessage(new HttpMethod(prRequest), prUrl))
            using (lcReqMessage.Content = new StringContent(JsonConvert.SerializeObject(prItem), Encoding.UTF8, "application/json"))
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.SendAsync(lcReqMessage);
                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }

        internal async static Task<string> InsertOrderAsync(clsOrder prOrder)
        {
            return await InsertOrUpdateAsync(prOrder, "http://localhost:60065/api/bikesareus/PostOrder", "POST");
        }
    }
}
