using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trabajo3.Data;

namespace Trabajo3
{
    public partial class Form1 : Form
    {
        ClienteRepository repo = new ClienteRepository();
        public Form1()
        {
            InitializeComponent();
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            tablas();
        }

        private void CargarTabla1()
        {
            dataGridView1.DataSource = repo.ObtenerTabla("SELECT * FROM ControlGimnasio");
        
        }
        private void CargarTabla2()
        {
            dataGridView2.DataSource = repo.ObtenerTabla("SELECT * FROM Trigger_RegistroControlLog");
        }
        
        private void CargarTabla3()
        {
            dataGridView3.DataSource = repo.ObtenerTabla("SELECT * FROM RegistroRutinas_Clientes");
        }
        private void tablas()
        {
            CargarTabla1();
            CargarTabla2();
            CargarTabla3();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                repo.EjecutarSP("SP_Insert", ObtenerDatosFormulario());
                MessageBox.Show("Operación exitosa", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tablas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private Dictionary<string, object> ObtenerDatosFormulario()
        {
            return new Dictionary<string, object>
    {
        { "@IdCliente",       txtId.Text },
        { "@NombreCompleto",  txtNombre.Text },
        { "@PorcentajeGrasa", txtGrasa.Text },
        { "@IdRutinaAsignada",        txtRutina.Text },
        { "@UltimoAcceso",    dtpUltimoAcceso.Value }
    };
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            repo.EjecutarSP("SP_Insert", ObtenerDatosFormulario());
            tablas();
        }

        private void Actualizar_Click(object sender, EventArgs e)
        {
            repo.EjecutarSP("SP_Update", ObtenerDatosFormulario());
            tablas();
        }
    }
}
