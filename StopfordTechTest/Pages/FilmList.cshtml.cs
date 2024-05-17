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
            
            //search for name of film here


        }
    }
}
