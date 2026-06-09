using Microsoft.AspNetCore.Mvc;
using Servicio.Interfaces;
using Model;
using DTOS.Tablero;

namespace Proyecto1.Controllers
{
    public class TableroController : Controller
    {

        private ITableroServicio _tableroServicio;

        public TableroController(ITableroServicio tableroServicio)
        {
            _tableroServicio = tableroServicio;
        }

        public IActionResult Index()
        {
            List<TableroViewModel> tableros = _tableroServicio.ObtenerTableros();
            return View("Index",tableros);
        }

        [HttpPost]
        public IActionResult Create(TableroViewModel tableroViewModel)
        {
            if (!ModelState.IsValid)
            {
                
                return RedirectToAction("Index");
            }

            try
            {
                
                _tableroServicio.CrearTablero(tableroViewModel);
                return RedirectToAction("Index");
            }
            catch
            {
                
                return RedirectToAction("Index");
            }
        }


        public IActionResult Detalle(int id)
        {
            try
            {
                DetalleTableroViewModel tablero = _tableroServicio.ObtenerTablero(id);
                if(tablero == null)
                {
                    return RedirectToAction("Index");
                }

                return View("Detalle", tablero);
            }
            catch
            {
                return View("Index");
            }
        }

        
        [HttpPost]
        
        public IActionResult Editar(TableroViewModel tableroViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View("Editar", tableroViewModel);
            }

            try
            {
                _tableroServicio.EditarTablero(tableroViewModel);
                return RedirectToAction("Detalle", new { id = tableroViewModel.Id });
            }
            catch
            {
                
                return View("Editar", tableroViewModel);
            }
        }

        [HttpPost]
        
        public IActionResult Borrar(int id)
        {
            try
            {
                _tableroServicio.EliminarTablero(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index");
            }
        }
    }
}
