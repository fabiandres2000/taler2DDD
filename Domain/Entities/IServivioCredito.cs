using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    interface IServivioCredito
    {
        string Cedula { get; set; }
        string Nombre { get; set; }
        double ValorPrestamo { get; set; }
        double Salario { get; set; }
        double ValoprPagar { get; set; }
        double ValorCuota { get; set; }
        double SaldoCredito { get; set; }
        DateTime Fecha { get; set; }
        int PlazoPago { get; set; }

        void GenerarCuotas(string Cedula,double ValoprPagar,double ValorCuota,int PlazoPago );
        void Validar(double valor,int plazo);
        void Abonar(string idcredito, double valor);
        List<Cuota> ConsultarPorCedula(string cedula);
        List<Abono> ConsultarAbono(string cedula);

    }
}
