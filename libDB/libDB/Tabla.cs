using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libDB;

namespace libDB
{
    public class Tabla
    {
        public string error;
        public int id;
        public static FlagState Estado;
        public string nombreTabla;
        public string TablaComentario;
        public List<Columna> columnas = new List<Columna>();

        public Tabla()
        {
            Tabla.Estado = FlagState.CREACION;
        }

        public bool EjecutaCrearTabla()
        {
            return libDB.CrearTabla.crearEstadoTabla(crearTabla());
        }

        public bool EjecutarInsert()
        {
            return insertDatos.crearEstadoInsert(insertarEnTabla());
        }

        public string crearTabla()
        {
            string sentencia = "";
           
            sentencia = "DROP TABLE IF EXISTS " + nombreTabla + "; CREATE TABLE IF NOT EXISTS ";
            try
            {
                sentencia += this.nombreTabla;
                int count = 0;
                sentencia += " ( id int(16) NOT NULL UNSIGNED PRIMARY KEY  auto_increment, ";
                
                foreach (Columna col in this.columnas)
                {
                    string longitud = "";
                    string noNull = "";
                    string _default = "";
                    string comentario = "";
                   
                    
                    if (col.longitud_columna != 0)
                    {
                        longitud = "("+col.longitud_columna+")";
                        if (col.tipoDeDato_columna == TipoDeCampo.Char || col.tipoDeDato_columna == TipoDeCampo.Char || col.tipoDeDato_columna == TipoDeCampo.Text || col.tipoDeDato_columna == TipoDeCampo.Varchar || col.tipoDeDato_columna == TipoDeCampo.Json)
                        {

                            _default = "DEFAULT '" + col.default_columna + "' ";
                        }
                        else
                        {
                            _default = "DEFAULT " + col.default_columna;
                        }
                    }

                    if (col.comentario_columna.Length > 0)
                    {
                        comentario = "COMMENT '" + col.comentario_columna + "' ";

                    } 
                    if (!col.es_null)
                    {
                        noNull = "NOT NULL";
                    }
                    else
                    {
                        noNull = "NULL";
                    }

                    sentencia += " "+col.nombre_columna + " " + col.tipoDeDato_columna + longitud + " " + noNull+" "+_default+" "+comentario;
                    count++;
                    if (count < columnas.Count)
                    {
                        sentencia += ",";
                    }
                }

                sentencia += " );";

            }
            catch (Exception e)
            {
                error = e.StackTrace.ToString();
                Tabla.Estado = FlagState.ERROR_SINTAXIS;
            }

            return sentencia;
        }

        public string insertarEnTabla()
        {
            string sentencia = "";
            sentencia = "INSERT INTO " + nombreTabla + " (";
            try
            {
                if (columnas[0].nombre_columna !="")
                {
                    sentencia += columnas[0].nombre_columna+",";
                }
                if (columnas[1].nombre_columna != "")
                {
                    sentencia += columnas[1].nombre_columna + ",";
                }
                if (columnas[2].nombre_columna != "")
                {
                    sentencia += columnas[2].nombre_columna + ",";
                }
                if (columnas[3].nombre_columna != "")
                {
                    sentencia += columnas[3].nombre_columna + ",";
                }
                if (columnas[4].nombre_columna != "")
                {
                    sentencia += columnas[4].nombre_columna;
                }
                sentencia += ") VALUES (" + columnas[0].value + "," + columnas[1].value + "," + columnas[2].value +
                             "," + columnas[3].value + "," + columnas[4].value + ");";
            }
            catch (Exception e)
            {
                error = e.StackTrace.ToString();
                Tabla.Estado = FlagState.ERROR_SINTAXIS;
            }
            return sentencia;
        }

        public class Columna
        {
            public object value;
            public string nombre_columna;
            public TipoDeCampo tipoDeDato_columna;
            public int longitud_columna;
            public bool es_null;
            public object default_columna;
            public TipoDeIndice indice_columna;
            public string comentario_columna;

            public Columna(object valor,string nombre, TipoDeCampo tipoDeCampo, int longitud, bool esNull, object defaultColumna, TipoDeIndice indice, string comentario)
            {
                this.value = valor;
                this.nombre_columna = nombre;
                this.tipoDeDato_columna = tipoDeCampo;
                this.longitud_columna = longitud;
                this.es_null = esNull;
                this.default_columna = defaultColumna;
                this.indice_columna = indice;
                this.comentario_columna = comentario;
            }
        }
    }

    public enum TipoDeCampo
    {
        TinyInt,Int,BigInt, Float,Double,Decimal,Char,Varchar,Text,LongText,Json,Date,Time,DateTime,Boolean
    }

    public enum TipoDeIndice
    {
        Nulo,Pri, Key, Unique, Mul
    }

    public class Clientes : Tabla
    {
        public class subCliente : Clientes
        {
            public List<Clientes> registros = new List<Clientes>();
        }
        public Clientes()
        {
            this.nombreTabla = "localidades";
            this.TablaComentario = "Tabla de los clientes que me deben dinero";
            //Columna 0
            this.columnas.Add(new Columna(null,"nombre_cliente",TipoDeCampo.Varchar,20,false,null,TipoDeIndice.Nulo,"Nombre de los clientes"));
            //Columna 1
            this.columnas.Add(new Columna(null, "apellidos_cliente", TipoDeCampo.Varchar, 50, false, null, TipoDeIndice.Nulo, "Apellidos de los clientes"));
            //Columna 2
            this.columnas.Add(new Columna(null, "telefono_cliente", TipoDeCampo.Varchar, 10,false, null, TipoDeIndice.Nulo, "teléfono de los clientes"));
            //Columna 3
            this.columnas.Add(new Columna(null, "correo_cliente", TipoDeCampo.Varchar, 50, true, "No definido", TipoDeIndice.Nulo, "Correo de los clientes"));
            //Columna 4
            this.columnas.Add(new Columna(null,"rfc_cliente", TipoDeCampo.Varchar, 13, false, null, TipoDeIndice.Nulo, "RFC de los clientes"));
        }

        public void crearCampo(string nombre, TipoDeCampo tipo, int longitud, bool esNull, object defaultColumna, TipoDeIndice indice, string comentario)
        {
            this.columnas.Add(new Columna(null,nombre,tipo,longitud,esNull,defaultColumna,indice,comentario));
        }
        
        public string nombre
        {
            set { this.columnas[0].value = value;}
            get { return this.columnas[0].value.ToString(); }
        }
        public string apellidos
        {
            set { this.columnas[1].value = value; }
            get { return this.columnas[1].value.ToString(); }
        }
        public string telefono
        {
            set
            {
                if (value.Length == 10)
                {
                    this.columnas[2].value = value;
                }
                else
                {
                    this.columnas[2].value = "Formato incorrecto";
                }
            }
            get {return this.columnas[2].value.ToString(); }
        }
        public string correo
        {
            set { this.columnas[3].value = value; }
            get { return this.columnas[3].value.ToString(); }
        }
        public string rfc
        {
            set { this.columnas[4].value = value; }
            get { return this.columnas[4].value.ToString(); }
        }
    }

    public enum FlagState
    {
        CREACION, CREADA, ALTERADA,BORRADA, ERROR_SINTAXIS
    }

    public class Ventas
    {
        //public Clientes cliente = new Clientes();

        //public void datosDeClientes()
        //{
        //    cliente.nombre = "Oliver";
        //    cliente.apellidos = "Burgara Estrella";
        //    cliente.correo = "oliver.burgara@gmail.com";
        //    cliente.telefono = "6622015980";
        //    cliente.rfc = "BUEO881014";
        //    string formato = cliente.telefono + "Incorrecto";
        //}
    }
}
