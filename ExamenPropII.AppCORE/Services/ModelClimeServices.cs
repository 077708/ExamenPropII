using ExamenPII2022.Domain.Entities;
using ExamenPII2022.Domain.Interfaces;
using ExamenPropII.AppCORE.IContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenPropII.AppCORE.Services
{
    public class ModelClimeServices : Base<ClimeWeather.Root>, IClimeServices
    {
        private IClimeModel Coleccion;
        public ModelClimeServices(IClimeModel coleccion) : base(coleccion)
        {
            this.Coleccion = coleccion;
        }
    }
}
