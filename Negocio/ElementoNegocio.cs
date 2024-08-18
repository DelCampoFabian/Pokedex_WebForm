using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace Negocio
{
    public class ElementoNegocio
    {
        public List<Elemento> Listar()
        {
			List<Elemento> ListaElemento = new List<Elemento>();
			AccesoDatos Datos = new AccesoDatos();
			try
			{
				Datos.setearConsulta("select id, descripcion from ELEMENTOS");
				Datos.EjecutarConsulta();

				while (Datos.Lector.Read())
				{
					Elemento aux = new Elemento();
					aux.id = (int)Datos.Lector["id"];
					aux.Descripcion = (string)Datos.Lector["descripcion"];

					ListaElemento.Add(aux);
				}

				return ListaElemento;
			}
			catch (Exception ex) 
			{

				throw ex;
			}
			finally
			{
				Datos.CerrarConexion();
			}
        }
    }
}
