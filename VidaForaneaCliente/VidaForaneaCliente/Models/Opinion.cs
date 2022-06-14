using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaForaneaCliente.Models
{
    public class Opinion
    {
        [JsonIgnore]
        public int id { get; set; }

        [JsonProperty("id")]
        private int IntAlternativeSetter
        {
            set { id = value; }
        }
        public string date { get; set; }
        public string hour { get; set; }
        public string description { get; set; }

        public int id_place { get; set; }
        public string student { get; set; }
        public int score { get; set; }

    }
}
