using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Application.Test
{
    public class CreditTest
    {
        BancoContext _context;

        [SetUp]
        public void Setup()
        {
            var optionsInMemory = new DbContextOptionsBuilder<BancoContext>().UseInMemoryDatabase("Creditos").Options;
            _context = new BancoContext(optionsInMemory);
        }

        [Test]
        public void CrearCreditoTest()
        {
            var request = new CrearCreditoRequest { Cedula = "1111", ValorPrestamo = 5200000, Nombre = "fabian", Salario=1200000 ,PlazoPago = 4, TipoCredito = 0, Fecha=DateTime.Now };
            CrearPrestamoService _service = new CrearPrestamoService(new UnitOfWork(_context));
            var response = _service.Ejecutar(request);
            Assert.AreEqual("Se creó con exito el credito 1111.", response.Mensaje);
        }

        [Test]
        public void DuplicreditTest()
        {
            var request = new CrearCreditoRequest { Cedula = "1112", ValorPrestamo = 5200000, Nombre = "fabian", Salario = 1200000, PlazoPago = 4, TipoCredito = 0, Fecha = DateTime.Now };
            var request2 = new CrearCreditoRequest { Cedula = "1112", ValorPrestamo = 5200000, Nombre = "fabian", Salario = 1200000, PlazoPago = 4, TipoCredito = 0, Fecha = DateTime.Now };
            CrearPrestamoService _service = new CrearPrestamoService(new UnitOfWork(_context));
            var response = _service.Ejecutar(request);
            var response2 = _service.Ejecutar(request2);
            Assert.AreEqual("El numero de credito 1112 ya existe.", response2.Mensaje);
        }

        [Test]
        public void ZAbonnarCredito()
        {
            var request = new CrearCreditoRequest { Cedula = "1111", ValorPrestamo = 5000000, Nombre = "fabian", Salario = 1200000, PlazoPago = 4, TipoCredito = 0, Fecha = DateTime.Now };
            CrearPrestamoService _service2 = new CrearPrestamoService(new UnitOfWork(_context));
            var response = _service2.Ejecutar(request);
            ConsignarService _service = new ConsignarService(new UnitOfWork(_context));
            var request3 = new ConsignarRequest { Cedula = "1111", Fecha = DateTime.Now, Valor = 3850000 };
            //se realizan 2 avonos por un valor de 7700000
            InvalidOperationException ex2 = Assert.Throws<InvalidOperationException>(() => _service.Ejecutar(request3));
            Assert.AreEqual(ex2.Message, "abonado con exito en las cuotas con indicador 1111 usted debe 11150000");

        }

    }
}
