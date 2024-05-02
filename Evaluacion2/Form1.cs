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

namespace Evaluacion2
{
    public partial class Form1 : Form
    {
        string connectionString = Properties.Settings.Default.cnx;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarEmpleados();
            CargarPedidos();
        }

        private void CargarPedidos()
        {
            // Llamar al procedimiento almacenado para obtener todos los pedidos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("pa_listaOrdenesempleado", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dtPedidos = new DataTable(); // Crear DataTable para almacenar los pedidos
                adapter.Fill(dtPedidos);

                // Mostrar todos los pedidos en el DataGridView
                dataGridView1.DataSource = dtPedidos;
            }
        }

        private void CargarEmpleados()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT DISTINCT ApeEmpleado FROM dbo.EMPLEADO", connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        cmbEmpleado.Items.Add(reader["ApeEmpleado"].ToString());
                    }
                    reader.Close();
                }
            }
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            // Obtener el ID del empleado seleccionado en el ComboBox
            string nombreCompleto = cmbEmpleado.SelectedItem.ToString();

            // Dividir el nombre completo en apellido y nombre
            string[] nombreParts = nombreCompleto.Split(',');
            string apellido = nombreParts[0].Trim(); // Obtener el apellido

            // Obtener las fechas de inicio y fin
            DateTime fechaInicio, fechaFin;
            if (!DateTime.TryParse(dtpInicio.Text, out fechaInicio) || !DateTime.TryParse(dtpFin.Text, out fechaFin))
            {
                MessageBox.Show("Por favor, ingrese fechas válidas.");
                return;
            }

            // Llamar al procedimiento almacenado para obtener los pedidos por empleado y fechas
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("pa_listaOrdenesPorEmpleado", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApeEmpleado", apellido); // Pasar el apellido como parámetro
                command.Parameters.AddWithValue("@f1", fechaInicio);
                command.Parameters.AddWithValue("@f2", fechaFin);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dtPedidos = new DataTable();
                adapter.Fill(dtPedidos);

                // Mostrar los resultados en el DataGridView
                dataGridView1.DataSource = dtPedidos;

                // Cambiar el nombre de las columnas del DataGridView
                dataGridView1.Columns["IdPedido"].HeaderText = "ID Pedido";
                dataGridView1.Columns["FechaPedido"].HeaderText = "Fecha Pedido";
                dataGridView1.Columns["NombreCia"].HeaderText = "Nombre de la Compañía";
                dataGridView1.Columns["Direccion"].HeaderText = "Dirección";
                dataGridView1.Columns["CiudadDestinatario"].HeaderText = "Ciudad Destinatario";
                dataGridView1.Columns["PaisDestinatario"].HeaderText = "País Destinatario";
                dataGridView1.Columns["ApeEmpleado"].HeaderText = "Apellido del Empleado";

                dataGridView1.Columns["Total"].HeaderText = "Total";

                // Mostrar la cantidad de órdenes en el TextBox
                totalResultado.Text = dtPedidos.Rows.Count.ToString() + " registros";
            }
        }

        private void btnMTodo_Click(object sender, EventArgs e)
        {
             using (SqlConnection connection = new SqlConnection(connectionString))
    {
        SqlCommand command = new SqlCommand("pa_listaOrdenesempleado", connection);
        command.CommandType = CommandType.StoredProcedure;

        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataTable dtPedidos = new DataTable(); // Crear DataTable para almacenar los pedidos
        adapter.Fill(dtPedidos);

        // Mostrar todos los pedidos en el DataGridView
        dataGridView1.DataSource = dtPedidos;
                // Limpiar el TextBox
                totalResultado.Text = "";
            }
        }
    }
}


