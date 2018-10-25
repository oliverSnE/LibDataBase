using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace libDB
{
    public interface IDB
    {
        /// <summary>
        /// Abre una conexión ya definida de DB.
        /// </summary>
        /// <returns>True: cuando se abre correctamente, false: cuando hay error.</returns>

        bool abrir();
        /// <summary>
        /// Cierra una conexión ya abierta de DB
        /// </summary>
        /// <returns>True: cuando se cierra, false: cuando hay error en el cierre de la conexión</returns>
        bool cerrar();
        /// <summary>
        /// Creará un registro nuevo
        /// </summary>
        /// <param name="tabla">tabla donde se inserta</param>
        /// <param name="campos">campos separados por comas</param>
        /// <param name="valores">Los valores con sus apostrofes y separados por comas</param>
        /// <returns>True: si se inserta correctamente, False: Si hay un error en la insersión</returns>
        bool insertar(string tabla, string campos, string valores);
        bool modificar(string tabla, string campoValores, string campoWhere, string where);
        bool borrar(string tabla, string campoWhere, string id);
        bool borradoLogico(string tabla, string id, string campo, string valor);
        DataTable consultaTodo(string campos, string tabla);
        DataTable consultaUnRegistro(string campo, string tabla, string campoWhere, string id);
        object consultaUnValor(string tabla, string campo, string id);
        DataTable consultaVarios(string tabla, string campos, string where);
        DataTable consultaVariosDt(string tabla, string campos, string campoCondicion, string where);
        bool autenticar(string campos, string tabla, string campo1, string valor1, string campo2, string valor2);
        DataTable buscarDato(string campo, string tabla, string campoWhere, string valor);
        List<object[]> consultaParaCombo(string campos, string tabla);
        bool consultaUnDato(string campo, string tabla, string campo1, string valor1, string campo2, string valor2);
        bool consultaUnDato2(string campo, string tabla, string where, string valorWhere);
        void lastId();
        bool ejecutarQuery(string query);
    }
}
