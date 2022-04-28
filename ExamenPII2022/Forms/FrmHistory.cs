using ExamenPII2022.Domain.DaoClime;
using ExamenPropII.AppCORE.IContracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamenPII2022.Forms
{
    public partial class FrmHistory : Form
    {
        private IClimeServices ClimeServices;
        public FrmHistory(IClimeServices climeServices)
        {
            this.ClimeServices = climeServices;
            InitializeComponent();
        }

        private void FrmHistory_Load(object sender, EventArgs e)
        {
            List<DaoHistory> list = new List<DaoHistory>();

            Random random = new Random();
            foreach (var item in ClimeServices.GetAll())
            {
                list.Add(new DaoHistory()
                {
                    Id = item.Id,
                    Lat = item.lat,
                    Long = item.lon,
                    Time_zone = item.timezone,
                    Timezoneoff = item.timezone_offset,

                });

            }

            dtgvData.DataSource = list;
        }
    }
}
