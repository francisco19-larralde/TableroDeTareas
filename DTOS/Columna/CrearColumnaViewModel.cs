using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DTOS.Columna
{
    public class CrearColumnaViewModel
    {
        [Required]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string nombre { get; set; }
        public int tableroId { get; set; }
    }
}
