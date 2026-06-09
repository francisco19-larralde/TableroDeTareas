using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    public class Tablero
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioCreador { get; set; }
        [ForeignKey("IdUsuarioCreador")]
        public Usuario UsuarioCreador { get; set; }
        public ICollection<Columna> Columnas { get; set; } = new List<Columna>();

    }
}
