using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Domain.Base;

namespace Domain.Entities
{
    public class Credito : Entity<int>, IServivioCredito
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public double ValorPrestamo { get; set; }
        public double Salario { get; set; }
        public double ValoprPagar { get; set; }
        public double ValorCuota { get; set; }
        public double SaldoCredito { get; set; }
        public DateTime Fecha { get; set; }
        public int PlazoPago { get; set; }
        public List<Cuota> Cuotas { get; set; }
        public List<Abono> Abonos { get; set; }
        public Credito()
        {
            Cuotas = new List<Cuota>();
            Abonos = new List<Abono>();

        }

        public virtual void Validar(double valor, int plazo)
        {
            if (valor < 0)
            {
                throw new InvalidOperationException("El valor que desea pedir como prestamo es incorrecto");
            }
            if (ValorPrestamo >= 5000000 && ValorPrestamo <= 10000000)
            {
                if (plazo <= 0 || plazo > 10)
                {
                    throw new InvalidOperationException("El plazo en meses es incorrecto");
                }
                else
                {
                    ValoprPagar = ValorPrestamo * (1 + 0.5 * plazo);
                    ValorCuota = ValoprPagar / PlazoPago;
                    SaldoCredito = ValoprPagar;
                    GenerarCuotas(Cedula, ValoprPagar, ValorCuota, PlazoPago);
                }
            }
            else
            {
                throw new InvalidOperationException("El valor del prestamo debe de estar entre 5000000 y 10000000");
            }
        }

        public virtual void GenerarCuotas(string Cedula, double ValoprPagar, double ValorCuota, int PlazoPago)
        {
            for (int i = 0; i < PlazoPago; i++)
            {
                Cuota cuota = new Cuota();
                cuota.IdCuota = Cedula;
                cuota.NumeroCuota = i;
                cuota.ValorCuota = ValorCuota;
                cuota.ValorAbonado = 0;
                cuota.ValorPendiente = 0;
                cuota.FechaAbono = DateTime.Now;
                Cuotas.Add(cuota);
            }
            // throw new InvalidOperationException($"Se ha generado 4 cuotas de pago con valor cada una de {ValorCuota}");
        }

       public List<Cuota> ConsultarPorCedula(string cedula){
            List<Cuota> Cuotas2 = new List<Cuota>();
            foreach (var item in Cuotas)
            {
                if (item.IdCuota.Equals(cedula))
                {
                    Cuotas2.Add(item);
                }
            }
            return Cuotas2;
        }

        public List<Abono> ConsultarAbono(string cedula)
        {
            List<Abono> Abonos2 = new List<Abono>();
            foreach (var item in Abonos)
            {
                if (item.Cedula.Equals(cedula))
                {
                    Abonos2.Add(item);
                }
            }
            return Abonos2;
        }



        public virtual void Abonar(string cedula, double valor)
        {
            if (valor<0)
            {
                throw new InvalidOperationException($"EL valor a abonar no puede ser menor a 0");
            }
            if (valor>SaldoCredito)
            {
                throw new InvalidOperationException($"EL valor a abonar no puede ser mayor a el saldo del credito");
            }
            else
            {
                if (valor>=ValorCuota)
                {
                    double valorabono = valor;
                    foreach (var dto in Cuotas)
                    {
                        if (dto.IdCuota.Equals(cedula))
                        {
                            if (dto.ValorAbonado==0 && dto.ValorPendiente==0)
                            {
                                if (valor>=ValorCuota){
                                    dto.ValorAbonado = ValorCuota;
                                    dto.ValorPendiente = 0;
                                    valor = valor - ValorCuota;
                                }
                                else
                                {
                                    if (valor<ValorCuota)
                                    {
                                        dto.ValorAbonado = valor;
                                        dto.ValorPendiente = ValorCuota - valor;
                                        valor = 0;
                                        break;
                                    }
                                }
                               
                            }
                            if(dto.ValorAbonado != 0 && dto.ValorPendiente != 0)
                            {
                                if (valor >= ValorCuota)
                                {
                                    valor -= (ValorCuota-dto.ValorAbonado);
                                    dto.ValorAbonado = ValorCuota;
                                    dto.ValorPendiente = 0;                             
                                }
                            }
                            if (dto.ValorAbonado != 0 && dto.ValorPendiente == 0)
                            {
                               
                            }
                        }

                    }
                    
                    this.SaldoCredito -= valorabono;
                    Abono abono = new Abono();
                    abono.Cedula = cedula;
                    abono.ValorAbonado = valorabono;
                    abono.FechaAbono = DateTime.Now;
                    Abonos.Add(abono);
                    throw new InvalidOperationException($"abonado con exito en las cuotas con indicador {cedula} usted debe {SaldoCredito}");
                }
                else
                {
                    throw new InvalidOperationException($"EL valor a abonar no puede ser menor al valor de la cuota");
                }
            }
        }
    }
}
