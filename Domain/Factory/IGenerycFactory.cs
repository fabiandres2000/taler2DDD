using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;

namespace Domain.Interfaces
{
    public interface IGenericFactory<T> where T : BaseEntity
    {
        T CreateEntity(int type);
    }
}
