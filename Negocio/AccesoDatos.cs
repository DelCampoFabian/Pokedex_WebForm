using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace Negocio
{
    class AccesoDatos
    {
        private SqlConnection conexion ;
        private SqlCommand comando;
        private SqlDataReader lector; 
        public SqlDataReader Lector {
            get { return lector; }
        }

        //Constructor acceso a datos
        public AccesoDatos()
        {
            conexion = new SqlConnection("server=DESKTOP-P72N324\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true");
            comando = new SqlCommand();   
        }
        //Metodo hacer consulta
        public void setearConsulta(string consulta)
        {
            comando.CommandType= System.Data.CommandType.Text;
            comando.CommandText= consulta;
        }
        //Metodo ejecutar consulta
        public void EjecutarConsulta()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //Metodo Insertar 
        public void EjecutarAccion()
        {
            comando.Connection= conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex )
            {

                throw ex;
            }
        }
        //Metodo Setear Parametro

        public void SetearParametro(string parametro, object valor)
        {
            comando.Parameters.AddWithValue(parametro, valor);
        }

        public void CerrarConexion()
        {
            if (lector != null)
            {
                lector.Close();
            }
            conexion.Close();
        }

    }
}
