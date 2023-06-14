using Microsoft.VisualBasic.ApplicationServices;
using MySqlConnector;
using PManager.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PManager.Repositorios
{
    public class UserRepo : RepositorioBase, UIRepositable
    {
        public void Add(UserModel userModel)
        {
            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO USER (name, email, password, profile_picture) VALUES ( @name, @email, @password, @picture );";
                command.Parameters.AddWithValue("@name", userModel.Name);
                command.Parameters.AddWithValue("@email", userModel.Email);
                command.Parameters.AddWithValue("@password", userModel.Password);
                command.Parameters.AddWithValue("@picture", userModel.ProfilePicture);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public bool UserExists(string email)
        {
            bool validUser = false;

            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                connection.Close();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM USER WHERE email =@email";
                command.Parameters.AddWithValue("@email", email);
                validUser = command.ExecuteScalar() == null ? false : true;
            }

            return validUser;

        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser= false;

            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                connection.Close();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM USER WHERE EMAIL =@EMAIL AND PASSWORD =@PASSWORD";
                command.Parameters.Add(new SQLiteParameter("@EMAIL", DbType.String) { Value = credential.UserName });
                command.Parameters.Add(new SQLiteParameter("@PASSWORD", DbType.String) { Value = credential.Password });
                validUser = command.ExecuteScalar() == null ? false : true;
            }

            return validUser;
        }

        public void Edit(UserModel userModel)
        {

            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE USER SET name = @name, email = @email, profile_picture = @profile_picture WHERE idUser = @id";
                command.Parameters.AddWithValue("@name", userModel.Name);
                command.Parameters.AddWithValue("@email", userModel.Email);
                command.Parameters.AddWithValue("@profile_picture", userModel.ProfilePicture);
                command.Parameters.AddWithValue("@id", userModel.Id);
                command.ExecuteNonQuery();
                connection.Close();
            }

        }

        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }

        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> GetByUsername(string username)
        {
            UserModel user = null;

            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM USER WHERE NAME =@USERNAME";
                command.Parameters.Add("@USERNAME", DbType.String).Value = username;
                ImageBrush imgBrush = new ImageBrush();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = Int32.Parse(reader[0].ToString()),
                            UserName = reader[1].ToString(),
                            Name = reader[1].ToString(),
                            Email = reader[2].ToString(),
                            ProfilePicture = (byte[])reader[4],
                        };
                    }
                }
            }

            user.Type = await GetUserType(user.Id);

            return user;
        }

        private async Task<int> GetUserType(int id)
        {

            var n = 0;

            try
            {
                using (var connection = new SQLiteConnection(GetConecction()))
                using (var command = new SQLiteCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT IDTYPE FROM UserType WHERE idUser =@idUser";
                    command.Parameters.Add("@idUser", DbType.String).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            n = reader.GetInt32(0);
                        }
                    }
                }
            }
            catch
            {
                n = 0;
            }

            return n;
        }

        public UserModel GetByEmail(string email)
        {
            UserModel user = null;

            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM USER WHERE EMAIL =@USEREMAIL";
                command.Parameters.Add("@USEREMAIL", DbType.String).Value = email;
                ImageBrush imgBrush = new ImageBrush();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = Int32.Parse(reader[0].ToString()),
                            UserName = reader[1].ToString(),
                            Name = reader[1].ToString(),
                            Password = reader[3].ToString(),
                            Email = reader[2].ToString(),
                            ProfilePicture = (byte[])reader[4],
                        };
                    }
                }
            }

            user.Type = GetUserType(user.Id).Result;

            return user;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool SuscribeUser(string email)
        {
            var suscribed = false;

            var idUser = GetByEmail(email).Id;

            Task.Run(() =>
            {
                try
                {

                    using (var connection = new SQLiteConnection(GetConecction()))
                    using (var command = new SQLiteCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO UserType VALUES(@IDUSER, 1);";
                        command.Parameters.Add("@IDUSER", DbType.String).Value = idUser;
                        command.ExecuteNonQuery();
                    }

                    suscribed = true;
                }
                catch (Exception ex) 
                {
                    throw;
                }

            });

            return suscribed;

        }

        UserModel UIRepositable.GetByUsername(string username)
        {
            Task<UserModel> user = GetByUsername(username);
            user.Wait();

            return user.Result;
        }

        internal bool DeleteUserApp(string userAppName, string userAppPassword)
        {
            var deleted = false;

            try
            {

                Task.Run(() =>
                {

                    using (var connection = new SQLiteConnection(GetConecction()))
                    using (var command = new SQLiteCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "DELETE FROM UserApp WHERE userAppName = @USERNAME AND userAppPassword = @USERPWD;";
                        command.Parameters.Add("@USERNAME", DbType.String).Value = userAppName;
                        command.Parameters.Add("@USERPWD", DbType.String).Value = userAppPassword;
                        command.ExecuteNonQuery();
                    }

                    deleted = true;

                });

            }
            catch (Exception ex)
            {
                throw;
            }

            return deleted;
        }
    }
}
