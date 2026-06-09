using DTOS.Columna;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicio;
using Servicio.Interfaces;

namespace Proyecto1.Controllers
{
    public class ColumnaController : Controller
    {

        private IColumnaServicio _columnaServicio;
       

        public ColumnaController(IColumnaServicio columnaServicio)
        {
            _columnaServicio = columnaServicio;
        }

        [HttpPost]
        public IActionResult Agregar(CrearColumnaViewModel columnaViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Detalle","Tablero", new { id = columnaViewModel.tableroId });
            }
            try
            {
                _columnaServicio.AgregarColumna(columnaViewModel);
                return RedirectToAction("Detalle", "Tablero", new { id = columnaViewModel.tableroId });
            }
            catch(Exception ex) 
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public ActionResult Editar(ColumnaViewModel columnaViewModel)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Detalle","Tablero", new { id = columnaViewModel.IdTablero });
            }
            try
            {
                _columnaServicio.EditarColumna(columnaViewModel);
                return RedirectToAction("Detalle", "Tablero", new { id = columnaViewModel.IdTablero });
            }
            catch
            {
                return RedirectToAction("Detalle", "Tablero", new { id = columnaViewModel.IdTablero });
            }
        }


        [HttpPost]        
        public IActionResult Eliminar(ColumnaViewModel columnaViewModel)
        {
            try
            {
                _columnaServicio.EliminarColumna(columnaViewModel.Id);
                return RedirectToAction("Detalle", "Tablero", new { id = columnaViewModel.IdTablero });
            }
            catch
            {
                return RedirectToAction("Detalle", "Tablero", new { id = columnaViewModel.IdTablero });
            }
        }

        [HttpPost]
        public IActionResult Reordenar([FromBody] ReordenarColumna request)
        {
            try
            {
                _columnaServicio.ReordenarColumna(request);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
