using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    public class Tarea
    {
        [Key]
        public int id {  get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public bool completada { get; set; }
        public int orden { get; set; }
        public int idColumna { get; set; }
        [ForeignKey("idColumna")]
        public Columna Columna { get; set; }
    }
}
