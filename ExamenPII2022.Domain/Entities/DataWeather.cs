using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenPII2022.Domain.Entities
{
    public class DataWeather
    {
        public double dt { get; set; } // 8
        public Main main { get; set; } // 50
        public List<Weather> weather { get; set; } //520
        public clouds clouds { get; set; } // 4
        public Wind wind { get; set; } //12
        public double visibility { get; set; } // 8
        public double pop { get; set; } // 8
        public Sys sys { get; set; } // 20
        public string dt_txt { get; set; } // 50

        //800
    }
}
