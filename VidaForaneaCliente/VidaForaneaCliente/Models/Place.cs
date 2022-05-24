using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VidaForaneaCliente.Models
{
   
    public class Place
    {
   
        public string name { get; set; }
       
        public string address { get; set; }
  
        public string services { get; set; }

        public string schedule { get; set; }


        public string type_place { get; set; }
   
        public StatusPlace status { get; set; }
    }

    public enum StatusPlace
    {
        pendiente,
        aprobado
    }
}
