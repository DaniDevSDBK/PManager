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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
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
                command.CommandText = "INSERT INTO USER (name, email, password) VALUES ( @name, @email, @password );";
                command.Parameters.AddWithValue("@name", userModel.Name);
                command.Parameters.AddWithValue("@email", userModel.Email);
                command.Parameters.AddWithValue("@password", userModel.Password);
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
                command.CommandText = "SELECT * FROM USER WHERE NAME =@USERNAME AND PASSWORD =@PASSWORD";
                command.Parameters.Add(new SQLiteParameter("@USERNAME", DbType.String) { Value = credential.UserName });
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

        public UserModel GetByUsername(string username)
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

            return user;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

    }
}
