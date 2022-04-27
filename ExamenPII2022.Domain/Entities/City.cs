using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenPII2022.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string name { get; set; }
        public Coord coord { get; set; }
    }
}
