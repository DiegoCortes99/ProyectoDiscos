using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class DiscosNegocio
    {
        public List<Discos> listar()
        {
            List<Discos> lista = new List<Discos>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Select Titulo,FechaLanzamiento,CantidadCanciones,UrlImagenTapa, E.Descripcion Estilo, T.Descripcion Tipos, D.IdEstilo, D.IdTipoEdicion, D.Id from DISCOS D, ESTILOS E, TIPOSEDICION T WHERE D.IdEstilo = E.Id AND D.IdTipoEdicion = T.Id AND Activo = 1");
                datos.ejecutarConsulta();

                while (datos.Lector.Read())
                {
                    Discos aux = new Discos();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Titulo = (string)datos.Lector["Titulo"];
                    aux.FechaLanzamiento = (DateTime)datos.Lector["FechaLanzamiento"];
                    aux.CantidadCanciones = (int)datos.Lector["CantidadCanciones"];
                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("UrlImagenTapa"))))
                    {
                        aux.UrlImagen = (string)datos.Lector["UrlImagenTapa"];

                    }
                    aux.Estilo = new Estilos();
                    aux.Estilo.Id = (int)datos.Lector["IdEstilo"];
                    aux.Estilo.Descripcion = (string)datos.Lector["Estilo"];

                    aux.Tipo = new TipoEdicion();
                    aux.Tipo.Id = (int)datos.Lector["IdTipoEdicion"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipos"];


                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error inesperado: " + ex.Message);
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void agregar(Discos disco)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("Insert into DISCOS (Titulo, FechaLanzamiento, CantidadCanciones, UrlImagenTapa, IdEstilo, IdTipoEdicion) values (@titulo,@fechalanza,@cantidadcanc,@urlimg,@idestilo,@idtipoedicion)");
                datos.setearParametro("@titulo",disco.Titulo);
                datos.setearParametro("@fechalanza", disco.FechaLanzamiento);
                datos.setearParametro("@cantidadcanc",disco.CantidadCanciones);
                datos.setearParametro("@urlimg",disco.UrlImagen);
                datos.setearParametro("@idestilo",disco.Estilo.Id);
                datos.setearParametro("@idtipoedicion",disco.Tipo.Id);
                datos.ejecutarConsultaParametros();

            }
            catch (Exception ex)
            {

                throw new Exception("Error al agregar articulo: " + ex.Message,ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Discos disco)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("update DISCOS set Titulo = @titulo, FechaLanzamiento = @fechalanza, CantidadCanciones = @cantidadcan, UrlImagenTapa = @urlimg,IdEstilo = @idestilo, IdTipoEdicion = @idtipo where id = @id");
                datos.setearParametro("@titulo", disco.Titulo);
                datos.setearParametro("@fechalanza", disco.FechaLanzamiento);
                datos.setearParametro("@cantidadcan", disco.CantidadCanciones);
                datos.setearParametro("@urlimg", disco.UrlImagen);
                datos.setearParametro("@idestilo", disco.Estilo.Id);
                datos.setearParametro("@idtipo", disco.Tipo.Id);
                datos.setearParametro("@id", disco.Id);
                datos.ejecutarConsultaParametros();
            }
            catch (Exception ex)
            {

                throw new Exception("Error al Modificar articulo: " + ex.Message, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminar(Discos disco)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("delete from DISCOS where id = @id");
                datos.setearParametro("@id", disco.Id);
                datos.ejecutarConsultaParametros();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminarLogico(Discos disco)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update DISCOS set Activo = 0 where Id = @id");
                datos.setearParametro("@id",disco.Id);
                datos.ejecutarConsultaParametros();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public List<Discos>filtradoAvanzado(string campo, string criterio, string filtro)
        {
            List<Discos> lista = new List<Discos>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "Select Titulo,FechaLanzamiento,CantidadCanciones,UrlImagenTapa, E.Descripcion Estilo, T.Descripcion Tipos, D.IdEstilo, D.IdTipoEdicion, D.Id from DISCOS D, ESTILOS E, TIPOSEDICION T WHERE D.IdEstilo = E.Id AND D.IdTipoEdicion = T.Id AND Activo = 1 AND ";

                if (campo == "Titulo")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Titulo like '" + filtro + "%' ";
                            break;

                        case "Termina con":
                            consulta += "Titulo like '%" + filtro + "' ";
                            break;

                        case "Contiene":
                            consulta += "Titulo like '%" + filtro + "%' ";
                            break;                        
                    }
                }
                else
                {
                    switch(criterio)
                    {
                        case "Mayor a":
                            consulta += "CantidadCanciones > " + filtro;
                            break;

                        case "Menor a":
                            consulta += "CantidadCanciones < " + filtro;
                            break;

                        case "Igual a":
                            consulta += "CantidadCanciones = " + filtro;
                            break;                        
                    }
                }

                datos.setearConsulta(consulta);
                datos.ejecutarConsulta();

                while (datos.Lector.Read())
                {
                    Discos aux = new Discos();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Titulo = (string)datos.Lector["Titulo"];
                    aux.FechaLanzamiento = (DateTime)datos.Lector["FechaLanzamiento"];
                    aux.CantidadCanciones = (int)datos.Lector["CantidadCanciones"];
                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("UrlImagenTapa"))))
                    {
                        aux.UrlImagen = (string)datos.Lector["UrlImagenTapa"];

                    }
                    aux.Estilo = new Estilos();
                    aux.Estilo.Id = (int)datos.Lector["IdEstilo"];
                    aux.Estilo.Descripcion = (string)datos.Lector["Estilo"];

                    aux.Tipo = new TipoEdicion();
                    aux.Tipo.Id = (int)datos.Lector["IdTipoEdicion"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipos"];


                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
