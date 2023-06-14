using PManager.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace PManager.Utils
{

    public static class UserApiService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        static UserApiService()
        {
            httpClient.BaseAddress = new Uri("https://localhost/");
        }

        public static async Task InsertSuscribtor(UserApiModel user)
        {

            // Crear una instancia de HttpClient
            var httpClient = new HttpClient();

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

        internal static void UpdateUser()
        {
            throw new NotImplementedException();
        }
    }
}
