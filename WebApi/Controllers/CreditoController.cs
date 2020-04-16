using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Domain.Contracts;
using Infrastructure;
using Infrastructure.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditoController : ControllerBase
    {
        readonly BancoContext _context;
        readonly IUnitOfWork _unitOfWork;
        
        //Se Recomienda solo dejar la Unidad de Trabajo
        public CreditoController(BancoContext context,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        [HttpPost]
        public ActionResult<CrearCreditoResponse> Post(CrearCreditoRequest request)
        {
            CrearPrestamoService _service = new CrearPrestamoService(_unitOfWork);
            CrearCreditoResponse response = _service.Ejecutar(request);
            return Ok(response);
        }

        [HttpPost("consignacion")]
        public ActionResult<ConsignarResponse> Post(ConsignarRequest request)
        {
            var _service = new ConsignarService(new UnitOfWork(_context));
            var response = _service.Ejecutar(request);
            return Ok(response);
        }
    }
}