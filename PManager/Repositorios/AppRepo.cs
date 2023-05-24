using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualBasic.ApplicationServices;
using MySqlConnector;
using PManager.Model;
using PManager.ViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PManager.Repositorios
{
    public class AppRepo : UserRepo, AppRepositable
    {
        public void AddApp(AppModel appModel)
        {
            if (!CheckApp(appModel.AppName))
            {
                using (var connection = new SQLiteConnection(GetConecction()))
                using (var command = new SQLiteCommand())
                {
                    //Insert App Name
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO APP(NAME) VALUES('" + appModel.AppName.ToUpper() + "')";
                    command.ExecuteNonQuery();
                    connection.Close();

                    //Insert User Name and App Password
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO UserApp VALUES('" + GetByUsername(Thread.CurrentPrincipal.Identity.Name).Id + "', " + GetAppByName(appModel.AppName.ToUpper()).AppId + ",'" +appModel.UserAppName+"','"+appModel.AppPassword+"')";
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                using (var connection = new SQLiteConnection(GetConecction()))
                using (var command = new SQLiteCommand())
                {
                    //Insert User Name and App Password
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO UserApp VALUES('" + GetByUsername(Thread.CurrentPrincipal.Identity.Name).Id + "','" + appModel.UserAppName + "','" + appModel.AppPassword + "')";
                    command.ExecuteNonQuery();
                    connection.Close();
                }

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
                    while(reader.Read())
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

        public AppModel GetAppByName(string name)
        {
            AppModel app = null;

            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM APP WHERE NAME ='@APPNAME'";
                command.Parameters.Add("@APPNAME", DbType.String).Value = name;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        app=new AppModel()
                        {
                            AppId= Int32.Parse(reader[0].ToString()),
                            AppName = reader[1].ToString(),  
                        };
                    }
                }
            }

            return app;
        }

        public List<AppModel> UpdateData()
        {
            List<AppModel> appList = new List<AppModel>();
            AppModel app = null;

            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM APP JOIN UserApp USING(IDAPP)";
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

                        using (var connection2 = new SQLiteConnection(GetConecction()))
                        using (var command2 = new SQLiteCommand())
                        {
                            connection2.Open();
                            command2.Connection = connection2;
                            command2.CommandText = "SELECT COUNT(*) FROM APP JOIN USER_APP USING(IDAPP) WHERE NAME='" + app.AppName+"'";
                            using (var readerSub = command2.ExecuteReader())
                            {

                                while (readerSub.Read())
                                {
                                    app.UsersNumber = Int32.Parse(readerSub[0].ToString());
                                    app.PasswordsNumber = Int32.Parse(readerSub[0].ToString());

                                }
                            }

                        }
                            
                        appList.Add(app);
                    }
                }

            }

            return appList;
        }

        public List<ContentViewModel> GetContentList(string _appName)
        {

            List<ContentViewModel> contentList= new List<ContentViewModel>();

            ContentViewModel _content = null;

            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT userAppName, userAppPassword FROM APP JOIN UserApp USING(IDAPP) WHERE NAME='" + _appName+"'";
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
