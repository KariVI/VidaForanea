using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VidaForaneaCliente.Models;
using System.Windows;
using Newtonsoft.Json;
namespace VidaForaneaCliente.ServerConnection
{
    public class Connection
    {
        static HttpClient client = new HttpClient();
        public static HttpStatusCode latestStatusCode;

        public static void initializeConnection()
        {
<<<<<<< HEAD
            client.BaseAddress = new Uri("http://192.168.0.25:9090");
=======
            client.BaseAddress = new Uri("http://10.50.14.14:9090/");
>>>>>>> main
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
   
        }

        public static async Task<Student> Login(string matricula, string password)
        {
            Student student = new Student() { password = password };
            try
            {
                string url = "login/" + matricula;
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(student);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, data);
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

        
        public static async Task<Admin> LoginAdmin(string usuario, string password)
        {
            Admin admin = new Admin() { contrasenia = password };
            try
            {
                string url = "loginAdmin/" + usuario;
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(admin);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, data);
                if (response.IsSuccessStatusCode)
                {
                    admin = await response.Content.ReadAsAsync<Admin>();
                }
                latestStatusCode = response.StatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return admin;
        }
        public static async Task<List<Place>> GetPlacesByCategory(string estado, string category)
        {
            List <Place> places = new List<Place> ();
            try
            {
                HttpResponseMessage response = await client.GetAsync("lugares/" + estado + "/" + category);
                if (response.IsSuccessStatusCode)
                {

                    var stringData = response.Content.ReadAsStringAsync().Result;
                   var places2 = JsonConvert.DeserializeObject<Root>(stringData);
                    places = places2.makeAList();
                }
                latestStatusCode = response.StatusCode;
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return places;
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

        public static async Task<bool> PostStudent(Student student)
        {
            bool value = true;
            try
            {
                 Console.WriteLine(student.name);
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
                value = false;
                Console.WriteLine(e.Message);
            }
            return value;
        }

        public static async Task<bool> PostPlace(Place place)
        {
            bool value = true;
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("/lugares", place);
                latestStatusCode = response.StatusCode;
                if (latestStatusCode != HttpStatusCode.OK)
                {
                    value = false;
                }
            }
            catch (Exception e)
            {
                value = false;
                Console.WriteLine(e.Message);
            }
            return value;
        }
    }
}
class Root
{
    public List<Dictionary<string, object>> Data { get; set; }
    public List<Place> makeAList()
    {
        List<Place> places = new List<Place>();
        foreach (var dict in Data)
        {
            Place place = new Place(); 
            StatusPlace status = new StatusPlace();
            place.name = (string)dict["name"];
            place.address = (string)dict["address"];
            place.services = (string)dict["services"]; 
            string statusRetrieved = (string)dict["status"];
            if (statusRetrieved.Equals("aprobado")){
                status = StatusPlace.aprobado;
            }
            else
            {
                status = StatusPlace.pendiente;
            }
            place.status = status;
            place.type_place = (string)dict["type_place"];
            places.Add(place);
        }
        return places;
    }
}