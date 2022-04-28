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
            try
            {
                List<DaoHistory> list = new List<DaoHistory>();

                if (ClimeServices.GetAll().Count > 0)
                {
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
    }
}
