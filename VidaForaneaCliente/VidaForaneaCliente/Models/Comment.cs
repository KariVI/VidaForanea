using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaForaneaCliente.Models
{
    public class Comment
    {
        [JsonIgnore]
        public int id { get; set; }

        [JsonProperty("id")]
        private int IntAlternativeSetter
        {
            // get is intentionally omitted here
            set { id = value; }
        }
        public string student { get; set; }
        public string date { get; set; }
        public string hour { get; set; }
        public string description { get; set; }
        public int forum_id { get; set; }




    }
}
