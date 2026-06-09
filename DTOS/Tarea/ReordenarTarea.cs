using System;
using System.Collections.Generic;
using System.Text;

namespace DTOS.Tarea
{
    public class ReordenarTarea
    {
        public int? ColumnaOrigenId { get; set; }
        public int? ColumnaDestinoId { get; set; }

        public int[] IdsOrigen { get; set; }
        public int[] IdsDestino { get; set; }
    }
}
