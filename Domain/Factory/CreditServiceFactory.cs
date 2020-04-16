using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Factory
{
    public class CreditServiceFactory : IGenericFactory<Credito>
    {
        public Credito CreateEntity(int type)
        {
            CreditlServiceType accountType = (CreditlServiceType)type;
            switch (accountType)
            {
                case  CreditlServiceType.Credito:
                    return new Credito();
                default:
                    throw new ArgumentOutOfRangeException(message: "Tipo de Credito No Válido.", innerException: null);
            }
        }
    }

    public enum CreditlServiceType
    {
        Credito = 0,
       
    }
}

