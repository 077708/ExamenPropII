using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenPII2022.Domain.Entities
{
    public class ClimeWeather
    {/*
        public string Cod { get; set; } // 50
        public string Message { get; set; } // 50 
        public int cnt { get; set; } // 4
        public List<DataWeather> dataWeathers { get; set; } 
        public City city { get; set; }
        public string country { get; set; }
        public int timezone { get; set; }
        public long sunrise { get; set; }
        public long sunset { get; set; }*/

        public string Message { get; set; } // 50 
        public string Cod { get; set; } // 50
        public long city_id { get; set; } //8
        public double calctime { get; set; } //8
        public int cnt { get; set; } // 4
        public List<HMain> hMains { get; set; } // data
    }
}
