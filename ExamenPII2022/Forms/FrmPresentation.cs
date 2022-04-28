using ExamenPII2022.Infraestructure.Repository;
using ExamenPropII.AppCORE.IContracts;
using System;
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
    public partial class FrmPresentation : Form
    {
        private IClimeServices climeServices;
        public FrmPresentation(IClimeServices climeServices)
        {
            this.climeServices = climeServices;
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            climeServices.Add(guna2TextBox1.Text);
            var data = climeServices.GetAll();
            guna2DataGridView1.DataSource = climeServices.GetAll();
        }
    }
}
