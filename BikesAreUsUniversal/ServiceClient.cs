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
    }
}
