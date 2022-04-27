using ExamenPII2022.Domain.Entities;
using ExamenPII2022.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExamenPII2022.Infraestructure.Repository
{
    public class ModelClimeWeather : IColeccion<ClimeWeather>
    { 
        private RAFContext RAFContext;

        public ModelClimeWeather()
        {
            RAFContext = new RAFContext();
        }

        public void Add(ClimeWeather item)
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}", txt, key);
                    var json = web.DownloadString(url);
                    RAFContext.Add<ClimeWeather>();
                    ClimeWeather data = JsonConvert.DeserializeObject<ClimeWeather>(json);

                }
            }
            catch (Exception)
            {
                throw;
            }
            throw new NotImplementedException();
        }

        public bool Delete(int item)
        {
            throw new NotImplementedException();
        }

        public ClimeWeather Get(int t)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ClimeWeather> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(ClimeWeather item, int i)
        {
            throw new NotImplementedException();
        }
    }
}
