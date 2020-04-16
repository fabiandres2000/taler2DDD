using Domain.Contracts;
using Domain.Entities;
using Domain.Repositories;
using System;

namespace Application
{
    public class ConsignarService 
    {
        readonly IUnitOfWork _unitOfWork;
        
        public ConsignarService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ConsignarResponse Ejecutar(ConsignarRequest request)
        {
            var credito = _unitOfWork.CreditoRepository.FindFirstOrDefault(t => t.Cedula==request.Cedula);
            if (credito != null)
            {
                credito.Abonar(request.Cedula,request.Valor);
                _unitOfWork.Commit();
                return new ConsignarResponse() { Mensaje = $"Su Nuevo saldo es {credito.SaldoCredito}." };
            }
            else
            {
                return new ConsignarResponse() { Mensaje = $"Número de Cuenta No Válido." };
            }
        }
    }
    public class ConsignarRequest
    {
        public string Cedula { get; set; }   
        public double Valor { get; set; }
        public DateTime Fecha { get; set; }
    }
    public class ConsignarResponse
    {
        public string Mensaje { get; set; }
    }
}
