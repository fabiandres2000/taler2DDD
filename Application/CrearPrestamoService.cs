using Domain.Contracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Factory;
using Domain.Interfaces;
using Domain.Base;
using System.Linq;


namespace Application
{
    public class CrearPrestamoService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IGenericFactory<Credito> _factory;

        public CrearPrestamoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _factory = new CreditServiceFactory();
        }

        public CrearCreditoResponse Ejecutar(CrearCreditoRequest request)
        {
            Credito credito = _unitOfWork.CreditoRepository.FindFirstOrDefault(t => t.Cedula == request.Cedula);
            if (credito != null) return new CrearCreditoResponse() { Mensaje = $"El numero de credito {request.Cedula} ya existe." };

            try
            {
                Credito newCredit = _factory.CreateEntity(request.TipoCredito);
                newCredit.Cedula = request.Cedula;
                newCredit.Fecha = request.Fecha;
                newCredit.ValorPrestamo = request.ValorPrestamo;
                newCredit.PlazoPago = request.PlazoPago;
                newCredit.Salario = request.Salario;
                newCredit.Nombre = request.Nombre;
                _unitOfWork.CreditoRepository.Add(newCredit);
                newCredit.Validar(request.ValorPrestamo,request.PlazoPago);
                _unitOfWork.Commit();
                return new CrearCreditoResponse() { Mensaje = $"Se creó con exito el credito {newCredit.Cedula}." };
            }
            catch (System.Exception ex)
            {
                return new CrearCreditoResponse() { Mensaje = ex.Message };
            }
        }



    }
    public class CrearCreditoRequest
    {
        public int TipoCredito { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public double ValorPrestamo { get; set; }
        public double Salario { get; set; }
        public DateTime Fecha { get; set; }
        public int PlazoPago { get; set; }
    }
    public class CrearCreditoResponse
    {
        public string Mensaje { get; set; }
    }
}

