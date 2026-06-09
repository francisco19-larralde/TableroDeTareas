using Data;
using DTOS.Tarea;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestTablero.ServiciosTest
{
    public class ServicioTareaTest
    {
        private readonly DbContextOptions<TableroContext> _context;

        public ServicioTareaTest()
        {

            _context = new DbContextOptionsBuilder<TableroContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void AlAgregarUnaTareaDevuelveTrue()
        {
            using (var dbContext = new TableroContext(_context))
            {

                var servicioTarea = new Servicio.TareaServicio(dbContext);
                var columna = new Columna { nombre = "Columna 1", descripcion = "Descripción de la columna 1" };
                dbContext.Columnas.Add(columna);
                dbContext.SaveChanges();

                var tareaViewModel = new CrearTareaViewModel
                {
                    titulo = "Tarea 1",
                    columnaId = columna.Id
                };

                var resultado = servicioTarea.AgregarTarea(tareaViewModel);


                Assert.True(resultado);
            }
        }

        [Fact]
        public void AlAgregarUnaTareaDevuelveFalsePorColumnaInexistente()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var servicioTarea = new Servicio.TareaServicio(dbContext);
                var tareaViewModel = new CrearTareaViewModel
                {
                    titulo = "Tarea 1",
                    columnaId = 999
                };
                var resultado = servicioTarea.AgregarTarea(tareaViewModel);
                Assert.False(resultado);
            }
        }

        [Fact]
        public void AlCambiarEstadoDeUnaTareaDevuelveTrue()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var servicioTarea = new Servicio.TareaServicio(dbContext);

                var columna = new Columna { nombre = "Columna 1", descripcion = "Descripción de la columna 1" };
                dbContext.Columnas.Add(columna);
                dbContext.SaveChanges();

                var tarea = new Tarea { nombre = "Tarea 1", descripcion = "Descripción de la tarea 1", idColumna = columna.Id, orden = 0, completada = false };
                dbContext.Tareas.Add(tarea);
                dbContext.SaveChanges();

                var resultado = servicioTarea.CambiarEstadoTarea(1);

                Tarea tareaEditada = dbContext.Tareas.Find(1);

                Assert.True(resultado);
                Assert.True(tareaEditada.completada);
            }
        }

        [Fact]
        public void AlCambiarEstadoDeUnaTareaDevuelveFalsePorTareaInexistente()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var servicioTarea = new Servicio.TareaServicio(dbContext);
                var resultado = servicioTarea.CambiarEstadoTarea(999);
                Assert.False(resultado);
            }
        }

        [Fact]
        public void AlEliminarUnaTareaDevuelveTrue()
        {
            using (var dbContext = new TableroContext(_context))
            {

                var servicioTarea = new Servicio.TareaServicio(dbContext);

                var columna = new Columna { nombre = "Columna 1", descripcion = "Descripción de la columna 1" };
                dbContext.Columnas.Add(columna);
                dbContext.SaveChanges();

                var tarea = new Tarea { nombre = "Tarea 1", descripcion = "Descripción de la tarea 1", idColumna = columna.Id, orden = 0, completada = false };
                dbContext.Tareas.Add(tarea);
                dbContext.SaveChanges();

                var resultado = servicioTarea.EliminarTarea(1);

                Tarea tareaEliminada = dbContext.Tareas.Find(1);
                Assert.True(resultado);
                Assert.Null(tareaEliminada);
            }
        }

        [Fact]
        public void AlEliminarUnaTareaDevuelveFalsePorTareaInexistente()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var servicioTarea = new Servicio.TareaServicio(dbContext);
                var resultado = servicioTarea.EliminarTarea(999);
                Assert.False(resultado);
            }
        }

        [Fact]
        public void AlEditarUnaTareaDevuelveTrue()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var servicioTarea = new Servicio.TareaServicio(dbContext);

                var columna = new Columna { nombre = "Columna 1", descripcion = "Descripción de la columna 1" };
                dbContext.Columnas.Add(columna);
                dbContext.SaveChanges();

                var tarea = new Tarea { nombre = "Tarea 1", descripcion = "Descripción de la tarea 1", idColumna = columna.Id, orden = 0, completada = false };
                dbContext.Tareas.Add(tarea);
                dbContext.SaveChanges();

                var tareaEditViewModel = new TareaViewModel
                {
                    id = 1,
                    Nombre = "Tarea Editada",
                    Descripcion = "Descripción editada"
                };

                var resultado = servicioTarea.EditarTarea(tareaEditViewModel);
                Tarea tareaEditada = dbContext.Tareas.Find(1);

                Assert.True(resultado);
                Assert.Equal("Tarea Editada", tareaEditada.nombre);
                Assert.Equal("Descripción editada", tareaEditada.descripcion);
            }
        }

        [Fact]
        public void AlEditarUnaTareaDevuelveFalsePorTareaInexistente()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var servicioTarea = new Servicio.TareaServicio(dbContext);
                var tareaEditViewModel = new TareaViewModel
                {
                    id = 999,
                    Nombre = "Tarea Editada",
                    Descripcion = "Descripción editada"
                };
                var resultado = servicioTarea.EditarTarea(tareaEditViewModel);
                Assert.False(resultado);
            }
        }

        [Fact]
        public void QueSePuedaReordenarTareas()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var servicioTarea = new Servicio.TareaServicio(dbContext);

                var columna1 = new Columna { nombre = "Columna 1", descripcion = "Desc 1" };
                var columna2 = new Columna { nombre = "Columna 2", descripcion = "Desc 2" };

                dbContext.Columnas.AddRange(columna1, columna2);
                dbContext.SaveChanges();

                var tarea1 = new Tarea { nombre = "Tarea 1", descripcion = "Desc", idColumna = columna1.Id, orden = 0 };
                var tarea2 = new Tarea { nombre = "Tarea 2", descripcion = "Desc", idColumna = columna1.Id, orden = 1 };
                var tarea3 = new Tarea { nombre = "Tarea 3", descripcion = "Desc", idColumna = columna2.Id, orden = 0 };

                dbContext.Tareas.AddRange(tarea1, tarea2, tarea3);
                dbContext.SaveChanges();

                var request = new ReordenarTarea
                {
                    ColumnaOrigenId = columna1.Id,
                    ColumnaDestinoId = columna2.Id,
                    IdsOrigen = new int[] { tarea2.id },
                    IdsDestino = new int[] { tarea3.id, tarea1.id }
                };

                servicioTarea.ReordenarTarea(request);

                var t1 = dbContext.Tareas.Find(tarea1.id);
                var t2 = dbContext.Tareas.Find(tarea2.id);
                var t3 = dbContext.Tareas.Find(tarea3.id);

                
                Assert.Equal(columna2.Id, t1.idColumna);
                Assert.Equal(columna2.Id, t3.idColumna);

                Assert.Equal(2, t1.orden); 
                Assert.Equal(1, t3.orden); 

                
                Assert.Equal(columna1.Id, t2.idColumna);
                Assert.Equal(1, t2.orden);
            }
        }

    }
}
