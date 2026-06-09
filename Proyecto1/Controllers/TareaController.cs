using DTOS.Columna;
using DTOS.Tarea;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicio;
using Servicio.Interfaces;

namespace Proyecto1.Controllers
{
    public class TareaController : Controller
    {
        private ITareaServicio _tareaServicio;

        public TareaController(ITareaServicio tareaServicio)
        {
            _tareaServicio = tareaServicio;
        }


        
        [HttpPost]
        public ActionResult Agregar(CrearTareaViewModel tareaViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Detalle", "Tablero", new { id = tareaViewModel.tableroId });
            }
            try
            {
                _tareaServicio.AgregarTarea(tareaViewModel);
                return RedirectToAction("Detalle", "Tablero", new { id = tareaViewModel.tableroId });
            }
            catch
            {
                return RedirectToAction("Detalle", "Tablero", new { id = tareaViewModel.tableroId });
            }
        }


        [HttpPost]
        public IActionResult CambiarEstadoTarea(int tareaId,int tableroId)
        {
            try
            {
                _tareaServicio.CambiarEstadoTarea(tareaId);
                return RedirectToAction("Detalle", "Tablero", new { id = tableroId });
            }
            catch
            {
                return RedirectToAction("Detalle", "Tablero", new { id = tableroId });
            }

        }


        
        [HttpPost]
        public ActionResult EditarTarea(TareaViewModel tareaViewModel)
        {
            try
            {
                _tareaServicio.EditarTarea(tareaViewModel);
                return RedirectToAction("Detalle", "Tablero", new { id = tareaViewModel.IdTablero });
            }
            catch
            {
                return RedirectToAction("Detalle", "Tablero", new { id = tareaViewModel.IdTablero });
            }
        }

        
        [HttpPost]
        public IActionResult Eliminar(int tareaId, int tableroId)
        {
            try
            {   
                _tareaServicio.EliminarTarea(tareaId);
                return RedirectToAction("Detalle", "Tablero", new { id = tableroId });
            }
            catch
            {
                return RedirectToAction("Detalle", "Tablero", new { id = tableroId });
            }
        }

        [HttpPost]
        public IActionResult Reordenar([FromBody] ReordenarTarea request)
        {
            try
            {
                _tareaServicio.ReordenarTarea(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
