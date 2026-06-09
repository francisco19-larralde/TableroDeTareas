using System;
using System.Collections.Generic;
using System.Text;

namespace DTOS.Tarea
{
    public class TareaViewModel
    {
        public int id { get; set; }
        public string Nombre { get; set; }

        public string Descripcion { get; set; }
        public int orden { get; set; }

        public bool Completada { get; set; }

        public int IdColumna { get; set; }
        public int IdTablero { get; set; }
    }
}
