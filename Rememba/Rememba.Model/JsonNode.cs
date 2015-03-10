using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Model
{
    public class JsonNode
    {
        public JsonNode()
        {
            c = new List<JsonNode>();
            cid = "0";
        }

        public string id { get; set; }
        public string n { get; set; }
        public string d { get; set; }
        public string cid { get; set; }
        public List<JsonNode> c { get; set; }
    }
}
