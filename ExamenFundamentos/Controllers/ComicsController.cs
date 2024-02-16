using ExamenFundamentos.Models;
using ExamenFundamentos.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamenFundamentos.Controllers
{
    public class ComicsController : Controller
    {
        private IRepositoryComics repo;
        public ComicsController(IRepositoryComics repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {   
            List<Comic> comics = this.repo.GetComics();
            return View(comics);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Comic comic)
        {
            this.repo.InsertComic(comic.IdComic, comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }
        public IActionResult CreateProcedure()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProcedure(Comic comic)
        {
            this.repo.InsertComicProcedure( comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }
        public IActionResult DatosComics()
        {
            ViewData["NOMBRES"] = this.repo.GetNombres();
            return View();
        }
        [HttpPost]
        public IActionResult DatosComics(string nombre)
        {
            ViewData["NOMBRES"] = this.repo.GetNombres();
            Comic comic = this.repo.GetComicsNombre(nombre);
            return View(comic);
        }
        public IActionResult Delete(int idcomic)
        {
            Comic comic = this.repo.GetComicsID(idcomic);
            return View(comic);
        }
        [HttpPost]
        public IActionResult DeletePost(int idcomic)
        {
        
            this.repo.DeleteComic(idcomic);
            return RedirectToAction("Index");
        }
    }
}
