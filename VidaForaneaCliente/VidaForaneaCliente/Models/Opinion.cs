using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaForaneaCliente.Models
{
    class Opinion
    {
        public int Id { get; set; }
        public string date { get; set; }
        public string hour { get; set; }
        public string description { get; set; }

        public int id_place { get; set; }
        public int id_student { get; set; }

    }
}
