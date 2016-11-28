using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComponentType;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Commands
{
    public class Client
    {
        [JsonProperty("id")]
        private int id;
        [JsonProperty("type")]
        private string type;
        [JsonProperty("subtype")]
        private string subtype;

        /// <summary>
        /// Create a client object for a json command.
        /// </summary>
        /// <param name="pId"></param>
        public Client(int pId)
        {
            this.id = pId;
            this.type = ClientType.Type.App.ToString();
            this.subtype = "null";
        }

        /// <summary>
        /// Get the client as a json object.
        /// </summary>
        /// <returns>JObject of the client.</returns>
        public JObject GetClient()
        {
            var jsonclient = new JObject
            {
                {"id", id},
                {"type", type},
                {"subtype", subtype}
            };
            return jsonclient;
        }

        public int Id => id;
        public string Type => type;
        public string SubType => subtype;
    }
}
