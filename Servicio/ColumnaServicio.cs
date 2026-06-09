using Data;
using DTOS.Columna;
using Microsoft.EntityFrameworkCore;
using Model;
using Servicio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Servicio
{
    public class ColumnaServicio : IColumnaServicio
    {
        private readonly TableroContext _context;

        public ColumnaServicio(TableroContext context)
        {
            _context = context;
        }

        public bool AgregarColumna(CrearColumnaViewModel columnaViewModel)
        {
            Tablero tablero = _context.Tableros
                .Include(t => t.Columnas)
                .FirstOrDefault(t => t.Id == columnaViewModel.tableroId);
            if (tablero == null)
            {
                return false;
            }

            var ultimoOrden = _context.Columnas
                .Where(c => c.idTablero == tablero.Id) 
                .Select(c => (int?)c.orden)
                .Max() ?? -1;

            tablero.Columnas.Add(new Columna
            {
                nombre = columnaViewModel.nombre,
                descripcion = "",
                idTablero = columnaViewModel.tableroId,
                orden = ultimoOrden + 1
            });

            _context.SaveChanges();

            return true;
        }

        public bool EliminarColumna(int id)
        {
            Columna columna = _context.Columnas.FirstOrDefault(c => c.Id == id);
            if (columna == null)
            {
                return false;
            }
            _context.Columnas.Remove(columna);
            _context.SaveChanges();
            return true;
        }

        public bool EditarColumna(ColumnaViewModel columnaViewModel)
        {
            Columna columna = _context.Columnas.FirstOrDefault(c => c.Id == columnaViewModel.Id);
            if (columna == null)
            {
                return false;
            }
            columna.nombre = columnaViewModel.Nombre;
            columna.descripcion = columnaViewModel.Descripcion ?? "";
            _context.SaveChanges();
            return true;
        }

        public void ReordenarColumna(ReordenarColumna request)
        {
            var columnas = _context.Columnas
                .Where(c => c.idTablero == request.TableroId)
                .OrderBy(c => c.orden)
                .ToList();

            for (int i = 0; i < request.Ids.Count; i++)
            {
                var columna = columnas.FirstOrDefault(c => c.Id == request.Ids[i]);
                if (columna != null)
                {
                    columna.orden = i + 1;
                }
            }

            _context.SaveChanges();
        }

    }
}
