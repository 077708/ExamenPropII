using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenPII2022.Domain.Interfaces
{
    public interface IColeccion <T>
    {
        void Add(string item);
        bool Update(T item, int i);
        bool Delete(int item);
        List<T> GetAll();
        T Get(int t);
    }
}