using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenPII2022.Domain.Interfaces
{
    public interface IColeccion <T>
    {
        void Add(T item);
        bool Update(T item, int i);
        bool Delete(int item);
        IReadOnlyList<T> GetAll();
        T Get(int t);
    }
}
