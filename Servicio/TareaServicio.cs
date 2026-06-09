using Data;
using DTOS.Columna;
using DTOS.Tarea;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Model;
using Servicio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Servicio
{
    public class TareaServicio : ITareaServicio
    {
        private readonly TableroContext _context;

        public TareaServicio(TableroContext context)
        {
            _context = context;
        }

        public bool AgregarTarea(CrearTareaViewModel tareaViewModel)
        {
            Columna columna = _context.Columnas.Find(tareaViewModel.columnaId);
            if (columna == null)
            {
                return false;
            }

            var ultimoOrden = _context.Tareas
               .Where(c => c.idColumna == columna.Id)
               .Select(c => (int?)c.orden)
               .Max() ?? -1;

            Tarea tarea = new Tarea
            {
                nombre = tareaViewModel.titulo,
                descripcion = "Descripcion de la tarea",
                idColumna = columna.Id,
                orden = ultimoOrden + 1,
            };


            _context.Tareas.Add(tarea);
            _context.SaveChanges();

            return true;
        }

        public bool CambiarEstadoTarea(int tareaId)
        {
            Tarea tareaEncontrada = _context.Tareas.Find(tareaId);
            if (tareaEncontrada == null)
            {
                return false;
            }

            tareaEncontrada.completada = !tareaEncontrada.completada;
            _context.SaveChanges();

            return true;
        }

        public bool EliminarTarea(int tareaId)
        {
            Tarea tareaEncontrada = _context.Tareas.Find(tareaId);
            if (tareaEncontrada == null)
            {
                return false;
            }
            _context.Tareas.Remove(tareaEncontrada);
            _context.SaveChanges();
            return true;
        }

        public bool EditarTarea(TareaViewModel tareaViewModel)
        {
            Tarea tareaEncontrada = _context.Tareas.Find(tareaViewModel.id);
            if (tareaEncontrada == null)
            {
                return false;
            }
            tareaEncontrada.nombre = tareaViewModel.Nombre;
            tareaEncontrada.descripcion = tareaViewModel.Descripcion;
            _context.SaveChanges();
            return true;
        }

        public void ReordenarTarea(ReordenarTarea request)
        {
            if (request.IdsDestino != null)
            {
                for (int i = 0; i < request.IdsDestino.Length; i++)
                {
                    var tarea = _context.Tareas.First(t => t.id == request.IdsDestino[i]);
                    tarea.idColumna = request.ColumnaDestinoId.Value;
                    tarea.orden = i + 1;
                }
            }

            if (request.IdsOrigen != null)
            {
                for (int i = 0; i < request.IdsOrigen.Length; i++)
                {
                    var tarea = _context.Tareas.First(t => t.id == request.IdsOrigen[i]);
                    tarea.idColumna = request.ColumnaOrigenId.Value;
                    tarea.orden = i + 1;
                }
            }

            _context.SaveChanges();
        }
    }
}
