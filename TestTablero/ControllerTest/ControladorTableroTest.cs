using DTOS.Tablero;
using Microsoft.AspNetCore.Mvc;
using Model;
using Moq;
using Proyecto1.Controllers;
using Servicio.Interfaces;

namespace TestTablero.ControllerTest
{
    public class ControladorTableroTest
    {
        private readonly Mock<ITableroServicio> _servicioTablero;
        private readonly TableroController _controlador;

        public ControladorTableroTest()
        {
            _servicioTablero = new Mock<ITableroServicio>();
            _controlador = new TableroController(_servicioTablero.Object);
        }

        [Fact]
        public void QueSePuedaObtenerElIndexConTodosLosTablerosCreados()
        {
            Tablero tablero1 = new Tablero { Id = 1, Name = "Tablero 1", Descripcion = "Descripción del Tablero 1", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1 };
            Tablero tablero2 = new Tablero { Id = 2, Name = "Tablero 2", Descripcion = "Descripción del Tablero 2", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1 };
            Tablero tablero3 = new Tablero { Id = 3, Name = "Tablero 3", Descripcion = "Descripción del Tablero 3", FechaCreacion = DateTime.Now, IdUsuarioCreador = 1 };

            var listaTableros = new List<Tablero> { tablero1, tablero2, tablero3 };

            _servicioTablero.Setup(servicio => servicio.ObtenerTableros()).Returns(listaTableros.Select(t => new TableroViewModel
            {
                Id = t.Id,
                Titulo = t.Name,
                Descripcion = t.Descripcion,
            }).ToList());


            var resultado = _controlador.Index() as ViewResult;

            List<TableroViewModel> tablerosObtenidos = resultado.Model as List<TableroViewModel>;

            Assert.Equal("Index", resultado.ViewName);
            Assert.NotNull(tablerosObtenidos);
            Assert.Equal(3, tablerosObtenidos.Count);

        }

        [Fact]
        public void QueSePuedaIrALaVistaCrear()
        {
            var resultado = _controlador.Index() as ViewResult;
            Assert.Equal("Index", resultado.ViewName);
        }

        [Fact]
        public void QueSePuedaCrearUnTablero()
        {
            TableroViewModel nuevoTablero = new TableroViewModel { Titulo = "Nuevo Tablero", Descripcion = "Descripción del nuevo tablero" };
            var resultado = _controlador.Create(nuevoTablero) as RedirectToActionResult;
            Assert.Equal("Index", resultado.ActionName);
            _servicioTablero.Verify(servicio => servicio.CrearTablero(nuevoTablero), Times.Once);
        }

        [Fact]
        public void QueNoSePuedaCrearUnTableroConDatosInvalidos()
        {
            TableroViewModel nuevoTablero = new TableroViewModel { Titulo = "", Descripcion = "Descripción del nuevo tablero" };
            _controlador.ModelState.AddModelError("Titulo", "El título es obligatorio");
            var resultado = _controlador.Create(nuevoTablero) as RedirectToActionResult;
            Assert.Equal("Index", resultado.ActionName);
            Assert.False(_controlador.ModelState.IsValid);
            _servicioTablero.Verify(servicio => servicio.CrearTablero(nuevoTablero), Times.Never);
        }

        [Fact]
        public void Create_SiHayException_RedireccionaAIndex()
        {
            
            var nuevoTablero = new TableroViewModel
            {
                Titulo = "",
                Descripcion = "Descripción del nuevo tablero"
            };

            _servicioTablero
                .Setup(s => s.CrearTablero(It.IsAny<TableroViewModel>()))
                .Throws(new Exception("Error al crear el tablero"));
            
            var resultado = _controlador.Create(nuevoTablero);

            var redirect = Assert.IsType<RedirectToActionResult>(resultado);
            Assert.Equal("Index", redirect.ActionName);

            _servicioTablero.Verify(
                s => s.CrearTablero(It.IsAny<TableroViewModel>()),
                Times.Once
            );
        }


        [Fact]
        public void QueSePuedaEditarUnTablero()
        {
            TableroViewModel tableroExistente = new TableroViewModel { Id = 1, Titulo = "Tablero Existente", Descripcion = "Descripción del tablero existente" };
            var resultado = _controlador.Editar(tableroExistente) as RedirectToActionResult;
            Assert.Equal("Detalle", resultado.ActionName);
            _servicioTablero.Verify(servicio => servicio.EditarTablero(tableroExistente), Times.Once);
        }

        [Fact]
        public void QueNoSePuedaEditarUnTableroConDatosInvalidos()
        {
            TableroViewModel tableroExistente = new TableroViewModel { Id = 1, Titulo = "", Descripcion = "Descripcion" };
            _controlador.ModelState.AddModelError("Titulo", "El título es obligatorio");
            var resultado = _controlador.Editar(tableroExistente) as ViewResult;
            Assert.Equal("Editar", resultado.ViewName);
            Assert.False(_controlador.ModelState.IsValid);
            _servicioTablero.Verify(servicio => servicio.EditarTablero(It.IsAny<TableroViewModel>()), Times.Never);
        }

        [Fact]
        public void QueNoSePuedaEditarUnTableroPorUnaExcepcionDeberiaDevolverViewEditar()
        {
            TableroViewModel tableroExistente = new TableroViewModel { Id = 1, Titulo = "", Descripcion = "Descripcion" };
            _servicioTablero.Setup(servicio => servicio.EditarTablero(tableroExistente))
                            .Throws(new Exception("Error al editar el tablero"));

            var resultado = _controlador.Editar(tableroExistente) as ViewResult;
            Assert.NotNull(resultado);
            Assert.Equal("Editar", resultado.ViewName);
            _servicioTablero.Verify(servicio => servicio.EditarTablero(tableroExistente), Times.Once);
        }

        

        [Fact]
        public void QueSePuedaEliminarUnTablero()
        {
            var resultado = _controlador.Borrar(1) as RedirectToActionResult;
            Assert.Equal("Index", resultado.ActionName);
            _servicioTablero.Verify(servicio => servicio.EliminarTablero(1), Times.Once);
        }

        [Fact]
        public void QueNoSePuedaEliminarUnTableroPorUnaExcepcioDeberiaDevolverViewIndex()
        {
            _servicioTablero.Setup(servicio => servicio.EliminarTablero(1))
                            .Throws(new Exception("Error al eliminar el tablero"));

            var resultado = _controlador.Borrar(1) as ViewResult;
            Assert.NotNull(resultado);
            Assert.Equal("Index", resultado.ViewName);
            _servicioTablero.Verify(servicio => servicio.EliminarTablero(1), Times.Once);
        }

    }
}

