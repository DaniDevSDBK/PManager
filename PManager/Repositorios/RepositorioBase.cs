using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySqlConnector;

namespace PManager.Repositorios
{
    public abstract class RepositorioBase
    {
        private readonly string cadenaConexion;

        public RepositorioBase()
        {

            cadenaConexion = "server=localhost;port=3306;user id=root;password=CIFP1;database=myappxaml;Allow Zero Datetime=True;CHARSET=latin1";
        }

        protected MySqlConnection GetConecction()=> new MySqlConnection(cadenaConexion);
    }
}
