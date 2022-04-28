using ExamenPII2022.Domain.Interfaces;
using ExamenPropII.AppCORE.IContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenPropII.AppCORE.Services
{
    public abstract class Base <T> : IServices<T>
    {
        private IColeccion<T> coleccion;
        public Base(IColeccion<T> coleccion)
        {
            this.coleccion = coleccion;
        }

        public void Add(string item, double dt)
        {
            coleccion.Add(item, dt);
        }

        public bool Delete(int item)
        {
            return coleccion.Delete(item);  
        }

        public T Get(int t)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            return coleccion.GetAll();
        }

        public bool Update(T item, int i)
        {
            throw new NotImplementedException();
        }
    }
}
