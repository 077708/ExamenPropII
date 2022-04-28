using Autofac;
using ExamenPII2022.Domain.Interfaces;
using ExamenPII2022.Forms;
using ExamenPII2022.Infraestructure.Repository;
using ExamenPropII.AppCORE.IContracts;
using ExamenPropII.AppCORE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamenPII2022
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ModelClimeWeather>().As<IClimeModel>();
            builder.RegisterType<ModelClimeServices>().As<IClimeServices>();

            var container = builder.Build();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmPresentation(container.Resolve<IClimeServices>()));
        }
    }
}
