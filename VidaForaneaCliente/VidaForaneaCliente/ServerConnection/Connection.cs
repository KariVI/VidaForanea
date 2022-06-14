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
        public static Token token;
        public static string matricula { get; set; }
        public static string password { get; set; }

        public static void InitializeConnection()
        {

            client.BaseAddress = new Uri("http://192.168.0.180:9090");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
   
        }

        public static void AutheticateWithToken()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.accessToken);
        }

        public static void AutheticateWithRefresh()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.refreshToken);
        }

        private static void showConnectionError()
        {
            MessageBox.Show("Error en la conexión con el servidor", "Error de conexión", MessageBoxButton.OK);
        }

        public static async void RefreshToken()
        {
            try
            {
                string url = "/refresh";
                AutheticateWithRefresh();
                Student student = new Student() { enrollment = matricula, password = password };
                var json = JsonConvert.SerializeObject(student);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, data);
                if (response.IsSuccessStatusCode)
                {
                    var stringData = response.Content.ReadAsStringAsync().Result;
                    var preToken = JsonConvert.DeserializeObject<RootTokenRefresh>(stringData);
                    token.accessToken = preToken.token;
                    Console.WriteLine(token.refreshToken);
                }
                AutheticateWithToken();
                latestStatusCode = response.StatusCode;
            }
            catch (Exception e)
            {
                showConnectionError();
                Console.WriteLine(e.Message);
            }
        }

        public static async Task<Token> Login(string matricula, string password)
        {
            Student student = new Student() {enrollment = matricula, password = password };
            Token token = new Token() { accessToken = "null", refreshToken = "null"};
            try
            {
                string url = "/token";
                var json = JsonConvert.SerializeObject(student);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, data);
                if (response.IsSuccessStatusCode)
                {
                    var stringData = response.Content.ReadAsStringAsync().Result;
                    var preToken = JsonConvert.DeserializeObject<RootToken>(stringData);
                    token = preToken.makeToken();
                }
                latestStatusCode = response.StatusCode;
            }
            catch (Exception e)
            {
                showConnectionError();
                Console.WriteLine(e.Message);
            }
            return token;
        }

       
        public static async Task<List<Place>> GetPlacesByCategory(string estado, string category)
        {
            List <Place> places = new List<Place> ();
            try
            {
                string route;
                if (category.Equals(""))
                {
                    route = "lugares/" + estado ;
                }
                else
                {
                    route = "lugares/" + estado + "/" + category;
                }
                HttpResponseMessage response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {

                    var stringData = response.Content.ReadAsStringAsync().Result;
                   var places2 = JsonConvert.DeserializeObject<Root>(stringData);
                    places = places2.makeAList();
                }
                latestStatusCode = response.StatusCode;
                RefreshToken();
            } 
            catch (Exception e)
            {
                showConnectionError();
                Console.WriteLine(e.Message);
            }
            return places;
        }

        internal static async Task<bool> DeleteOpinion(Place place, Opinion opinion)
        {
            bool value = true;
            try
            {
                String idPlace = place.id.ToString();
                String rute = "/lugares/" + idPlace + "/opiniones/" + opinion.id;
                HttpResponseMessage response = await client.DeleteAsync(rute);
                latestStatusCode = response.StatusCode;
                Console.WriteLine(latestStatusCode);
                if (latestStatusCode != HttpStatusCode.NoContent)
                {
                    value = false;
                }
                RefreshToken();
            }
            catch (Exception e)
            {
                value = false;
                Console.WriteLine(e.Message);
            }
            return value;

        }
        public async static Task<bool> PutPlace(Place place, int id)
        {
            bool value = true;
            try
            {
                string url = "/lugares/" + id;
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(place);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url,data);
                latestStatusCode = response.StatusCode;
                if (latestStatusCode != HttpStatusCode.OK)
                {
                    value = false;
                }
                RefreshToken();
            }
            catch (Exception e)
            {
                showConnectionError();
                value = false;
                Console.WriteLine(e.Message);
            }
            return value;
    }

        public static async Task<List<Opinion>> GetOpinionsByPlace(Place place)
        {
            List<Opinion> opinions = new List<Opinion>();
            try
            {
                HttpResponseMessage response = await client.GetAsync("lugares/" + place.id + "/opiniones" );
                if (response.IsSuccessStatusCode)
                {

                    var stringData = response.Content.ReadAsStringAsync().Result;
                    var opinions2 = JsonConvert.DeserializeObject<List<Opinion>>(stringData);
                    opinions = opinions2;
                }
                latestStatusCode = response.StatusCode;
                RefreshToken();
            }
            catch (Exception e)
            {
                showConnectionError();
                Console.WriteLine(e.Message);
            }
            return opinions;
        }

        public static async Task<List<Comment>> GetCommentsByForum(Forum forum)
        {
            List<Comment> comments = new List<Comment>();
            try
            {
                HttpResponseMessage response = await client.GetAsync("foros/" + forum.Id + "/comentarios");
                if (response.IsSuccessStatusCode)
                {

                    var stringData = response.Content.ReadAsStringAsync().Result;
                    var comments2 = JsonConvert.DeserializeObject<List<Comment>>(stringData);
                    comments = comments2;
                    for (int i = 0; i < comments.Count; i++)
                    {
                        Console.WriteLine(comments[i].id);
                    }
                }
                latestStatusCode = response.StatusCode;
                RefreshToken();
            }
            catch (Exception e)
            {
                showConnectionError();
                Console.WriteLine(e.Message);
            }
            return comments;
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
                RefreshToken();

            }
            catch (Exception e)
            {
                showConnectionError();
                Console.WriteLine(e.Message);
            }
            return student;
        }

        public static async Task<bool> PostStudent(Student student)
        {
            bool value = true;
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("/estudiantes",student);
                if (response.IsSuccessStatusCode)
                {
                    student = await response.Content.ReadAsAsync<Student>();

                }
                latestStatusCode = response.StatusCode;
                Console.WriteLine(latestStatusCode);
                RefreshToken();

            }
            catch (Exception e)
            {
                value = false;
                showConnectionError();
                Console.WriteLine(e.Message);
            }
            return value;
        }

        public static async Task<bool> PostPlace(Place place,bool isAdmin)
        {
            bool value = true;
            try
            {
                if (isAdmin)
                {
                    place.status = StatusPlace.aprobado;
                }
                else
                {
                    place.status = StatusPlace.pendiente;
                }
                HttpResponseMessage response = await client.PostAsJsonAsync("/lugares", place);
                latestStatusCode = response.StatusCode;
                if (latestStatusCode != HttpStatusCode.Created)
                {
                    value = false;
                }
                RefreshToken();

            }
            catch (Exception e)
            {
                showConnectionError();
                value = false;
                Console.WriteLine(e.Message);
            }
            return value;
        }

        public static async Task<bool> PostOpinion(Place place, Opinion opinion)
        {
            bool value = true;
            try
            {

                String idPlace = place.id.ToString();
                String rute = "/lugares/" + idPlace + "/opiniones";
                HttpResponseMessage response = await client.PostAsJsonAsync(rute, opinion);
                latestStatusCode = response.StatusCode;
                if (latestStatusCode != HttpStatusCode.Created)
                {
                    value = false;
                }
                RefreshToken();

            }
            catch (Exception e)
            {
                showConnectionError();
                value = false;
                Console.WriteLine(e.Message);
            }
            return value;
        }
        public static async Task<bool> PostComment(Forum forum, Comment comment)
        {
            bool value = true;
            try
            {

                String idForum = forum.Id.ToString();
                String rute = "/foros/" + idForum + "/comentarios";
                HttpResponseMessage response = await client.PostAsJsonAsync(rute, comment);
                latestStatusCode = response.StatusCode;
                if (latestStatusCode != HttpStatusCode.Created)
                {
                    value = false;
                }
                RefreshToken();

            }
            catch (Exception e)
            {
                showConnectionError();
                value = false;
                Console.WriteLine(e.Message);
            }
            return value;
        }

        public static async Task<bool> DeleteComment(Forum forum, Comment comment)
        {
            bool value = true;
            try
            {
                String idForum = forum.Id.ToString();
                String rute = "/foros/" + idForum + "/comentarios/" + comment.id;
                HttpResponseMessage response = await client.DeleteAsync(rute);
                latestStatusCode = response.StatusCode;
                Console.WriteLine(latestStatusCode);
                if (latestStatusCode != HttpStatusCode.NoContent)
                {
                    value = false;
                }
                RefreshToken();

            }
            catch (Exception e)
            {
                showConnectionError();
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
            place.id = Convert.ToInt32(dict["id"]);
            place.name = (string)dict["name"];
            place.address = (string)dict["address"];
            place.services = (string)dict["services"];

            place.image = dict["image"].ToString();

            string statusRetrieved = (string)dict["status"];
            if (statusRetrieved.Equals("aprobado")) {
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
     class RootOpinion
    {
        public List<Dictionary<string, object>> Data { get; set; }
        public List<Opinion> makeAList()
        {
            List<Opinion> opinions = new List<Opinion>();
            foreach (var dict in Data)
            {
                Opinion opinion = new Opinion();
                opinion.id = Convert.ToInt32(dict["id"]);
                opinion.description = (string)dict["description"];
                opinion.date = (string)dict["date"];
                opinion.score = Convert.ToInt32(dict["score"]);
                opinion.id_place = Convert.ToInt32(dict["score"]);
                opinion.hour = (string)dict["hour"];
                opinion.student = (string)dict["user"];

            opinions.Add(opinion);
            }
            return opinions;
        }
    }

class RootComment
{
    public Comment[] Data { get; set; }
    
}

class RootTokenRefresh
{
    public string token { get; set; }

}

class RootToken
{
    public string access_token { get; set; }
    public string refresh_token { get; set; }

    public Token makeToken()
    {
        Token token = new Token();
        token.accessToken = access_token;
        token.refreshToken = refresh_token;
        return token;
    }
}


