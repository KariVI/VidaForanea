using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaForaneaCliente.Models
{
    public class Forum
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string degree { get; set; }
    }

    public enum degreeId {
        estadistica,
        tecnologiasComputacionales,
        redes,
        software

    }
}
