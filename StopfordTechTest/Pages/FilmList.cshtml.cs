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



        public void OnPost(string search)
        {
            Task<string> jsonString = null; // To hold the JSON string fetched from the API
            List<FilmModel> filmList = new List<FilmModel>(); // Empty list initialised to store the film data

            using (var client = new HttpClient())
            {
                //Sends a GET request to the URL to fetch the list of films.
                using (HttpResponseMessage response = client.GetAsync("https://localhost:7127/api/Film/ReturnFilmList").Result)
                {
                    // Only progress if request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            jsonString = content.ReadAsStringAsync(); // Read the response content
                            filmList = JsonConvert.DeserializeObject<List<FilmModel>>(jsonString.Result); // Converts JSON string to FilmList
                        }
                    }
                    else
                    {
                        throw new Exception("An error has occurred");
                    }
                }
            }
            // Just a User error check, if the user enters a search then we can display a search result
            // But if no serach is specified then just assign FilmList.
            // This ensures that the Films property always contains the appropriate data to display.
            if (!string.IsNullOrEmpty(search))
            {
                Films = filmList.FindAll(f => f.FilmName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                Films = filmList;
            }
        }
    }
}
