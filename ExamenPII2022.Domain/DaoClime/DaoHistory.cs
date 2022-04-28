using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenPII2022.Domain.DaoClime
{
    public class DaoHistory
    {
        public int Id { get; set; } 
        public double Lat { get; set; }   
        public double Long { get; set; }
        public string Time_zone { get; set; }   
        public int Timezoneoff { get; set; }

    }
}
