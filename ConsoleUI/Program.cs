using Application;
using Infrastructure;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var optionsSqlServer = new DbContextOptionsBuilder<BancoContext>()
             .UseSqlServer("Server=.\\;Database=Banco;Trusted_Connection=True;MultipleActiveResultSets=true")
             .Options;*/

            var optionsInMemory = new DbContextOptionsBuilder<BancoContext>()
             .UseInMemoryDatabase("Creditos")
             .Options;

            BancoContext context = new BancoContext(optionsInMemory);

            CrearCuentaPrestamo(context);
            Abonar(context);
        }

        private static void Abonar(BancoContext context)
        {
            #region  Consignar

            ConsignarService _service = new ConsignarService(new UnitOfWork(context));
            var request = new ConsignarRequest() { Cedula = "524255", Valor = 1000000,Fecha =DateTime.Now };

            ConsignarResponse response = _service.Ejecutar(request);

            System.Console.WriteLine(response.Mensaje);
            #endregion
            System.Console.ReadKey();
        }

        private static void CrearCuentaPrestamo(BancoContext context)
        {
            #region  Crear

            CrearPrestamoService _service = new CrearPrestamoService(new UnitOfWork(context));
            var requestCrer = new CrearCreditoRequest() { Cedula = "524255", Nombre = "fabian quintero" , Fecha=DateTime.Now, PlazoPago=4, Salario=1200000, TipoCredito=0, ValorPrestamo=500000};

            CrearCreditoResponse responseCrear = _service.Ejecutar(requestCrer);

            System.Console.WriteLine(responseCrear.Mensaje);
            #endregion
        }
    }
}
