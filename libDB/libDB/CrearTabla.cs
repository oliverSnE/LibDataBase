using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libDB;

namespace libDB
{
    public  class CrearTabla
    {
        public static bool crearEstadoTabla(string query)
        {

            IDB bd = new MySqlserverDB();
            bool res = bd.ejecutarQuery(query);
            if (res)
            {
                Tabla.Estado = FlagState.CREADA;
                return  true;
            }
            else
            {
                Tabla.Estado = FlagState.ERROR_SINTAXIS;
                return false;
            }

        }

    }
}
