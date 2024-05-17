using StopfordTechTestAPI.Model;
using System.Linq.Expressions;

namespace StopfordTechTestAPI.Business
{
    public class Film
    {

        public List<FilmModel> ReturnFilmList()
        {

            List<FilmModel> fml = new List<FilmModel>();

            using (var reader = new StreamReader(@"Data\FilmData.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    FilmModel fm = new FilmModel();
                    fm.FilmId = Convert.ToInt64(values[0]);
                    fm.FilmName = values[1];
                    fm.Tagline = values[1];
                    fml.Add(fm);
                }
            }

            return fml;
        }

    }
}
