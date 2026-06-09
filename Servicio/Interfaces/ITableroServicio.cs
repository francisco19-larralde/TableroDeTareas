using DTOS.Tablero;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Servicio.Interfaces
{
    public interface ITableroServicio
    {
        public Boolean CrearTablero(TableroViewModel tableroViewModel);
        public Boolean EditarTablero(TableroViewModel tableroViewModel);
        public Boolean EliminarTablero(int id);
        public DetalleTableroViewModel ObtenerTablero(int id);
        public List<TableroViewModel> ObtenerTableros();

    }
}
