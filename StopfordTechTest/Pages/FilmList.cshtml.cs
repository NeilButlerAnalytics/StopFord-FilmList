using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StopfordTechTest.Model;
using System;

namespace StopfordTechTest.Pages
{
    public class FilmListModel : PageModel
    {

        public List<FilmModel> Films { get; set; }


        public async Task OnGetAsync()
        {
            Task<string> jsonString = null;
            List<FilmModel> filmList = new List<FilmModel>();


            using (var client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync("https://localhost:7127/api/Film/ReturnFilmList"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            // Get contents of page as a String.
                            jsonString = content.ReadAsStringAsync();
                            jsonString.Wait();
                            filmList = JsonConvert.DeserializeObject<List<FilmModel>>(jsonString.Result);
                            Films = filmList;
                        }
                    }
                    else
                    {
                        throw new Exception("An error has occured");
                    }
                }
            }
        }

        // OnPost changed to search for name initially
        // I have added further searching options, user can also search for a film by rating.
        public void OnPost(string search, int? rating)
        {
            Task<string> jsonString = null;
            List<FilmModel> filmList = new List<FilmModel>();

            using (var client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync("https://localhost:7127/api/Film/ReturnFilmList").Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            jsonString = content.ReadAsStringAsync();
                            filmList = JsonConvert.DeserializeObject<List<FilmModel>>(jsonString.Result);
                        }
                    }
                    else
                    {
                        throw new Exception("An error has occurred");
                    }
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                filmList = filmList.FindAll(f => f.FilmName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            if (rating.HasValue)
            {
                filmList = filmList.FindAll(f => f.Rating == rating.Value);
            }

                Films = filmList;
        }
    }
}
