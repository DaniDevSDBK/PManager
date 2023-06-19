using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
            try
            {
                dbPath = Path.Combine(appFolderPath, dbFolder, dbFileName);
                cadenaConexion = $"Data Source={dbPath};Version=3;";

                // Verificar la existencia de la carpeta
                if (!Directory.Exists(Path.Combine(appFolderPath, dbFolder)))
                {
                    Directory.CreateDirectory(Path.Combine(appFolderPath, dbFolder));
                }

                // Verificar la existencia del archivo de la base de datos
                if (!File.Exists(dbPath))
                {
                    SQLiteConnection.CreateFile(dbPath);
                }
            }
            catch
            {
            }


            VerificarTablas();
        }

        public void VerificarTablas()
        {
            string scriptPath = Path.Combine(appFolderPath, "Data", "pm.db.sql");

            // Verificar la existencia de una tabla específica
            bool tablaExiste = false;
            using (var connection = new SQLiteConnection(GetConecction()))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='USER' OR name='APP';", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        tablaExiste = reader.HasRows;
                    }
                }
            }

            // Si la tabla no existe, ejecutar el script SQL para crearla
            if (!tablaExiste)
            {
                string script = File.ReadAllText(scriptPath);

                using (var connection = new SQLiteConnection(GetConecction()))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand(script, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        protected string GetConecction()=> cadenaConexion;
    }
}
