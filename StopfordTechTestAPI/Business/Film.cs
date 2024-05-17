using StopfordTechTestAPI.Model;
using System.Linq.Expressions;

namespace StopfordTechTestAPI.Business
{
    public class Film
    {

        public List<FilmModel> ReturnFilmList()
        {

            List<FilmModel> fml = new List<FilmModel>();
            try
            {

                using (var reader = new StreamReader(@"Data\FilmData.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        FilmModel fm = new FilmModel();
                        fm.FilmId = Convert.ToInt64(values[0]);
                        fm.FilmName = values[1];
                        fm.Tagline = values[2];
                        // Check if the rating value is in a valid numeric format
                        if (int.TryParse(values[3], out int rating))
                        {
                            fm.Rating = rating;
                        }
                        else
                        {
                            // If the rating is not in the correct format then default to 0
                            fm.Rating = 0;
                        }
                        fml.Add(fm);
                    }
                }
            }catch (Exception ex)
            {
                // Handle any exceptions that occur during reading or parsing
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return fml;
        }

    }
}
