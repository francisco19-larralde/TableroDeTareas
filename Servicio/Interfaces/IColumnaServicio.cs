using DTOS.Columna;
using System;
using System.Collections.Generic;
using System.Text;

namespace Servicio.Interfaces
{
    public interface IColumnaServicio
    {
        public bool AgregarColumna(CrearColumnaViewModel columnaViewModel);
        public bool EliminarColumna(int id);
        public bool EditarColumna(ColumnaViewModel columnaViewModel);
        public void ReordenarColumna(ReordenarColumna reordenarColumna);

    }
}
