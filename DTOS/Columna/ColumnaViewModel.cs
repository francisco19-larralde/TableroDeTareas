using DTOS.Tarea;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOS.Columna
{
    public class ColumnaViewModel
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int IdTablero { get; set; }
        public int? Orden { get; set; }

        public List<TareaViewModel>? Tareas { get; set; }
    }
}
