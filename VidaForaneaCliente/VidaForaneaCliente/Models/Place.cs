using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace VidaForaneaCliente.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string services { get; set; }
        public string schedule { get; set; }

        public string type_place { get; set; }

        public string image { get; set; }
        public StatusPlace status { get; set; }
    }

    public enum StatusPlace
    {
        pendiente,
        aprobado
    }

    
}
