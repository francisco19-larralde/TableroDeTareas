using Data;
using DTOS.Columna;
using Microsoft.EntityFrameworkCore;
using Model;
using Servicio;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestTablero.ServiciosTest
{
    public class ServicioColumnaTest
    {
        private readonly DbContextOptions<TableroContext> _context;

        public ServicioColumnaTest()
        {

            _context = new DbContextOptionsBuilder<TableroContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void QueSePuedaAgregarUnaColumnaDevuelveTrue()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var usuarioFalso = new Model.Usuario { Id = 1, Nombre = "User1", mail = "user1@example.com", password = "123456" };

                dbContext.Tableros.Add(new Model.Tablero
                {
                    Name = "Tablero 1",
                    Descripcion = "Descripción 1",
                    FechaCreacion = DateTime.Now,
                    IdUsuarioCreador = 1,
                    UsuarioCreador = usuarioFalso,
                    Columnas = new List<Model.Columna>()
                });
                dbContext.SaveChanges();

                var servicio = new ColumnaServicio(dbContext);
                var resultado = servicio.AgregarColumna(new CrearColumnaViewModel
                {
                    nombre = "Columna 1",
                    tableroId = 1
                });

                var cantColumnas = dbContext.Columnas.Count();

                Assert.True(resultado);
                Assert.Equal(1, cantColumnas);
            }
        }

        [Fact]
        public void QueAlAgregarUnaColumnaAUnTableroInexistenteDevuelveFalse()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var servicio = new ColumnaServicio(dbContext);
                var resultado = servicio.AgregarColumna(new CrearColumnaViewModel
                {
                    nombre = "Columna 1",
                    tableroId = 999
                });
                var cantColumnas = dbContext.Columnas.Count();
                Assert.False(resultado);
                Assert.Equal(0, cantColumnas);
            }
        }

        [Fact]
        public void QueSePuedaEliminarUnaColumnaDevuelveTrue()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var usuarioFalso = new Model.Usuario { Id = 1, Nombre = "User1", mail = "user1@example.com", password = "123456" };

                dbContext.Tableros.Add(new Model.Tablero
                {
                    Name = "Tablero 1",
                    Descripcion = "Descripción 1",
                    FechaCreacion = DateTime.Now,
                    IdUsuarioCreador = 1,
                    UsuarioCreador = usuarioFalso,
                    Columnas = new List<Model.Columna>()
                });

                dbContext.Columnas.Add(new Model.Columna
                {
                    nombre = "Columna 1",
                    descripcion = "Descripción Columna 1",
                    orden = 1,
                    idTablero = 1
                });

                dbContext.SaveChanges();

                var servicio = new ColumnaServicio(dbContext);
                var resultado = servicio.EliminarColumna(1);

                Assert.True(resultado);
                Assert.Equal(0, dbContext.Columnas.Count());

            }

        }

        [Fact]
        public void QueAlEliminarUnaColumnaInexistenteDevuelveFalse()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var usuarioFalso = new Model.Usuario { Id = 1, Nombre = "User1", mail = "user1@example.com", password = "123456" };

                dbContext.Tableros.Add(new Model.Tablero
                {
                    Name = "Tablero 1",
                    Descripcion = "Descripción 1",
                    FechaCreacion = DateTime.Now,
                    IdUsuarioCreador = 1,
                    UsuarioCreador = usuarioFalso,
                    Columnas = new List<Model.Columna>()
                });

                dbContext.Columnas.Add(new Model.Columna
                {
                    nombre = "Columna 1",
                    descripcion = "Descripción Columna 1",
                    orden = 1,
                    idTablero = 1
                });

                dbContext.SaveChanges();

                var servicio = new ColumnaServicio(dbContext);
                var resultado = servicio.EliminarColumna(2);

                Assert.False(resultado);
                Assert.Equal(1, dbContext.Columnas.Count());
            }
        }

        [Fact]
        public void QueSePuedaEditarUnaColumnaDevuelveTrue()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var usuarioFalso = new Model.Usuario { Id = 1, Nombre = "User1", mail = "user1@example.com", password = "123456" };

                dbContext.Tableros.Add(new Model.Tablero
                {
                    Name = "Tablero 1",
                    Descripcion = "Descripción 1",
                    FechaCreacion = DateTime.Now,
                    IdUsuarioCreador = 1,
                    UsuarioCreador = usuarioFalso,
                    Columnas = new List<Model.Columna>()
                });

                dbContext.Columnas.Add(new Model.Columna
                {
                    nombre = "Columna 1",
                    descripcion = "Descripción Columna 1",
                    orden = 1,
                    idTablero = 1
                });

                dbContext.SaveChanges();

                var servicio = new ColumnaServicio(dbContext);
                var resultado = servicio.EditarColumna(new ColumnaViewModel
                {
                    Id = 1,
                    Nombre = "Columna 1 Editada",
                    Descripcion = "Descripción Columna 1 Editada"
                });

                Columna columnaEditada = dbContext.Columnas
                    .Where(c => c.Id == 1)
                    .Select(c => new Columna
                    {
                        Id = c.Id,
                        nombre = c.nombre,
                        descripcion = c.descripcion,
                        orden = c.orden
                    })
                    .FirstOrDefault();

                Assert.True(resultado);
                Assert.Equal("Columna 1 Editada", columnaEditada.nombre);
                Assert.Equal("Descripción Columna 1 Editada", columnaEditada.descripcion);
            }
        }

        [Fact]
        public void AlEditarUnaColumnaInexistenteDevuevleFalse()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var usuarioFalso = new Model.Usuario { Id = 1, Nombre = "User1", mail = "user1@example.com", password = "123456" };

                dbContext.Tableros.Add(new Model.Tablero
                {
                    Name = "Tablero 1",
                    Descripcion = "Descripción 1",
                    FechaCreacion = DateTime.Now,
                    IdUsuarioCreador = 1,
                    UsuarioCreador = usuarioFalso,
                    Columnas = new List<Model.Columna>()
                });

                dbContext.Columnas.Add(new Model.Columna
                {
                    nombre = "Columna 1",
                    descripcion = "Descripción Columna 1",
                    orden = 1,
                    idTablero = 1
                });

                dbContext.SaveChanges();

                var servicio = new ColumnaServicio(dbContext);
                var resultado = servicio.EditarColumna(new ColumnaViewModel
                {
                    Id = 999,
                    Nombre = "Columna 1 Editada",
                    Descripcion = "Descripción Columna 1 Editada"
                });

                Assert.False(resultado);
            }
        }

        [Fact]
        public void QueSePuedaReordenarLasColumnas()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var usuarioFalso = new Model.Usuario { Id = 1, Nombre = "User1", mail = "user1@example.com", password = "123456" };

                dbContext.Tableros.Add(new Model.Tablero
                {
                    Name = "Tablero 1",
                    Descripcion = "Descripción 1",
                    FechaCreacion = DateTime.Now,
                    IdUsuarioCreador = 1,
                    UsuarioCreador = usuarioFalso,
                    Columnas = new List<Model.Columna>()
                });

                dbContext.Columnas.Add(new Model.Columna
                {
                    nombre = "Columna 1",
                    descripcion = "Descripción Columna 1",
                    orden = 1,
                    idTablero = 1
                });

                dbContext.Columnas.Add(new Model.Columna
                {
                    nombre = "Columna 2",
                    descripcion = "Descripción Columna 2",
                    orden = 2,
                    idTablero = 1
                });

                dbContext.Columnas.Add(new Model.Columna
                {
                    nombre = "Columna 3",
                    descripcion = "Descripción Columna 3",
                    orden = 3,
                    idTablero = 1
                });

                dbContext.SaveChanges();


                var servicio = new ColumnaServicio(dbContext);

                servicio.ReordenarColumna(new ReordenarColumna
                {
                    TableroId = 1,
                    Ids = new List<int> { 3, 1, 2 }
                });

                var columnasReordenadas = dbContext.Columnas
                    .Where(c => c.idTablero == 1)
                    .OrderBy(c => c.orden)
                    .ToList();

                Assert.Equal(3, columnasReordenadas.Count);
                Assert.Equal(3, columnasReordenadas[0].Id);
                Assert.Equal(1, columnasReordenadas[1].Id);
                Assert.Equal(2, columnasReordenadas[2].Id);
            }
        }

    }
}
