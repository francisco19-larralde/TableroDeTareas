using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTOS.Tarea
{
    public class CrearTareaViewModel
    {
        public int columnaId { get; set; }

        public int tableroId { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres")]
        public string titulo { get; set; }
    }
}
