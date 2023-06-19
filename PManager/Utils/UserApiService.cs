using PManager.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace PManager.Utils
{
    /// <summary>
    /// Clase que proporciona métodos para interactuar con un servicio de API de usuario.
    /// </summary>
    public static class UserApiService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        static UserApiService()
        {
            httpClient.BaseAddress = new Uri("https://localhost/");
        }

        /// <summary>
        /// Inserta un suscriptor en el servicio de API de usuario.
        /// </summary>
        /// <param name="user">El objeto UserApiModel que representa el suscriptor.</param>
        public static async Task InsertSubscriber(UserApiModel user)
        {
            // URL del endpoint de registro
            var url = "https://localhost:443/User/register";

            // Realizar la solicitud POST
            var response = await httpClient.PostAsJsonAsync(url, user);

            // Verificar si la solicitud fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // Obtener la respuesta como texto
                var responseContent = await response.Content.ReadAsStringAsync();

                // Mostrar la respuesta en la consola
                Console.WriteLine(responseContent);
            }
            else
            {
                // La solicitud no fue exitosa, mostrar el código de estado
                Console.WriteLine("Error en la solicitud: " + response.StatusCode);
            }
        }

        /// <summary>
        /// Inicia sesión en el servicio de API de usuario y devuelve el token de autenticación.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <param name="password">La contraseña del usuario.</param>
        /// <returns>El token de autenticación.</returns>
        public static async Task<string> LogIn(string email, string password)
        {
            var requestContent = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("email", email),
            new KeyValuePair<string, string>("password", password)
        });

            var response = await httpClient.PostAsync("User/logIn", requestContent);
            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadAsStringAsync();
            return token;
        }

        /// <summary>
        /// Actualiza los datos del usuario en el servicio de API de usuario.
        /// </summary>
        internal static void UpdateUser()
        {
            throw new NotImplementedException();
        }
    }
}
