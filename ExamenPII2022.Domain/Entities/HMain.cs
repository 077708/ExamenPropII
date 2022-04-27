using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenPII2022.Domain.Entities
{
    public class HMain
    {
        public long dt { get; set; } // 8
        public Main Main { get; set; } // 50 
        public Wind Wind { get; set; } // 15 
        public clouds clouds { get; set; } // 5
        public List<Weather> weather { get; set; }//
    }
}
