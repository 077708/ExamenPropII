using ExamenPII2022.Domain.Entities;
using ExamenPII2022.Domain.Interfaces;
using ExamenPropII.Common.CommonData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static ExamenPII2022.Domain.Entities.ClimeCity;

namespace ExamenPII2022.Infraestructure.Repository
{
    public class ModelClimeWeather : IColeccion<ClimeWeather.Root>, IClimeModel
    { 
        private RAFContext RAFContext;

        public ModelClimeWeather()
        {
            RAFContext = new RAFContext(30000, "ClimeAPI");
        }

        public void Add(string txt, double dt)
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    string cityUrl = $"{AppSettings.ApiUrlCity}q={txt}&appid={AppSettings.Token}";
                    var json = web.DownloadString(cityUrl);

                    Root data = JsonConvert.DeserializeObject<Root>(json);

                    string url = $"{AppSettings.ApiUrlHistory}lat={data.coord.lat}&lon={data.coord.lon}&dt={dt}&appid={AppSettings.Token}";

                    var jsonHistory = web.DownloadString(url);

                    ClimeJson climeJson = new ClimeJson()
                    {
                        JsonAPI = jsonHistory,
                    };


                    int count = climeJson.JsonAPI.Length;
                    RAFContext.Add<ClimeJson>(climeJson);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Delete(int item)
        {
            throw new NotImplementedException();
        }

        public ClimeWeather.Root Get(int t)
        {
            throw new NotImplementedException();
        }

        public List<ClimeWeather.Root> GetAll()
        {
            try
            {
                var data = RAFContext.GetAll<ClimeJson>();
                List<ClimeWeather.Root> weatherList = new List<ClimeWeather.Root>();

                foreach (var item in data)
                {
                    var temp = item.JsonAPI.Length;
                    ClimeWeather.Root jsonObject = JsonConvert.DeserializeObject<ClimeWeather.Root>(item.JsonAPI);
                    jsonObject.Id = item.Id;
                    weatherList.Add(jsonObject);
                }

                return weatherList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Update(ClimeWeather.Root item, int i)
        {
            throw new NotImplementedException();
        }
    }
}
