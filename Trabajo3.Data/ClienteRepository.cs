using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Trabajo3.Data
{
    public class ClienteRepository
    {


        private string ConnectionString = ConfigurationManager.ConnectionStrings["MiDB"].ConnectionString;



        public void EjecutarSP(string nombreSP, Dictionary<string, object> parametros)
        {
            
                using (SqlConnection cn = new SqlConnection(ConnectionString))
                {
                    cn.Open();
                    using (var cmd = new SqlCommand(nombreSP, cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (var par in parametros)
                        {
                            cmd.Parameters.AddWithValue(par.Key, par.Value ?? DBNull.Value);
                        }
                        cmd.ExecuteNonQuery();
                    }
                }

        }
 
        public DataTable ObtenerTabla(string query)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt; 
                }
            }
        }
    }
}
