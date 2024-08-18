using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;
using Negocio;
using System.Collections;

namespace negocio
{
    public class PokemonNegocio
    {
        public List<Pokemon> Listar() { 
            List<Pokemon> Lista = new List<Pokemon>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;


            try
            {
                conexion.ConnectionString = "server=DESKTOP-P72N324\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true";
                comando.CommandType= System.Data.CommandType.Text;
                comando.CommandText = "select Numero,Nombre, P.Descripcion, UrlImagen, t.Descripcion Tipo, d.Descripcion Debilidad, P.idTipo, P.IdDebilidad, P.Id from POKEMONS P, ELEMENTOS T, ELEMENTOS D where t.Id = p.IdTipo and d.Id = p.IdDebilidad AND p.Activo=1";
                comando.Connection = conexion;
                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read()) {
                    
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)lector["Id"];
                    aux.Numero = lector.GetInt32(0);
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];

                    if (!(lector["urlImagen"] is DBNull))
                        aux.urlImagen = (string)lector["urlImagen"];


                    aux.Tipo = new Elemento();
                    aux.Tipo.id = (int)lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.id = (int)lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)lector["Debilidad"];

                    Lista.Add(aux);

                }

            conexion.Close(); 
            return Lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<Pokemon> filtrar(string campo, string criterio, string filtro)
        {

            List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "select Numero,Nombre, P.Descripcion, UrlImagen, t.Descripcion Tipo, d.Descripcion Debilidad, P.idTipo, P.IdDebilidad, P.Id from POKEMONS P, ELEMENTOS T, ELEMENTOS D where t.Id = p.IdTipo and d.Id = p.IdDebilidad AND p.Activo=1 AND ";
                if (campo == "Número") {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Numero > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Numero < " + filtro;

                            break;
                        case "Igual a":
                            consulta += "Numero = " + filtro;
                            break;


                        default:
                            break;
                    }
                }else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '"+ filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";

                            break;
                        case "Contiene":
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "p.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "p.Descripcion like '%" + filtro + "'";

                            break;
                        case "Contiene":
                            consulta += "p.Descripcion like '%" + filtro + "%'";
                            break;
                        default:
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.EjecutarConsulta();
                while (datos.Lector.Read())
                {

                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["urlImagen"] is DBNull))
                        aux.urlImagen = (string)datos.Lector["urlImagen"];


                    aux.Tipo = new Elemento();
                    aux.Tipo.id = (int)datos.Lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.id = (int)datos.Lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    lista.Add(aux);

                }

                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        } 

        public void Agregar(Pokemon nuevo)
        {
            AccesoDatos Datos = new AccesoDatos();
            try
            {
                Datos.setearConsulta("insert into POKEMONS (Numero, Nombre,Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen) values ("+ nuevo.Numero + ",'" + nuevo.Nombre + "','" + nuevo.Descripcion + "',1, @idTipo, @idDebilidad, @UrlImagen)");
                Datos.SetearParametro("@idTipo", nuevo.Tipo.id);
                Datos.SetearParametro("@idDebilidad", nuevo.Debilidad.id);
                Datos.SetearParametro("@UrlImagen", nuevo.urlImagen);

                Datos.EjecutarAccion();
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
        public void Modificar(Pokemon modificado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update POKEMONS set Numero = @Numero, Nombre = @Nombre, Descripcion = @Descripcion, UrlImagen = @UrlImg, IdTipo = @IdTipo, IdDebilidad = @IdDebilidad where Id = @Id");
                datos.SetearParametro("@Numero",modificado.Numero);
                datos.SetearParametro("@Nombre",modificado.Nombre);
                datos.SetearParametro("@Descripcion",modificado.Descripcion);
                datos.SetearParametro("@UrlImg",modificado.urlImagen);
                datos.SetearParametro("@IdTipo",modificado.Tipo.id);
                datos.SetearParametro("@IdDebilidad",modificado.Debilidad.id);
                datos.SetearParametro("@Id",modificado.Id);


                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void EliminarFisico(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from POKEMONS where id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarConsulta();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void EliminarLogico(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("update POKEMONS set Activo = 0 where Id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarConsulta();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
