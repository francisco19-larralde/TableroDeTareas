using DTOS.Columna;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Model;
using Moq;
using Proyecto1.Controllers;
using Servicio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestTablero.ControllerTest
{
    public class ControladorColumnaTest
    {
        private readonly Mock<IColumnaServicio> _servicioColumna;
        private readonly ColumnaController _controlador;

        public ControladorColumnaTest()
        {
            _servicioColumna = new Mock<IColumnaServicio>();
            _controlador = new ColumnaController(_servicioColumna.Object);
            _controlador.TempData = new Mock<ITempDataDictionary>().Object;
        }


        [Fact]
        public void QueSePuedaAgregarUnaColumna()
        {
            _controlador.ModelState.Clear();

            var columna1 = new CrearColumnaViewModel
            {
                nombre = "Columna 1",
                tableroId = 1
            };

            var resultado = _controlador.Agregar(columna1) as RedirectToActionResult;

            Assert.NotNull(resultado);
            Assert.Equal("Detalle", resultado.ActionName);
            Assert.Equal("Tablero", resultado.ControllerName);

            _servicioColumna.Verify(s => s.AgregarColumna(It.Is<CrearColumnaViewModel>(
                c => c.nombre == "Columna 1" && c.tableroId == 1
            )), Times.Once);
        }

        [Fact]
        public void QueSiModelStateEsInvalidoNoSeAgregueLaColumna()
        {
            _controlador.ModelState.Clear();
            _controlador.ModelState.AddModelError("nombre", "El nombre es obligatorio");

            var columna = new CrearColumnaViewModel
            {
                nombre = "",
                tableroId = 1
            };

            var resultado = _controlador.Agregar(columna) as RedirectToActionResult;

            Assert.NotNull(resultado);
            Assert.Equal("Detalle", resultado.ActionName);
            Assert.Equal("Tablero", resultado.ControllerName);

            _servicioColumna.Verify(
                s => s.AgregarColumna(It.IsAny<CrearColumnaViewModel>()),
                Times.Never
            );
        }

        [Fact]
        public void CuandoFallaServicioPorException_RedireccionaAVistaError()
        {

            var columna = new CrearColumnaViewModel
            {
                nombre = "Columna 1",
                tableroId = 1
            };

            _servicioColumna
                .Setup(s => s.AgregarColumna(It.IsAny<CrearColumnaViewModel>()))
                .Throws(new Exception("Error simulado"));

            var resultado = _controlador.Agregar(columna) as RedirectToActionResult;

            Assert.NotNull(resultado);
            Assert.Equal("Error", resultado.ActionName);
            Assert.Equal("Error", resultado.ControllerName);
        }

        [Fact]
        public void QueSePuedaEditarUnaColumna()
        {
            _controlador.ModelState.Clear();
            var columna = new ColumnaViewModel
            {
                Id = 1,
                Nombre = "Columna Editada",
                IdTablero = 1
            };
            var resultado = _controlador.Editar(columna) as RedirectToActionResult;
            Assert.NotNull(resultado);
            Assert.Equal("Detalle", resultado.ActionName);
            Assert.Equal("Tablero", resultado.ControllerName);
            _servicioColumna.Verify(s => s.EditarColumna(It.Is<ColumnaViewModel>(
                c => c.Id == 1 && c.Nombre == "Columna Editada" && c.IdTablero == 1
            )), Times.Once);

        }

        [Fact]
        public void QueSiModelStateEsInvalidoNoSeEditeLaColumna()
        {
            _controlador.ModelState.Clear();
            _controlador.ModelState.AddModelError("Nombre", "El nombre es obligatorio");
            var columna = new ColumnaViewModel
            {
                Id = 1,
                Nombre = "",
                IdTablero = 1
            };
            var resultado = _controlador.Editar(columna) as RedirectToActionResult;
            Assert.NotNull(resultado);
            Assert.Equal("Detalle", resultado.ActionName);
            Assert.Equal("Tablero", resultado.ControllerName);
            _servicioColumna.Verify(
                s => s.EditarColumna(It.IsAny<ColumnaViewModel>()),
                Times.Never
            );

        }

        [Fact]
        public void CuandoFallaServicioPorExceptionAlEditar_RedireccionaAVistaError()
        {
            var columna = new ColumnaViewModel
            {
                Id = 1,
                Nombre = "Columna Editada",
                IdTablero = 1
            };
            _servicioColumna
                .Setup(s => s.EditarColumna(It.IsAny<ColumnaViewModel>()))
                .Throws(new Exception("Error simulado"));
            var resultado = _controlador.Editar(columna) as RedirectToActionResult;
            Assert.NotNull(resultado);
            Assert.Equal("Error", resultado.ActionName);
            Assert.Equal("Error", resultado.ControllerName);
        }

        [Fact]
        public void QueSePuedaEliminarUnaColumna()
        {
            var columna = new ColumnaViewModel
            {
                Id = 1,
                Nombre = "Columna Editada",
                IdTablero = 1
            };
            var resultado = _controlador.Eliminar(columna) as RedirectToActionResult;
            Assert.NotNull(resultado);
            Assert.Equal("Detalle", resultado.ActionName);
            Assert.Equal("Tablero", resultado.ControllerName);
            _servicioColumna.Verify(s => s.EliminarColumna(1), Times.Once);
        }

        [Fact]
        public void CuandoFallaServicioPorExceptionAlEliminar_RedireccionaAVistaError()
        {
            var columna = new ColumnaViewModel
            {
                Id = 1,
                Nombre = "Columna Editada",
                IdTablero = 1
            };
            _servicioColumna
                .Setup(s => s.EliminarColumna(1))
                .Throws(new Exception("Error simulado"));
            var resultado = _controlador.Eliminar(columna) as RedirectToActionResult;
            Assert.NotNull(resultado);
            Assert.Equal("Error", resultado.ActionName);
            Assert.Equal("Error", resultado.ControllerName);
        }

        [Fact]
        public void QueSePuedaReordenarUnaColumna()
        {
            var columna = new ReordenarColumna
            {
                TableroId = 1,
                Ids = new List<int> { 3, 1, 2 }

            };
            var resultado = _controlador.Reordenar(columna);

            Assert.IsType<OkResult>(resultado);

            _servicioColumna.Verify(
                s => s.ReordenarColumna(columna),
                Times.Once);
        }

        [Fact]
        public void CuandoFallaServicioPorExceptionAlReordenar_DevuelveBadRequest()
        {
            var columna = new ReordenarColumna
            {
                TableroId = 1,
                Ids = new List<int> { 3, 1, 2 }
            };
            _servicioColumna
                .Setup(s => s.ReordenarColumna(columna))
                .Throws(new Exception("Error simulado"));
            var resultado = _controlador.Reordenar(columna);

            var badRequest = Assert.IsType<BadRequestObjectResult>(resultado);
            Assert.Equal("Error simulado", badRequest.Value);

            _servicioColumna.Verify(
                s => s.ReordenarColumna(columna),
                Times.Once);

        }
    }
}
