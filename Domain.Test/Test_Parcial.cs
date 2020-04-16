using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using NUnit.Framework;

namespace Domain.Test
{
    class Test_Parcial
    {
        Credito NuevoCredito;
        [SetUp]
        public void Setup()
        {
            NuevoCredito = new Credito
            {
                Cedula = "1234",
                Nombre = "fabian",
                ValorPrestamo = 5000000,
                Salario = 1200000,
                Fecha = DateTime.Now,
                PlazoPago = 4,

            };
        }
        [Test]
        public void PrestamoCorrecto()
        {
            NuevoCredito.Validar(NuevoCredito.ValorPrestamo, NuevoCredito.PlazoPago);
            Assert.AreEqual(NuevoCredito.ValorCuota, 3750000);
        }

        [Test]
        public void PrestamoIncorrectoValorNegativo()
        {
            NuevoCredito.ValorPrestamo = -2000000;
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Validar(NuevoCredito.ValorPrestamo, NuevoCredito.PlazoPago));
            Assert.AreEqual(ex.Message, "El valor que desea pedir como prestamo es incorrecto");
        }

        [Test]
        public void PrestamoIncorrectoPlazo()
        {
            NuevoCredito.PlazoPago = 13;
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Validar(NuevoCredito.ValorPrestamo, NuevoCredito.PlazoPago));
            Assert.AreEqual(ex.Message, "El plazo en meses es incorrecto");
        }

        [Test]
        public void PrestamoIncorrectoPlazoNegativo()
        {
            NuevoCredito.PlazoPago = -13;
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Validar(NuevoCredito.ValorPrestamo, NuevoCredito.PlazoPago));
            Assert.AreEqual(ex.Message, "El plazo en meses es incorrecto");
        }

        [Test]
        public void PrestamoIcorrectoValorMenor()
        {
            NuevoCredito.ValorPrestamo = 4200000;
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Validar(NuevoCredito.ValorPrestamo, NuevoCredito.PlazoPago));
            Assert.AreEqual(ex.Message, "El valor del prestamo debe de estar entre 5000000 y 10000000");
        }

        [Test]
        public void PrestamoIcorrectoValorMayor()
        {
            NuevoCredito.ValorPrestamo = 12000000;
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Validar(NuevoCredito.ValorPrestamo, NuevoCredito.PlazoPago));
            Assert.AreEqual(ex.Message, "El valor del prestamo debe de estar entre 5000000 y 10000000");
        }

        [Test]
        public void Abonnar()
        {
            //el valor total a pagar son 15 millones de acuerdo a la ecuacion SaldoInicialCredito = ValorCredito*(1 + TasaInteres   PlazoCredito)  y el valor de la cuota es 37500000
            NuevoCredito.ValorPrestamo = 5000000;
            double valorAbonar = 3850000;
            double valorAbonar2 = 3850000;
            NuevoCredito.Validar(NuevoCredito.ValorPrestamo, NuevoCredito.PlazoPago);
            //se realizan 2 avonos por un valor de 7700000
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Abonar(NuevoCredito.Cedula, valorAbonar));
            InvalidOperationException ex2 = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Abonar(NuevoCredito.Cedula, valorAbonar2));
            Assert.AreEqual(ex2.Message, "abonado con exito en las cuotas con indicador 1234 usted debe 7300000");
           
        }

        [Test]
        public void AbonnarIncorrecto()
        {
            //el valor total a pagar son 15 millones de acuerdo a la ecuacion SaldoInicialCredito = ValorCredito*(1 + TasaInteres   PlazoCredito)  y el valor de la cuota es 3750000
            NuevoCredito.ValorPrestamo = 5000000;
            double valorAbonar = 3500000;
            NuevoCredito.Validar(NuevoCredito.ValorPrestamo, NuevoCredito.PlazoPago);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Abonar(NuevoCredito.Cedula, valorAbonar));
            Assert.AreEqual(ex.Message, "EL valor a abonar no puede ser menor al valor de la cuota");
        }

        [Test]
        public void AbonnarIncorrectoMayorSaldoCredito()
        {
            NuevoCredito.ValorPrestamo = 5000000;
            double valorAbonar = 3750000;
            double valorAbonar2 = 12000000;
            //el valor total a pagar son 15 millones de acuerdo a la ecuacion SaldoInicialCredito = ValorCredito*(1 + TasaInteres   PlazoCredito)  y el valor de la cuota es 3750000
            NuevoCredito.Validar(NuevoCredito.ValorPrestamo, NuevoCredito.PlazoPago);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Abonar(NuevoCredito.Cedula, valorAbonar));
            InvalidOperationException ex2 = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Abonar(NuevoCredito.Cedula, valorAbonar2));
            Assert.AreEqual(ex2.Message, "EL valor a abonar no puede ser mayor a el saldo del credito");
        }

        [Test]
        public void AbonnarIncorrectoValorNegativo()
        {
            NuevoCredito.ValorPrestamo = 5000000;
            double valorAbonar = -3750000;
            //el valor total a pagar son 15 millones de acuerdo a la ecuacion SaldoInicialCredito = ValorCredito*(1 + TasaInteres   PlazoCredito)  y el valor de la cuota es 3750000
            NuevoCredito.Validar(NuevoCredito.ValorPrestamo, NuevoCredito.PlazoPago);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Abonar(NuevoCredito.Cedula, valorAbonar));
            Assert.AreEqual(ex.Message, "EL valor a abonar no puede ser menor a 0");
        }


        [Test]
        public void MostrarCuotasAntesDeAbonar()
        {
            NuevoCredito.Validar(NuevoCredito.ValorPrestamo, NuevoCredito.PlazoPago);
            List<Cuota> Cuotas = new List<Cuota>();
            Cuotas = NuevoCredito.ConsultarPorCedula(NuevoCredito.Cedula);

            foreach (var dto in Cuotas)
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("ID :" + dto.IdCuota);
                Console.WriteLine("# Cuota : " + dto.NumeroCuota);
                Console.WriteLine("V. Abonado : " + dto.ValorAbonado);
                Console.WriteLine("V. Cuota : " + dto.ValorCuota);
                Console.WriteLine("V. Pendiente : " + dto.ValorPendiente);
                Console.WriteLine("Fecha : " + dto.FechaAbono);
            }
            Assert.AreEqual(NuevoCredito.ValorCuota, 3750000);
        }

        [Test]
        public void MostrarCuotasDespuesDeAbonar()
        {
            //el valor total a pagar son 15 millones de acuerdo a la ecuacion SaldoInicialCredito = ValorCredito*(1 + TasaInteres   PlazoCredito)  y el valor de la cuota es 37500000
            NuevoCredito.ValorPrestamo = 5000000;
            double valorAbonar = 3850000;
            double valorAbonar2 = 3850000;
            NuevoCredito.Validar(NuevoCredito.ValorPrestamo, NuevoCredito.PlazoPago);
            //se realizan 2 avonos por un valor de 7700000
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Abonar(NuevoCredito.Cedula, valorAbonar));
            InvalidOperationException ex2 = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Abonar(NuevoCredito.Cedula, valorAbonar2));
            Assert.AreEqual(ex2.Message, "abonado con exito en las cuotas con indicador 1234 usted debe 7300000");

            List<Cuota> Cuotas = new List<Cuota>();
            Cuotas = NuevoCredito.ConsultarPorCedula(NuevoCredito.Cedula);
            
            foreach (var dto in Cuotas)
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("ID :" + dto.IdCuota);
                Console.WriteLine("# Cuota : " + dto.NumeroCuota);
                Console.WriteLine("V. Abonado : " + dto.ValorAbonado);
                Console.WriteLine("V. Cuota : " + dto.ValorCuota);
                Console.WriteLine("V. Pendiente : " + dto.ValorPendiente);
                Console.WriteLine("Fecha : " + dto.FechaAbono);
            }
        }

        [Test]
        public void MostrarAbonos()
        {
            //el valor total a pagar son 15 millones de acuerdo a la ecuacion SaldoInicialCredito = ValorCredito*(1 + TasaInteres   PlazoCredito)  y el valor de la cuota es 37500000
            NuevoCredito.ValorPrestamo = 5000000;
            double valorAbonar = 3850000;
            double valorAbonar2 = 3850000;
            NuevoCredito.Validar(NuevoCredito.ValorPrestamo, NuevoCredito.PlazoPago);
            //se realizan 2 avonos por un valor de 7700000
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Abonar(NuevoCredito.Cedula, valorAbonar));
            InvalidOperationException ex2 = Assert.Throws<InvalidOperationException>(() => NuevoCredito.Abonar(NuevoCredito.Cedula, valorAbonar2));
            Assert.AreEqual(ex2.Message, "abonado con exito en las cuotas con indicador 1234 usted debe 7300000");


            List<Abono> Abonos = new List<Abono>();
            Abonos = NuevoCredito.ConsultarAbono(NuevoCredito.Cedula);
            foreach (var dto in NuevoCredito.Abonos)
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("ID :" + dto.Cedula);
                Console.WriteLine("V. Abonado : " + dto.ValorAbonado);
                Console.WriteLine("Fecha : " + dto.FechaAbono);
            }
        }
    }
}
