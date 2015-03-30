using Newtonsoft.Json;
using Rememba.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Model
{
    public class Content : IContent
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Data { get; set; }
        
        [JsonProperty(PropertyName = "t")]
        public string Touched { get; set; }
    }
}
