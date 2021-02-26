using Microsoft.AspNetCore.WebUtilities;
using MonkeyCache.SQLite;
using Newtonsoft.Json;
using Pokedex.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pokedex.Service
{
    public abstract class BaseService
    {

        protected readonly HttpClient _httpClient;
        protected readonly string BaseUri = "https://pokeapi.co/api/v2/";

        private JsonSerializerSettings serializerOptions;


        public BaseService()
        {
            _httpClient = new HttpClient();
            serializerOptions = new JsonSerializerSettings();
            
        }


        protected async Task<T> GetAsync<T>(string url)
        {
            var cached = Barrel.Current.Get<T>(url);
            
            if (cached != null)
                return cached;

            var response = await _httpClient.GetAsync(url.ToLowerInvariant());

            var responseJson = await response.Content.ReadAsStringAsync();

            var parsed = JsonConvert.DeserializeObject<T>(responseJson);

            Barrel.Current.Add(url, parsed, TimeSpan.FromDays(30));

            return parsed;
        }

        protected async Task<Paged<T>> GetPagedAsync<T>(string uri, int page = 1, int size = 20)
        {
            var fn = AddPaginationParamsToUrl(size, page == 1 ? null : (int?)(page - 1) * size);
            var url = fn(uri.ToLowerInvariant());

            var cached = Barrel.Current.Get<Paged<T>>(url);

            if (cached != null && cached.Results.Count > 0)
                return cached;

            var response = await _httpClient.GetAsync(url);

            var responseJson = await response.Content.ReadAsStringAsync();

            var parsed = JsonConvert.DeserializeObject<Paged<T>>(responseJson);

            Barrel.Current.Add(url, parsed, TimeSpan.FromDays(30));

            return parsed;
        }

        protected static Func<string, string> AddPaginationParamsToUrl(int? limit = null, int? offset = null)
        {
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();

            if (limit.HasValue)
            {
                queryParameters.Add(nameof(limit), limit.Value.ToString());
            }
            if (offset.HasValue)
            {
                queryParameters.Add(nameof(offset), offset.Value.ToString());
            }

            return uri => QueryHelpers.AddQueryString(uri, queryParameters);
        }

    }
}
