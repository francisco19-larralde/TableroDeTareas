using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    public class Columna
    {
        [Key]
        public int Id { get; set; }

        public string nombre { get; set; }

        public string descripcion { get; set; }
        public int orden { get; set; }

        public int idTablero { get; set; }
        [ForeignKey("idTablero")]
        public Tablero Tablero { get; set; }

        public ICollection<Tarea> Tareas { get; set; }
        
    }
}
