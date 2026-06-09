using DTOS.Columna;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOS.Tablero
{
    public class DetalleTableroViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }

        public List<ColumnaViewModel> Columnas { get; set; }
    }
}
