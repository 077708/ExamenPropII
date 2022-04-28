using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenPropII.AppCORE.IContracts
{
    public interface IServices <T>
    {
        void Add(string item, double dt);
        bool Update(T item, int i);
        bool Delete(int item);
        List<T> GetAll();
        T Get(int t);
    }
}
