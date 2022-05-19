using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaForaneaCliente.Models
{
  

    public class Admin
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public string usuario { get; set; }
        public string contrasenia { get; set; }
        public Estado estado { get; set; }

    }
}
