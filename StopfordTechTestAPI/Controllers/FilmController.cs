using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StopfordTechTestAPI.Model;

namespace StopfordTechTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : Controller
    {

        [HttpGet("ReturnFilmList")]
        public ActionResult ReturnFilmList()
        {
            List<FilmModel> fml = new List<FilmModel>();
            Business.Film film = new Business.Film();
            Thread.Sleep(5000);
            fml = film.ReturnFilmList();

            return Ok(fml);
        }

        [HttpGet("ReturnFilmById")]
        public ActionResult ReturnFilmById(Int64 id)
        {
            FilmModel filmModel = new FilmModel();
            Business.Film film = new Business.Film();
            filmModel = film.ReturnFilmList().FirstOrDefault(f => f.FilmId == id);

            return Ok(filmModel);
        }

        
      
    }
}
