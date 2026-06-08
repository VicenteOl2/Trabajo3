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
                    { "@IdCliente",txtId.Text },
                    { "@NombreCompleto",txtNombre.Text },
                    { "@PorcentajeGrasa",txtGrasa.Text },
                    { "@IdRutinaAsignada",txtRutina.Text },
                    { "@UltimoAcceso",dtpUltimoAcceso.Value }
                };
                    }

        
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Ingresa un IdCliente válido", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            repo.EjecutarSP("SP_Insert", ObtenerDatosFormulario());
            tablas();
        }
        

        private void Actualizar_Click(object sender, EventArgs e)
        {
            repo.EjecutarSP("SP_Update", ObtenerDatosFormulario());
            tablas();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // evita click en el header

            DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];

            txtId.Text = fila.Cells["IdCliente"].Value?.ToString();
            txtNombre.Text = fila.Cells["NombreCompleto"].Value?.ToString();
            txtGrasa.Text = fila.Cells["PorcentajeGrasa"].Value?.ToString();
            txtRutina.Text = fila.Cells["IdRutinaAsignada"].Value?.ToString();

            if (DateTime.TryParse(fila.Cells["UltimoAcceso"].Value?.ToString(), out DateTime fecha))
                dtpUltimoAcceso.Value = fecha;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int idRutina = int.Parse(txtR.Text);

            
            decimal promedio = repo.ObtenerPromedioGrasa(idRutina);
            lblPromedio.Text = promedio.ToString("F2");

          
            dataGridView4.DataSource = repo.ObtenerClientesPorRutina(idRutina);
        }

        private void DELETE_Click(object sender, EventArgs e)
        {
            repo.EjecutarSP("SP_Delete", new Dictionary<string, object>{{ "@IdCliente", txtId.Text } });
            tablas();
        }
    }
}
