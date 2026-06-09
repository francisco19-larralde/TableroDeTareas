using Data;
using DTOS.Tablero;
using DTOS.Tarea;
using Microsoft.EntityFrameworkCore;
using Model;
using Servicio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Servicio
{
    public class TableroServicio : ITableroServicio
    {
        private readonly TableroContext _context;

        public TableroServicio(TableroContext context)
        {
            _context = context;
        }

        public List<TableroViewModel> ObtenerTableros()
        {
            return _context.Tableros.Select(t => new TableroViewModel
            {
                Id = t.Id,
                Titulo = t.Name,
                Descripcion = t.Descripcion,
                
            }).ToList();
        }

        public bool CrearTablero(TableroViewModel tableroViewModel)
        {
            Tablero tablero = new Tablero
            {
               Name = tableroViewModel.Titulo,
               Descripcion = tableroViewModel.Descripcion,
               FechaCreacion = DateTime.Now,
               IdUsuarioCreador = 1,          
               Columnas = new List<Columna>()             
            };

            tablero.Columnas.Add(new Columna { nombre = "Por Hacer", descripcion = "Tareas por hacer", Tablero = tablero,orden=1 });
            tablero.Columnas.Add(new Columna { nombre = "En Progreso", descripcion = "Tareas en progreso", Tablero = tablero,orden=2 });
            tablero.Columnas.Add(new Columna { nombre = "Hecho", descripcion = "Tareas completadas", Tablero = tablero,orden=3 });

            _context.Tableros.Add(tablero);
            _context.SaveChanges();
            return true;
        }

        public bool EditarTablero(TableroViewModel tableroViewModel)
        {
            Tablero tablero = _context.Tableros.FirstOrDefault(t => t.Id == tableroViewModel.Id);
            if (tablero == null)
                return false;

            tablero.Name = tableroViewModel.Titulo;
            tablero.Descripcion = tableroViewModel.Descripcion;
            _context.SaveChanges();
            return true;
        }

        public bool EliminarTablero(int id)
        {
            Tablero tablero = _context.Tableros.FirstOrDefault(t => t.Id == id);
            if (tablero == null)
                return false;

            _context.Tableros.Remove(tablero);
            _context.SaveChanges();
            return true;
        }

        public DetalleTableroViewModel ObtenerTablero(int id)
        {
            var tablero = _context.Tableros
                .Include(t => t.Columnas)
                    .ThenInclude(c => c.Tareas)
                .FirstOrDefault(t => t.Id == id);

            if (tablero == null)
                return null;

            return new DetalleTableroViewModel
            {
                Id = tablero.Id,
                Titulo = tablero.Name,
                Descripcion = tablero.Descripcion,

                Columnas = tablero.Columnas
                    .OrderBy(c => c.orden) 
                    .Select(c => new DTOS.Columna.ColumnaViewModel
                    {
                        Id = c.Id,
                        Nombre = c.nombre,
                        Descripcion = c.descripcion,
                        IdTablero = c.idTablero,

                        Tareas = c.Tareas
                            .OrderBy(t => t.orden) 
                            .Select(t => new TareaViewModel
                            {
                                id = t.id,
                                Nombre = t.nombre,
                                Descripcion = t.descripcion,
                                Completada = t.completada,
                                IdColumna = t.idColumna,
                                orden = t.orden
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }
    }
}
