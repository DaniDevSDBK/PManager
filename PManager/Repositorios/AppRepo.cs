using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualBasic.ApplicationServices;
using PManager.Model;
using PManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading;
using System.Threading.Tasks;

namespace PManager.Repositorios
{
    public class AppRepo : UserRepo, AppRepositable
    {
        /// <summary>
        /// Agrega una nueva aplicación.
        /// </summary>
        /// <param name="appModel">El modelo de la aplicación.</param>
        public async void AddApp(AppModel appModel)
        {
            if (!CheckApp(appModel.AppName))
            {
                await InsertAppInfo(appModel);
                await InsertAppAdditionalInfo(appModel);
            }
            else
            {
                await InsertAppAdditionalInfo(appModel);
            }
        }

        private async Task InsertAppAdditionalInfo(AppModel appModel)
        {
            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                // Inserta el nombre de usuario y la contraseña de la aplicación.
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO UserApp VALUES('" + GetByEmail(Thread.CurrentPrincipal.Identity.Name).Id + "', " + GetAppByName(appModel.AppName.ToUpper()).AppId + ",'" + appModel.UserAppName + "','" + appModel.AppPassword + "')";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        /// <summary>
        /// Inserta la información de la aplicación.
        /// </summary>
        /// <param name="appModel">El modelo de la aplicación.</param>
        public async Task InsertAppInfo(AppModel appModel)
        {
            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                // Inserta el nombre de la aplicación.
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO APP(NAME) VALUES('" + appModel.AppName.ToUpper() + "')";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void DeleteApp(AppModel appModel)
        {
            throw new NotImplementedException();
        }

        public void EditApp(AppModel appModel)
        {
            throw new NotImplementedException();
        }

        public AppModel GetAppById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifica si la aplicación existe en la base de datos.
        /// </summary>
        /// <param name="name">El nombre de la aplicación.</param>
        /// <returns>True si la aplicación existe, False si no existe.</returns>
        public bool CheckApp(string name)
        {
            List<string> appName = new List<string>();

            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT NAME FROM APP";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        appName.Add(reader[0].ToString().ToUpper());
                    }
                }

                if (appName.Contains(name.ToUpper()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtiene la aplicación por su nombre
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AppModel GetAppByName(string name)
        {
            AppModel app = null;

            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM APP WHERE NAME = @APPNAME";
                command.Parameters.Add("@APPNAME", DbType.String).Value = name;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        app = new AppModel()
                        {
                            AppId = reader.GetInt32(0),
                            AppName = reader.GetString(1),
                        };
                    }
                }
            }

            return app;
        }

        /// <summary>
        /// Actualiza los datos según la id de usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<AppModel> UpdateData(int userId)
        {
            List<AppModel> appList = new List<AppModel>();
            AppModel app = null;

            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM APP JOIN UserApp USING(IDAPP) WHERE idUser = @IDUSER";
                command.Parameters.Add("@IDUSER", DbType.String).Value = userId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        app = new AppModel()
                        {
                            AppId = Int32.Parse(reader[0].ToString()),
                            AppName = reader[1].ToString(),
                            UserAppName = reader[3].ToString(),
                            AppPassword = reader[4].ToString(),
                        };

                        try
                        {
                            using (var connection2 = new SQLiteConnection(GetConecction()))
                            using (var command2 = new SQLiteCommand())
                            {
                                connection2.Open();
                                command2.Connection = connection2;
                                command2.CommandText = "SELECT COUNT(*) FROM UserApp JOIN APP USING(IDAPP) WHERE NAME=@APPNAME AND idUser=@IDUSER";
                                command2.Parameters.Add("@IDUSER", DbType.String).Value = userId;
                                command2.Parameters.Add("@APPNAME", DbType.String).Value = app.AppName;
                                using (var readerSub = command2.ExecuteReader())
                                {
                                    while (readerSub.Read())
                                    {
                                        app.UsersNumber = Int32.Parse(readerSub[0].ToString());
                                        app.PasswordsNumber = Int32.Parse(readerSub[0].ToString());
                                    }
                                }
                            }
                        }
                        catch
                        {
                            throw;
                        }

                        appList.Add(app);
                    }
                }
            }

            return appList;
        }

        /// <summary>
        /// Obtiene una lista con el contenido según el nombre de la aplicación
        /// </summary>
        /// <param name="_appName"></param>
        /// <returns></returns>
        public List<ContentViewModel> GetContentList(string appName, int id)
        {
            List<ContentViewModel> contentList = new List<ContentViewModel>();
            ContentViewModel _content = null;

            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT userAppName, userAppPassword FROM APP JOIN UserApp USING(IDAPP) WHERE NAME=@APPNAME AND IDUSER = @IDUSER";
                command.Parameters.Add("@APPNAME", DbType.String).Value = appName;
                command.Parameters.Add("@IDUSER", DbType.String).Value = id;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _content = new ContentViewModel()
                        {
                            UserAppName = reader[0].ToString(),
                            UserAppPassword = reader[1].ToString(),
                        };

                        contentList.Add(_content);
                    }
                }
            }

            return contentList;
        }
    }
}
