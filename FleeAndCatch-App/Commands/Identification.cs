using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Commands
{
    public class Identification
    {
        [JsonProperty("id")]
        private int id;
        [JsonProperty("address")]
        private string address;
        [JsonProperty("port")]
        private int port;
        [JsonProperty("type")]
        private string type;
        [JsonProperty("subtype")]
        private string subtype;

        public Identification(int pId, string pAddress, int pPort, string pType, string pSubtype)
        {
            id = pId;
            port = pPort;
            address = pAddress;
            type = pType;
            subtype = pSubtype;
        }

        public JObject GetJObject()
        {
            var jsonIdentification = new JObject
            {
                {"id", id},
                {"address", address},
                {"port", port},
                {"type", type},
                {"subtype", subtype}
            };
            return jsonIdentification;
        }

        public int Id => id;
        public string Address => address;
        public int Port => port;
        public string Type => type;
        public string Subtype => subtype;
    }
}
