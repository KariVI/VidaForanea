using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VidaForaneaCliente.Models;

namespace VidaForaneaCliente.ServerConnection
{
    public class Connection
    {
        static HttpClient client = new HttpClient();
        public static HttpStatusCode latestStatusCode;

        public static void initializeConnection()
        {
            client.BaseAddress = new Uri("http://192.168.0.25:9090/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
   
        }

        public static async Task<Student> GetStudentByMatricula(string matricula)
        {
            Student student = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync("estudiantes/" + matricula);
                if (response.IsSuccessStatusCode)
                {
                    student = await response.Content.ReadAsAsync<Student>();
                }
                latestStatusCode = response.StatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return student;
        }

        public static async void PostStudent(Student student)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("/estudiantes",student);
                Console.WriteLine("Post");
                if (response.IsSuccessStatusCode)
                {
                    student = await response.Content.ReadAsAsync<Student>();

                }
                latestStatusCode = response.StatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



    }
}
