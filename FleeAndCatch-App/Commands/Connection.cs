using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ComponentType;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commands
{
    public class Connection : Command
    {
        [JsonProperty(PropertyName = "type")]
        private string type;
        [JsonProperty(PropertyName = "client")]
        private Client client;

        public Connection(string pId, string pType, Client pClient) : base()
        {
            this.id = pId;
            this.type = pType;
            this.client = pClient;
        }

        public override string GetCommand()
        {
            var command = new JObject
            {
                {"id", id},
                {"type", type},
                {"apiid", apiid},
                {"errorhandling", errorhandling},
                {"client", client.GetClient()}
            };

            return JsonConvert.SerializeObject(command);
        }

        public string Type => type;
        public Client Client => client;
    }

    public class ConnectionType : CommandType
    {
        public new enum Type
        {
            SetId, GetType, SetType, Disconnect
        }

        public ConnectionType(Type pType) : base((CommandType.Type) pType)
        {
        }
    }

    public class Client
    {
        [JsonProperty(PropertyName = "id")]
        private int id;
        [JsonProperty(PropertyName = "type")]
        private string type;
        [JsonProperty(PropertyName = "subtype")]
        private string subtype;

        public Client(int pId)
        {
            this.id = pId;
            this.type = ClientType.Type.App.ToString();
            this.subtype = "null";
        }

        public JObject GetClient()
        {
            var jsonclient = new JObject
            {
                {"id", id},
                { "type", type.ToString()},
                { "subtype", subtype.ToString()}               
            };
            return jsonclient;
        }

        public int Id => id;
        public string Type => type;
        public string SubType => subtype;
    }
}
