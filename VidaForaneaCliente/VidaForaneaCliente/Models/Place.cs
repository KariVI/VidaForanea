﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
using Newtonsoft.Json;

=======
using System.Windows.Media.Imaging;

>>>>>>> Gustavo
namespace VidaForaneaCliente.Models
{
   
    public class Place
    {
   
        public string name { get; set; }
       
        public string address { get; set; }
  
        public string services { get; set; }

        public string schedule { get; set; }


<<<<<<< HEAD
        public string type_place { get; set; }
   
=======
        public string image { get; set; }
>>>>>>> Gustavo
        public StatusPlace status { get; set; }
    }

    public enum StatusPlace
    {
        pendiente,
        aprobado
    }

    
}
