using DTOS.Tarea;
using System;
using System.Collections.Generic;
using System.Text;

namespace Servicio.Interfaces
{
    public interface ITareaServicio
    {
        public bool AgregarTarea(CrearTareaViewModel tareaViewModel);
        public bool CambiarEstadoTarea(int tareaId);
        public bool EliminarTarea(int tareaId);
        public bool EditarTarea(TareaViewModel tareaViewModel);
        public void ReordenarTarea(ReordenarTarea request);
    }
}
