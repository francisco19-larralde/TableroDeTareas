using Data;
using DTOS.Tablero;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Servicio;
using Servicio.Interfaces;


namespace TestTablero.ServiciosTest
{
    public class ServicioTableroTest
    {
        private readonly DbContextOptions<TableroContext> _context;

        public ServicioTableroTest()
        {

            _context = new DbContextOptionsBuilder<TableroContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void QueSePuedanObtenerTodosLosTableros()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var usuarioFalso = new Model.Usuario { Id = 1, Nombre = "User1", mail = "user1@example.com", password = "123456" };

                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 1", Descripcion = "Descripción 1", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 2", Descripcion = "Descripción 2", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 3", Descripcion = "Descripción 3", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.SaveChanges();


                var servicio = new TableroServicio(dbContext);
                List<TableroViewModel> tablerosObtenidos = servicio.ObtenerTableros();


                Assert.NotNull(tablerosObtenidos);
                Assert.Equal(3, tablerosObtenidos.Count);          
                Assert.Contains(tablerosObtenidos, t => t.Titulo == "Tablero 1");
                Assert.Contains(tablerosObtenidos, t => t.Titulo == "Tablero 2");
                Assert.Contains(tablerosObtenidos, t => t.Titulo == "Tablero 3");
            }
        }

        [Fact]
        public void QueSePuedaCrearUnTablero()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var usuarioFalso = new Model.Usuario { Id = 1, Nombre = "User1", mail = "user1@example.com", password = "123456" };

                TableroViewModel tableroNuevo = new TableroViewModel { Titulo = "Tablero Nuevo", Descripcion = "Descripción del tablero nuevo" };

                var servicio = new TableroServicio(dbContext);
                bool resultado = servicio.CrearTablero(tableroNuevo);
                var cantTableros = servicio.ObtenerTableros().Count;

                Assert.True(resultado);
                Assert.Equal(1, cantTableros);
            }
        }


        [Fact]
        public void QueSePuedaEditarUnTablero()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var usuarioFalso = new Model.Usuario { Id = 1, Nombre = "User1", mail = "user1@example.com", password = "123456" };

                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 1", Descripcion = "Descripción 1", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 2", Descripcion = "Descripción 2", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 3", Descripcion = "Descripción 3", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.SaveChanges();

                var servicio = new TableroServicio(dbContext);
                servicio.EditarTablero(new TableroViewModel { Id = 1, Titulo = "Tablero Editado", Descripcion = "Descripción del tablero editado" });

                DetalleTableroViewModel tableroEditado = servicio.ObtenerTablero(1);

                Assert.Equal("Tablero Editado", tableroEditado.Titulo);
                Assert.Equal("Descripción del tablero editado", tableroEditado.Descripcion);
            }
        }

        [Fact]
        public void QueSePuedaEliminarUnTablero()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var usuarioFalso = new Model.Usuario { Id = 1, Nombre = "User1", mail = "user1@example.com", password = "123456" };

                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 1", Descripcion = "Descripción 1", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 2", Descripcion = "Descripción 2", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 3", Descripcion = "Descripción 3", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.SaveChanges();

                var servicio = new TableroServicio(dbContext);
                var resultado = servicio.EliminarTablero(1);
                List<TableroViewModel> tablerosRestantes = servicio.ObtenerTableros();

                Assert.True(resultado);
                Assert.Equal(2, tablerosRestantes.Count);
                Assert.Equal("Tablero 2", tablerosRestantes[0].Titulo);
                Assert.Equal("Tablero 3", tablerosRestantes[1].Titulo);
            }
        }

        [Fact]
        public void QueNoSePuedaEliminarUnTableroInexistente()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var usuarioFalso = new Model.Usuario { Id = 1, Nombre = "User1", mail = "user1@example.com", password = "123456" };

                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 1", Descripcion = "Descripción 1", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 2", Descripcion = "Descripción 2", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 3", Descripcion = "Descripción 3", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.SaveChanges();

                var servicio = new TableroServicio(dbContext);
                var resultado = servicio.EliminarTablero(5);

                Assert.False(resultado);

            }

        }

        [Fact]
        public void QueSePuedaObtenerUnTableroPorId()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var usuarioFalso = new Model.Usuario { Id = 1, Nombre = "User1", mail = "user1@example.com", password = "123456" };

                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 1", Descripcion = "Descripción 1", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 2", Descripcion = "Descripción 2", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 3", Descripcion = "Descripción 3", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.SaveChanges();

                var servicio = new TableroServicio(dbContext);
                DetalleTableroViewModel tableroObtenido = servicio.ObtenerTablero(2);

                Assert.Equal("Tablero 2", tableroObtenido.Titulo);
                Assert.Equal("Descripción 2", tableroObtenido.Descripcion);
            }

        }

        [Fact]
        public void QueNoSePuedaObtenerUnTableroInexistentePorId()
        {
            using (var dbContext = new TableroContext(_context))
            {
                var usuarioFalso = new Model.Usuario { Id = 1, Nombre = "User1", mail = "user1@example.com", password = "123456" };

                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 1", Descripcion = "Descripción 1", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 2", Descripcion = "Descripción 2", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.Tableros.Add(new Model.Tablero { Name = "Tablero 3", Descripcion = "Descripción 3", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1, UsuarioCreador = usuarioFalso, Columnas = new List<Model.Columna>() });
                dbContext.SaveChanges();

                var servicio = new TableroServicio(dbContext);
                DetalleTableroViewModel tableroObtenido = servicio.ObtenerTablero(5);

                Assert.Null(tableroObtenido);
            }

        }


    }
}



