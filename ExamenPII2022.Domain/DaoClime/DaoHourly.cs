using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenPII2022.Domain.DaoClime
{
    public class DaoHourly
    {
        public string Hora { get; set; }
        public double Temperatura { get; set; }
        public int Presión { get; set; }
        public int Nubes { get; set; }
        public int Visibilidad { get; set; }
        public double Velocidad_viento { get; set; }
    }
}
