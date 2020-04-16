using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Cuota
    {
        public int Id { get; set; }
        public string IdCuota { get; set; }
        public int NumeroCuota { get; set; }
        public double ValorCuota { get; set; }
        public double ValorAbonado { get; set; }
        public double ValorPendiente { get; set; }
        public DateTime FechaAbono { get; set; }

        public Cuota()
        {
            ValorAbonado = 0;
            ValorPendiente = 0;
        }
    }
}
