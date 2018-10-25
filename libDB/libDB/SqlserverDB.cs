using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace libDB
{
    public class SqlserverDB : IDB
    {
        public static string errorMsg;
        public SqlConnection cn;
        public SqlCommand cmd;
        public static SqlDataReader dr;
        public SqlDataAdapter da;
        public DataTable dt;
        public static int idAutenticacion;
        public static int idUsuario;
        public static string nom;
        public static string ap1;
        public static int numError;
        public static string concepto;
        public static int idMaestro;
        public static int selectLastId;
        public static string nomGrupo;
        public static string correo;
        public int resultadoDeNonQuery;
        /// <summary>
        /// Conexión a base de datos Sql
        /// </summary>
        /// <param name="server">Nombre del Servidor</param>
        /// <param name="database">Nombre de la base de datos</param>
        /// <param name="user">Nombre del usuario</param>
        /// <param name="password">Contraseña</param>
        public SqlserverDB(string server, string database, string user, string password)
        {
            string cs = "Server =" + server + "; Database =" + database + "; User Id =" + user + "; Password =" + password + ";";
            cn = new SqlConnection(cs);
        }//cierra construsctor de SqlServerDB

        /// <summary>
        /// Abre la conexión establecida previamente
        /// </summary>
        /// <returns>True: si se abre, false: si hay error. (Consultar variable Sqlserver errorMsg.Message)</returns>
        public bool abrir()
        {
            bool res = false;
            try
            {
                cn.Open();
                res = true;


            }
            catch (SqlException Sqle)
            {

                errorMsg = "Error Sql al abrir la conexión.\n" + Sqle.Message + "\n" + Sqle.Number;

            }
            catch (Exception ex)
            {
                errorMsg = "Error general al abrir conexión.\n" + ex.Message;
            }

            return res;
        } // llave cierra metodo abrir
        /// <summary>
        /// Cierra una conexión previamente abierta
        /// </summary>
        /// <returns>True: si se cierra, False: si hay algun error. (Consulta error en errorMsg.Message)</returns>
        public bool cerrar()
        {
            bool res = false;
            try
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                    res = true;
                }

            }
            catch (SqlException Sqle)
            {
                errorMsg = "Error Sql al cerrar conexión.\n" + Sqle.ToString();
            }

            catch (Exception ex)
            {

                errorMsg = "Error General al cerrar conexión.\n" + ex.ToString();
            }
            return res;

        }//Cierra metodo cerrar

        /// <summary>
        /// Inserta un un registro en la tabla especificada con los datos establecidos.
        /// </summary>
        /// <param name="tabla">Nombre de la tabla donde se va a insertar</param>
        /// <param name="campos"> Los Campos en el orden que se insertan separados por comas</param>
        /// <param name="valores">Valores en el orden de los campos en su formato correcto. ('valor'), (valor numerico)</param>
        /// <returns>True: si se inserta correctamente, False: si hay error (consultar errorMsg)</returns>
        bool IDB.insertar(string tabla, string campos, string valores)
        {
            bool res = false;
            try
            {
                abrir();
                cmd = new SqlCommand("INSERT INTO " + tabla + " (" + campos + ") VALUES (" + valores + ");", cn);
                cmd.ExecuteNonQuery();
                res = true;

            }
            catch (SqlException Sqle)
            {
                errorMsg = "Error Sql al insertar.\n " + Sqle.Number;
                numError = Sqle.Number;
            }
            catch (Exception ex)
            {

                errorMsg = "Error general al insertar. \n" + ex.ToString();
            }
            finally
            {
                cerrar();

            }

            return res;
        }//Cierra llave del método insertar.

        /// <summary>
        /// Modifica un un registro en la tabla especificada con los datos establecidos.
        /// </summary>
        /// <param name="tabla">Nombre de la tabla donde se va a modifica</param>
        /// <param name="campoValores"> Los Campos en el orden que se modifica separados por comas (campo1='valor1',campo2=valor2, campoN=valorN)</param>
        /// <param name="where">Criterio para seleccionar el registro a modificar (id) </param>
        /// <returns>True: si se modifica correctamente, False: si hay error (consultar errorMsg)</returns>
        bool IDB.modificar(string tabla, string campoValores, string campoWhere, string where)
        {
            bool res = false;
            try
            {
                abrir();
                cmd = new SqlCommand("UPDATE " + tabla + " SET " + campoValores + "  WHERE " + campoWhere + "= " + where + ";", cn);
                cmd.ExecuteNonQuery();
                res = true;


            }
            catch (SqlException Sqle)
            {
                errorMsg = "Error Sql al modificar.\n " + Sqle.ToString();
            }
            catch (Exception ex)
            {

                errorMsg = "Error general al modificar. \n" + ex.ToString();
            }
            finally
            {
                cerrar();

            }

            return res;
        }//Cierra llave del método modificar.
        /// <summary>
        /// Elimina un registro de la tabla especificada 
        /// </summary>
        /// <param name="tabla">Nombre de la tabla donde se va a borrar el dato</param>
        /// <param name="id">ID del registro a borrar. El campo deberá de llamarse id</param>
        /// <returns>True: se elimina registro, false: No se eliminó. (consulta errorMsg)</returns>
        bool IDB.borrar(string tabla, string campoWhere, string id)
        {
            bool res = false;
            try
            {
                abrir();
                cmd = new SqlCommand("DELETE FROM " + tabla + " WHERE " + campoWhere + "= " + id + ";", cn);
                cmd.ExecuteNonQuery();
                res = true;

            }
            catch (SqlException Sqle)
            {
                errorMsg = "Error Sql al borrar registro.\n" + Sqle.ToString();
            }
            catch (Exception ex)
            {
                errorMsg = "Error general al borrar.\n" + ex.ToString();


            }
            finally
            {
                cerrar();
            }

            return res;

        }//Cierra metodo borrar
        /// <summary>
        /// Realizamos un Borrado logico de la tabla especificada
        /// </summary>
        /// <param name="tabla">Tabla donde se va a modificar</param>
        /// <param name="campoValor">Los Campos en el orden que se modifica separados por comas</param>
        /// <param name="id">id del campo que se va a ver afectado el campo deberá llamarse ID</param>
        /// <returns>True si se realiza el borrado lógico o false si no se realiza</returns>
        bool IDB.borradoLogico(string tabla, string campo, string valor, string id)
        {
            bool res = false;
            try
            {
                abrir();
                cmd = new SqlCommand("UPDATE " + tabla + " SET " + campo + "=" + valor + " WHERE id=" + id + ";", cn);
                cmd.ExecuteNonQuery();
                res = true;
            }
            catch (SqlException Sqle)
            {
                errorMsg = "Error Sql al realizar el borrado.\n " + Sqle.Message;
            }
            catch (Exception ex)
            {

                errorMsg = "Error general. \n" + ex.Message;
            }
            finally
            {
                cerrar();
            }

            return res;
        }//Cierra la llave del borrado lógico
        /// <summary>
        /// Realiza una consulta con todos los datos de la tabla especificada
        /// </summary>
        /// <param name="tabla">tabla en donde va a realizar la busqueda</param>
        /// <returns>regresa el DataTable</returns>
        DataTable IDB.consultaTodo(string campos, string tabla)
        {
            dt = new DataTable();

            try
            {

                if (abrir() == false)
                {
                    errorMsg = "La conoxión de sql no pudo abrirse";
                }
                else
                {
                    da = new SqlDataAdapter("SELECT " + campos + " FROM " + tabla + ";", cn);

                    da.Fill(dt);
                    if (dt.Rows.Count <= 0)
                    {
                        dt = null;
                        errorMsg = "No hay registros que consultar.";
                    }

                }
                // cmd = new SqlCommand("SELECT * FROM " + tabla + ";", cn);
                //cmd.ExecuteReader();


            }
            catch (SqlException Sqle)
            {
                errorMsg = "Error Sql al cargar los datos. \n" + Sqle.Message;
            }
            catch (Exception ex)
            {

                errorMsg = "Error general. \n" + ex.Message;
            }
            finally
            {
                cerrar();
            }

            return dt;
        }

        /// <summary>
        /// Realiza la busqueda de un solo valor de la tabla y campos especificados
        /// </summary>
        /// <param name="tabla">Tabla donde se realiza la busqueda</param>
        /// <param name="campo">Campo de la tabla en donde buscas el valor</param>
        /// <param name="id">Valor valor del registro específico.</param>
        /// <returns>True si se realiza la buscada correctamente o false si no.</returns>
        object IDB.consultaUnValor(string tabla, string campo, string id)
        {
            object res = false;
            try
            {
                abrir();
                cmd = new SqlCommand("SELECT " + campo + " FROM " + tabla + " WHERE id=" + id + ";");
                res = cmd.ExecuteScalar();


            }
            catch (SqlException Sqle)
            {
                errorMsg = "Error al realizar la consulta.\n" + Sqle.Message;
            }
            catch (Exception ex)
            {

                errorMsg = "Error general.\n" + ex.Message;
            }
            finally
            {
                cerrar();
            }

            return res;
        }//Cierra llave de consultarUnValor
        /// <summary>
        /// Realiza la consulta de un registro de la tabla especificado
        /// </summary>
        /// <param name="tabla">Tabla de donde se realiza la busqueda</param>
        /// <param name="campo">campo de la tabla </param>
        /// <param name="id">Id de donde quiere el valor</param>
        /// <returns></returns>
        DataTable IDB.consultaUnRegistro(string campo, string tabla, string campoWhere, string id)
        {
            dt = new DataTable();
            dt = null;

            try
            {
                if (abrir() == false)
                {
                    errorMsg = "No fue posible abrir la conexión a la base de datos.";
                }
                else
                {

                    da = new SqlDataAdapter();

                    da = new SqlDataAdapter("SELECT " + campo + " FROM " + tabla + " WHERE " + campoWhere + "= " + id + " ;", cn);
                    da.Fill(dt);
                    if (dt.Rows.Count <= 0)
                    {
                        dt = null;
                        errorMsg = "No se encuentran registros.";
                    }

                }



            }
            catch (SqlException Sqle)
            {
                errorMsg = "Error Sql al consultal el registro. \n" + Sqle.ToString();
            }
            catch (Exception ex)
            {

                errorMsg = "Error general. \n" + ex.Message;
            }
            finally
            {
                cerrar();
            }
            return dt;

        }//Cierra llaves de IDataReader
         /// <summary>
         /// Consulta varios registros de la tabla especificada
         /// </summary>
         /// <param name="tabla">tabla de donde de donde necesita conseguir la consulta</param>
         /// <param name="campos">campos que necesita</param>
         /// <param name="where">condición</param>
         /// <returns>Regresa DataTable</returns>
        DataTable IDB.consultaVariosDt(string tabla, string campos, string campoCondicion, string where)
        {
            //dt = null;
            dt = new DataTable();
            try
            {
                abrir();
                da = new SqlDataAdapter("SELECT " + campos + " FROM " + tabla + " WHERE " + campoCondicion + "= " + where + ";", cn);
                //cmd.ExecuteReader();

                da.Fill(dt);


            }
            catch (SqlException Sqle)
            {
                errorMsg = "Error en la Sql server. \n" + Sqle.Message;
            }
            catch (Exception ex)
            {

                errorMsg = "Error general, Notifique a soporte técnico.\n " + ex.Message;
            }
            finally
            {
                cerrar();
            }

            return dt;
        }
        /// <summary>
        /// Consulta Varios valores
        /// </summary>
        /// <param name="tabla">Tabla donde se necesitan los datos</param>
        /// <param name="campos">campos de la tabla </param>
        /// <param name="where">condición</param>
        /// <returns></returns>
        DataTable IDB.consultaVarios(string tabla, string campos, string where)
        {
            dt = null;
            try
            {
                abrir();
                cmd = new SqlCommand("SELECT " + campos + " FROM " + tabla + " WHERE id=" + where + ";", cn);
                cmd.ExecuteReader();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (SqlException Sqle)
            {
                errorMsg = "Error en Sql.\n" + Sqle.Message;
            }
            catch (Exception ex)
            {

                errorMsg = "Error general, llame a soporte técnico.\n" + ex.Message;
            }
            finally
            {
                cerrar();
            }
            return dt;
        }
        /// <summary>
        /// Iniciar sesion en el sistema
        /// </summary>
        /// <param name="tabla">Nombre de la tabla de la consulta</param>
        /// <param name="campos"> campos que desea consultar</param>
        /// <param name="campo">campos condiciones de sesion</param>
        /// <param name="valor">valor del campo</param>
        /// <returns>return true si hay conicidencia false si no</returns>
        bool IDB.autenticar(string campos, string tabla, string campo1, string valor1, string campo2, string valor2)
        {
            idAutenticacion = 0;
            idUsuario = 0;
            bool res = false;
            try
            {
                abrir();
                cmd = new SqlCommand("SELECT " + campos + " FROM " + tabla + " WHERE " + campo1 + "='" + valor1 + "' AND " + campo2 + "= '" + valor2 + "';", cn);
                dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    idAutenticacion = Convert.ToInt32(dr[1].ToString());
                    idUsuario = Convert.ToInt32(dr[0].ToString());
                    nom = dr[2].ToString();
                    ap1 = dr[3].ToString();
                    idMaestro = Convert.ToInt32(dr[8].ToString());

                    res = true;
                }
                else
                {
                    errorMsg = "Usuario y/o contraseña incorrecta";

                }
            }
            catch (SqlException sqle)
            {
                errorMsg = "Error en Sql.\n" + sqle.Message;

            }
            catch (Exception ex)
            {

                errorMsg = "Error general, llame a soporte técnico.\n" + ex.Message;
            }
            finally
            {
                cerrar();
            }

            return res;
        }
        /// <summary>
        /// Realiza la busqueda de un registro que tenga incidencias en el inicio
        /// </summary>
        /// <param name="campo">campo de la tabla que buscas</param>
        /// <param name="tabla">tabla en donde buscas el dato</param>
        /// <param name="campoWhere">tabla donde realizas la busqueda</param>
        /// <param name="valor">valor que buscas</param>
        /// <returns></returns>
        DataTable IDB.buscarDato(string campo, string tabla, string campoWhere, string valor)
        {
            dt = new DataTable();
            try
            {
                if (abrir() == false)
                {
                    errorMsg = "No fue posible abrir la conexión.";
                }
                else
                {

                    da = new SqlDataAdapter("SELECT " + campo + " FROM " + tabla + " WHERE " + campoWhere + " LIKE '" + valor + "%';", cn);
                    da.Fill(dt);
                    if (dt.Rows.Count <= 0)
                    {
                        errorMsg = "No existen registros con ese criterio de búsqueda.";
                    }

                }
                //cmd = new SqlCommand("SELECT " + campo + " FROM " + tabla + " WHERE " + campoWhere + " LIKE '" + valor + "%';", cn);
                //cmd.ExecuteReader();


            }
            catch (SqlException sqle)
            {
                errorMsg = "Error en sql. \n" + sqle.Message;
            }
            catch (Exception ex)
            {

                errorMsg = "Error general. Consulte con soporte técnico. \n" + ex.Message;
            }
            finally
            {
                cerrar();
            }
            return dt;
        }

        bool IDB.consultaUnDato(string campo, string tabla, string campo1, string valor1, string campo2, string valor2)
        {
            bool res = false;
            try
            {
                if (abrir() == false)
                {
                    errorMsg = "No fue posible abrir la conexión a la base de datos.";
                }
                else
                {
                    cmd = new SqlCommand("SELECT " + campo + " FROM " + tabla + " WHERE " + campo1 + " = " + valor1 + " AND "+campo2+" = '"+valor2+"' ;)", cn);
                    dr = cmd.ExecuteReader();
                    dr.Read();

                    if (dr.HasRows)
                    {
                        concepto = dr.GetString(0);
                        res = true;
                    }
                }

            }
            catch (SqlException Sqle)
            {
                errorMsg = "Error Sql al consultal el registro. \n" + Sqle.ToString();
            }
            catch (Exception ex)
            {

                errorMsg = "Error general. \n" + ex.Message;
            }
            finally
            {
                cerrar();
            }
            return res;

        }//Cierra llaves de IDataReader

        bool IDB.consultaUnDato2(string campo, string tabla, string where, string valorWhere)
        {
            bool res = false;
            try
            {
                if (abrir() == false)
                {
                    errorMsg = "No se pudo abrir la conexión de Mysql";
                }
                else
                {
                    cmd = new SqlCommand("SELECT " + campo + " FROM " + tabla + " WHERE " + where + "=" + valorWhere + ";",cn);
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        nomGrupo = dr.GetString(0);
                        correo = dr.GetString(0);
                        res = true;

                    }
                   

                }

            }

            catch (SqlException MySqle)
            {
                errorMsg = "Error MySql al consultal el registro. \n" + MySqle.ToString();
            }
            catch (Exception ex)
            {

                errorMsg = "Error general. \n" + ex.Message;
            }
            finally
            {
                cerrar();
            }



            return res;
        }


        void IDB.lastId()
        {


            try
            {
                if (abrir() == false)
                {
                    errorMsg = "No se pudo abrir la conexión Mysql.";

                }
                else
                {
                    cmd = new SqlCommand("SELECT LAST_INSERT_ID()", cn);
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        selectLastId = dr.GetInt32(0);
                        
                    }
                }

            }
            catch (SqlException mysqle)
            {
                errorMsg = "Error en MySql.\n" + mysqle.Message;

            }
            catch (Exception ex)
            {

                errorMsg = "Error general, llame a soporte técnico.\n" + ex.Message;
            }
            finally
            {
                cerrar();
            }


        }

        List<object[]> IDB.consultaParaCombo(string campos, string tabla)
        {
            List<object[]> lista = new List<object[]>();
            object[] arreglo;
            try
            {

                if (abrir() == false)
                {
                    errorMsg = "La conoxión de sql no pudo abrirse";
                }
                else
                {
                    cmd = new SqlCommand("SELECT " + campos + " FROM " + tabla + ";", cn);

                    dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            arreglo = new object[dr.FieldCount];
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                arreglo[i] = dr.GetValue(i);

                            }
                            lista.Add(arreglo);

                        }
                    }
                    else
                    {
                        errorMsg = "No existen registros para esta consulta";
                    }

                }

            }
            catch (SqlException Sqle)
            {
                errorMsg = "Error Sql al cargar los datos. \n" + Sqle.Message;
            }
            catch (Exception ex)
            {

                errorMsg = "Error general. \n" + ex.Message;
            }
            finally
            {
                cerrar();
            }

            return lista;
        }

        bool IDB.ejecutarQuery(string query)
        {
            bool res = false;
            try
            {
                if (abrir() == false)
                {
                    errorMsg = "No fue posible abrir la conexión.";
                }
                else
                {
                    cmd = new SqlCommand(query, cn);
                    resultadoDeNonQuery = cmd.ExecuteNonQuery();
                    res = true;
                }
            }
            catch (SqlException msqe)
            {
                errorMsg = "Error en Mysql. " + msqe.Message;
            }
            catch (Exception e)
            {
                errorMsg = e.ToString();
            }

            finally
            {
                cerrar();
            }

            return res;
        }


    }//Cierra la llave de clase
}
