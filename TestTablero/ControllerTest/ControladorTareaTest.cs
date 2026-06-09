using DTOS.Columna;
using DTOS.Tarea;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Model;
using Moq;
using Proyecto1.Controllers;
using Servicio;
using Servicio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestTablero.ControllerTest
{
    public class ControladorTareaTest
    {
        private readonly Mock<ITareaServicio> _servicioTarea;
        private readonly TareaController _controlador;

        public ControladorTareaTest()
        {
            _servicioTarea = new Mock<ITareaServicio>();
            _controlador = new TareaController(_servicioTarea.Object);
            _controlador.TempData = new Mock<ITempDataDictionary>().Object;
        }


        [Fact]
        public void QueSePuedaAgregarUnaTarea()
        {
            _controlador.ModelState.Clear();

            var tarea = new CrearTareaViewModel
            {
                titulo = "Tarea 1",
                tableroId = 1,
                columnaId = 1
            };

            var resultado = _controlador.Agregar(tarea) as RedirectToActionResult;

            Assert.Equal("Detalle", resultado.ActionName);
            Assert.Equal("Tablero", resultado.ControllerName);

            _servicioTarea.Verify(s => s.AgregarTarea(It.Is<CrearTareaViewModel>(
                t => t.titulo == "Tarea 1" && t.tableroId == 1 && t.columnaId == 1
            )), Times.Once);
        }

        [Fact]
        public void QueSiModelStateEsInvalidoNoSeAgregueLaTarea()
        {
            _controlador.ModelState.Clear();
            _controlador.ModelState.AddModelError("titulo", "El título es obligatorio");
            var tarea = new CrearTareaViewModel
            {
                titulo = "",
                tableroId = 1,
                columnaId = 1
            };
            var resultado = _controlador.Agregar(tarea) as RedirectToActionResult;

            Assert.Equal("Detalle", resultado.ActionName);
            Assert.Equal("Tablero", resultado.ControllerName);
            _servicioTarea.Verify(s => s.AgregarTarea(It.IsAny<CrearTareaViewModel>()), Times.Never);
        }

        [Fact]
        public void CuandoFallaAgregarPorExceptionRedirigeAError()
        {
            _controlador.ModelState.Clear();

            var tarea = new CrearTareaViewModel
            {
                titulo = "Tarea 1",
                tableroId = 1,
                columnaId = 1
            };

            _servicioTarea
                .Setup(s => s.AgregarTarea(It.IsAny<CrearTareaViewModel>()))
                .Throws(new Exception("Error simulado"));

            var resultado = _controlador.Agregar(tarea) as RedirectToActionResult;

            Assert.Equal("Error", resultado.ActionName);
            Assert.Equal("Error", resultado.ControllerName);
        }

        [Fact]
        public void QueSePuedaCambiarElEstadoDeLaTarea()
        {
            _controlador.ModelState.Clear();

            var resultado = _controlador.CambiarEstadoTarea(1, 1) as RedirectToActionResult;

            Assert.Equal("Detalle", resultado.ActionName);
            Assert.Equal("Tablero", resultado.ControllerName);
        }

        [Fact]
        public void QueRedireccioneAErrorPorUnaExceptionAlCambiarElEstadoDeLaTarea()
        {
            _servicioTarea
                .Setup(s => s.CambiarEstadoTarea(1))
                .Throws(new Exception("Error simulado"));

            var resultado = _controlador.CambiarEstadoTarea(1, 1) as RedirectToActionResult;

            Assert.Equal("Error", resultado.ActionName);
            Assert.Equal("Error", resultado.ControllerName);
        }

        [Fact]
        public void QueSePuedaEditarUnaTarea()
        {
            var tareaViewModel = new TareaViewModel
            {
                id = 1,
                Nombre = "Tarea Editada",
                Descripcion = "Descripción editada",
                IdColumna = 1,
                IdTablero = 1
            };

            var resultado = _controlador.EditarTarea(tareaViewModel) as RedirectToActionResult;

            Assert.Equal("Detalle", resultado.ActionName);
            Assert.Equal("Tablero", resultado.ControllerName);
        }

        [Fact]
        public void QueRedireccioneAErrorPorUnaExceptionAlEditarTarea()
        {
            var tareaViewModel = new TareaViewModel
            {
                id = 1,
                Nombre = "Tarea Editada",
                Descripcion = "Descripción editada",
                IdColumna = 1,
                IdTablero = 1
            };
            _servicioTarea
                .Setup(s => s.EditarTarea(It.IsAny<TareaViewModel>()))
                .Throws(new Exception("Error simulado"));
            var resultado = _controlador.EditarTarea(tareaViewModel) as RedirectToActionResult;
            Assert.Equal("Error", resultado.ActionName);
            Assert.Equal("Error", resultado.ControllerName);
        }

        [Fact]
        public void QueSePuedaEliminarUnaTarea()
        {
            var resultado = _controlador.Eliminar(1, 1) as RedirectToActionResult;
            Assert.Equal("Detalle", resultado.ActionName);
            Assert.Equal("Tablero", resultado.ControllerName);
        }

        [Fact]
        public void QueRedireccioneAErrorPorUnaExceptionAlEliminarTarea()
        {
            _servicioTarea
                .Setup(s => s.EliminarTarea(1))
                .Throws(new Exception("Error simulado"));
            var resultado = _controlador.Eliminar(1, 1) as RedirectToActionResult;
            Assert.Equal("Error", resultado.ActionName);
            Assert.Equal("Error", resultado.ControllerName);
        }

        [Fact]
        public void ReordenarTarea_CuandoEsExitosoDevuelveOk()
        {
            
            var request = new ReordenarTarea
            {
                ColumnaOrigenId = 1,
                ColumnaDestinoId = 2,
                IdsOrigen = new[] { 1, 2 },
                IdsDestino = new[] { 3, 4 }
            };

            
            var resultado = _controlador.Reordenar(request);

            
            Assert.IsType<OkResult>(resultado);

            _servicioTarea.Verify(
                s => s.ReordenarTarea(request),
                Times.Once);

        }

        [Fact]
        public void ReordenarTarea_CuandoFallaDevuelveBadRequest()
        {
         var request = new ReordenarTarea
            {
                ColumnaOrigenId = 1,
                ColumnaDestinoId = 2,
                IdsOrigen = new[] { 1, 2 },
                IdsDestino = new[] { 3, 4 }
            };

            _servicioTarea
                .Setup(s => s.ReordenarTarea(It.IsAny<ReordenarTarea>()))
                .Throws(new Exception("Error simulado"));

            var resultado = _controlador.Reordenar(request);

            var badRequest = Assert.IsType<BadRequestObjectResult>(resultado);
            Assert.Equal("Error simulado", badRequest.Value);

            _servicioTarea.Verify(
                s => s.ReordenarTarea(It.IsAny<ReordenarTarea>()),
                Times.Once);
        }

    }
}
