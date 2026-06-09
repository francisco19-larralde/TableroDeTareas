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



    }
}
