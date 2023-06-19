using PManager.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PManager.Repositorios
{
    public class UserRepo : RepositorioBase, UIRepositable
    {
        /// <summary>
        /// Obtiene la clave privada del usuario.
        /// </summary>
        /// <param name="userId">El ID del usuario.</param>
        /// <returns>La clave privada del usuario.</returns>
        public string GetUserPrivateKey(int userId)
        {
            var privateKey = "";

            using (var connection = new SQLiteConnection(GetConecction()))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT PrivateKey FROM User WHERE idUser = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            privateKey = reader[0].ToString();
                        }
                    }
                }
            }

            return privateKey;
        }

        /// <summary>
        /// Agrega un nuevo usuario.
        /// </summary>
        /// <param name="userModel">El modelo de usuario.</param>
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

        /// <summary>
        /// Verifica si un usuario existe en la base de datos.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <returns>True si el usuario existe, False si no existe.</returns>
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

        /// <summary>
        /// Autentica a un usuario.
        /// </summary>
        /// <param name="credential">Las credenciales del usuario.</param>
        /// <returns>True si el usuario es válido, False si no es válido.</returns>
        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser = false;

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

        /// <summary>
        /// Edita los datos de un usuario.
        /// </summary>
        /// <param name="userModel">El modelo de usuario.</param>
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

        /// <summary>
        /// Obtiene todos los usuarios.
        /// </summary>
        /// <returns>Una colección de modelos de usuario.</returns>
        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario.</param>
        /// <returns>El modelo de usuario.</returns>
        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene un usuario por su nombre de usuario.
        /// </summary>
        /// <param name="username">El nombre de usuario.</param>
        /// <returns>El modelo de usuario.</returns>
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

        /// <summary>
        /// Obtiene el tipo de usuario.
        /// </summary>
        /// <param name="id">El ID del usuario.</param>
        /// <returns>El tipo de usuario.</returns>
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

        /// <summary>
        /// Obtiene un usuario por su correo electrónico.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <returns>El modelo de usuario.</returns>
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

        /// <summary>
        /// Elimina un usuario por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario a eliminar.</param>
        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Suscribe un usuario por su correo electrónico.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <returns>True si se suscribe correctamente, False en caso contrario.</returns>
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

        /// <summary>
        /// Obtiene un usuario por su nombre de usuario (UIRepositable implementation).
        /// </summary>
        /// <param name="username">El nombre de usuario.</param>
        /// <returns>El modelo de usuario.</returns>
        UserModel UIRepositable.GetByUsername(string username)
        {
            Task<UserModel> user = GetByUsername(username);
            user.Wait();

            return user.Result;
        }

        /// <summary>
        /// Elimina un usuario de la aplicación.
        /// </summary>
        /// <param name="userAppName">El nombre de usuario de la aplicación.</param>
        /// <param name="userAppPassword">La contraseña de la aplicación.</param>
        /// <returns>True si se elimina correctamente, False en caso contrario.</returns>
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

        /// <summary>
        /// Obtiene los datos de un usuario de la aplicación.
        /// </summary>
        /// <param name="userAppName">El nombre de usuario de la aplicación.</param>
        /// <returns>El modelo de usuario de la aplicación.</returns>
        internal UserModel GetUserAppData(string userAppName)
        {
            UserModel user = null;

            using (var connection = new SQLiteConnection(GetConecction()))
            using (var command = new SQLiteCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM UserApp WHERE userAppName =@USERNAME";
                command.Parameters.Add("@USERNAME", DbType.String).Value = userAppName;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            UserName = reader[0].ToString(),
                            Name = reader[0].ToString(),
                            Email = reader[1].ToString(),
                            Password = reader[2].ToString(),
                        };
                    }
                }
            }

            return user;
        }

        public void InsertOrUpdateUserPrivateKey(int userId, string privateKey)
        {
            try
            {

                Task.Run(() =>
                {

                    using (var connection = new SQLiteConnection(GetConecction()))
                    using (var command = new SQLiteCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "UPDATE USER SET PrivateKey =@PRIVATEKEY WHERE idUser = @IDUSER";
                        command.Parameters.Add("@IDUSER", DbType.String).Value = userId;
                        command.Parameters.Add("@PRIVATEKEY", DbType.String).Value = privateKey;
                        command.ExecuteNonQuery();
                    }

                });

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
