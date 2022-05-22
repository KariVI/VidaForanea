using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaForaneaCliente.Models
{
    public enum Estado
    {
        activo,
        inactivo
    }

    public class Student
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string enrollment { get; set; }
        public string degree { get; set; }
        public string password { get; set; }
        public Estado status { get; set; }

    }
}
