using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using StopfordTechTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StopfordTechTest.Pages
{
    public class FilmListModel : PageModel
    {
        // New static fields for caching
        private static List<FilmModel> CachedFilms { get; set; } // New
        private static DateTime CacheTimestamp { get; set; } // New
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10); // New

        public List<FilmModel> Films { get; set; }

        public async Task OnGetAsync()
        {
            // Use cache if valid, otherwise fetch from API
            if (CachedFilms == null || DateTime.Now - CacheTimestamp > CacheDuration) // New
            {
                await GetFilmsAsync(); // Modified to use a new method
            }
            Films = CachedFilms; // New
        }

        public async Task OnPostAsync(string search, int? rating) // Modified to async
        {
            // Use cache if valid, otherwise fetch from API
            if (CachedFilms == null || DateTime.Now - CacheTimestamp > CacheDuration) // New
            {
                await GetFilmsAsync(); // Modified to use a new method
            }

            var filteredFilms = CachedFilms;

            if (!string.IsNullOrEmpty(search))
            {
                filteredFilms = filteredFilms.Where(f => f.FilmName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (rating.HasValue)
            {
                filteredFilms = filteredFilms.Where(f => f.Rating == rating.Value).ToList();
            }

            Films = filteredFilms;
        }

        // New method to fetch films and update the cache
        private async Task GetFilmsAsync() // New
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:7127/api/Film/ReturnFilmList");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    CachedFilms = JsonConvert.DeserializeObject<List<FilmModel>>(jsonString);
                    CacheTimestamp = DateTime.Now; // New
                }
                else
                {
                    throw new Exception("An error has occurred while fetching data from the API.");
                }
            }
        }
    }
}
