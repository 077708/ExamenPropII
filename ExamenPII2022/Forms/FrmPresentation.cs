using ExamenPII2022.Domain.DaoClime;
using ExamenPII2022.Infraestructure.Repository;
using ExamenPropII.AppCORE.IContracts;
using Guna.Charts.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ExamenPII2022.Domain.Entities.ClimeWeather;

namespace ExamenPII2022.Forms
{
    public partial class FrmPresentation : Form
    {
        private IClimeServices climeServices;
        public FrmPresentation(IClimeServices climeServices)
        {
            this.climeServices = climeServices;
            InitializeComponent();
            ChargebyIdPro(1);
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        public void Spline(int id)
        {
            try
            {
                if (climeServices.GetAll().Count > 0)
                {
                    var p = climeServices.GetAll()[id - 1].hourly;
                    List<double> temperaturas = new List<double>();
                    List<string> hours = new List<string>();

                    foreach (var item in p)
                    {
                        temperaturas.Add(item.temp);
                    }

                    foreach (var item in p)
                    {
                        hours.Add(UnixTimeStampToDateTime(item.dt).ToShortTimeString());
                    }

                    ChartStadistics.YAxes.GridLines.Display = false;

                    //Create a new dataset 
                    var dataset = new Guna.Charts.WinForms.GunaSplineDataset();
                    dataset.PointRadius = 3;
                    dataset.PointStyle = PointStyle.Circle;
                    var r = new Random();

                    for (int i = 0; i < hours.Count; i++)
                    {
                        temperaturas[i] -= 273;
                        dataset.DataPoints.Add(hours[i], (temperaturas[i]));
                    }


                    //Add a new dataset to a chart.Datasets
                    ChartStadistics.Datasets.Add(dataset);

                    //An update was made to re-render the chart
                    ChartStadistics.Update();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        public void CharTxtById(int Id)
        {
            try
            {
                if (climeServices.GetAll().Count > 0)
                {
                    var dat = climeServices.GetAll()[Id - 1].hourly[0];

                    txtTemp.Text = (dat.temp - 273.12).ToString() + " C";
                    txtViento.Text = dat.wind_speed.ToString() + "KM/H";
                    txtPrecipitación.Text = dat.pressure.ToString();
                    txtHumidity.Text = dat.humidity.ToString();
                    txtLat.Text = dat.weather[0].description;
                    txtLong.Text = UnixTimeStampToDateTime(climeServices.GetAll()[Id - 1].current.sunrise).ToShortTimeString();
                    txtTimeZone.Text = climeServices.GetAll()[Id - 1].timezone;
                    label5.Text = UnixTimeStampToDateTime(climeServices.GetAll()[Id - 1].current.sunset).ToShortTimeString();
                    pictureIcon.ImageLocation = $"https://openweathermap.org/img/w/{dat.weather[0].icon}{".png"}";
                }
                else
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnGo_Click(object sender, EventArgs e)
        {
            double dt = ToUnixTime(dtSearch.Value);

            try
            {
                if (txtSearch.Text == String.Empty)
                {
                    throw new Exception("Por favor ingresa una ciudad");
                }
                climeServices.Add(txtSearch.Text, Math.Floor(dt));
                ChargebyIdPro(climeServices.GetAll()[climeServices.GetAll().Count - 1].Id);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch

            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;

        }
        private double ToUnixTime(DateTime input)
        {
            return input.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            FrmHistory frmHistory = new FrmHistory(this.climeServices);
            frmHistory.ShowDialog();
        }

        private void DaoHourlyGet(int id)
        {
            List<DaoHourly> daoHourlies = new List<DaoHourly>();

            try
            {
                if (climeServices.GetAll().Count > 0)
                {
                    foreach (var item in climeServices.GetAll()[id - 1].hourly)
                    {
                        daoHourlies.Add(new DaoHourly()
                        {
                            Hora = UnixTimeStampToDateTime(item.dt).ToShortTimeString(),
                            Temperatura = item.temp - 273.12,
                            Presión = item.pressure,
                            Nubes = item.clouds,
                            Visibilidad = item.visibility,
                            Velocidad_viento = item.wind_speed,
                        });
                    }

                    dtgvData.DataSource = daoHourlies;
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void DaoCountry(int id)
        {
            try
            {
                List<DaoName> list = new List<DaoName>();
                foreach (var item in climeServices.GetAll())
                {
                    list.Add(new DaoName()
                    {
                        Id = item.Id,
                        Time_zone = item.timezone,
                    });
                }
                DataGridWeather.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChargebyIdPro(int id)
        {
            try
            {
                CharTxtById(id);
                Spline(id);
                DaoHourlyGet(id);
                DaoCountry(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DataGridWeather_CellContentDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id = 0;
                if ((int)DataGridWeather.Rows.Count > 0)
                    id = (int)DataGridWeather.Rows[DataGridWeather.CurrentRow.Index].Cells[0].Value;
                ChargebyIdPro(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
