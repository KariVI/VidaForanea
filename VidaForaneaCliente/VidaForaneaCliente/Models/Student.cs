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
        public string nombre { get; set; }
        public string matricula { get; set; }
        public string licenciatura { get; set; }
        public string contrasenia { get; set; }
        public Estado estado { get; set; }

    }
}
