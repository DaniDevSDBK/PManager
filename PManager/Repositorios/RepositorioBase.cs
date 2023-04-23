using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySqlConnector;

namespace PManager.Repositorios
{
    public abstract class RepositorioBase
    {
        private string dbFolder = "Data";
        private string dbFileName = "pm.db";
        private string appFolderPath = AppDomain.CurrentDomain.BaseDirectory; 
        private string dbPath = "";

        private readonly string cadenaConexion;

        public RepositorioBase()
        {

            cadenaConexion = $"Data Source={@"C:\Users\Usuario\Desktop\PManagerFolder\PManager\PManager\Data\pm.db"};Version=3;";
            ;
        }

        protected string GetConecction()=> cadenaConexion;
    }
}
