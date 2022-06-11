using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaForaneaCliente.Models
{

    public class Student
    {
        [JsonIgnore]
        public int id { get; set; }
        public string name { get; set; }
        public string enrollment { get; set; }
        public string degree { get; set; }
        public string password { get; set; }
        public bool status { get; set; }
        public string rol { get; set; }

    }
}
