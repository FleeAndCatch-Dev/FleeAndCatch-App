using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands.Models
{
    public abstract class Identification
    {
        [JsonProperty("id")]
        protected int id;
        [JsonProperty("type")]
        protected string type;

        [JsonIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [JsonIgnore]
        public string Type => type;
    }

    public class ClientIdentification : Identification
    {
        [JsonProperty("address")]
        private string address;
        [JsonProperty("port")]
        private int port;

        public ClientIdentification()
        {
            
        }

        public ClientIdentification(int pId, string pType, string pAddress, int pPort)
        {
            id = pId;
            type = Enum.GetName(typeof(ComponentType.IdentificationType), ((ComponentType.IdentificationType)Enum.Parse(typeof(ComponentType.IdentificationType), pType)));   //Parse string to enum and check that the parameter is a part of the enum
            port = pPort;
            address = pAddress;
        }

        [JsonIgnore]
        public string Address => address;
        [JsonIgnore]
        public int Port => port;
    }

    public class RobotIdentification : Identification
    {
        [JsonProperty("subtype")]
        private string subtype;
        [JsonProperty("roletype")]
        private string roletype;

        public RobotIdentification()
        {
            
        }

        public RobotIdentification(int pId, string pType, string pSubType, string pRoleType)
        {
            id = pId;
            type = Enum.GetName(typeof(ComponentType.IdentificationType), ((ComponentType.IdentificationType)Enum.Parse(typeof(ComponentType.IdentificationType), pType)));
            subtype = Enum.GetName(typeof(ComponentType.RobotType), ((ComponentType.RobotType)Enum.Parse(typeof(ComponentType.RobotType), pSubType)));
            roletype = Enum.GetName(typeof(ComponentType.RoleType), ((ComponentType.RoleType)Enum.Parse(typeof(ComponentType.RoleType), pRoleType)));
        }

        [JsonIgnore]
        public string Subtype => subtype;
        [JsonIgnore]
        public string Roletype => roletype;
    }

    public class AppIdentification : Identification
    {
        [JsonProperty("roletype")]
        private string roletype;

        public AppIdentification()
        {
            
        }

        public AppIdentification(int pId, string pType, string pRoleType)
        {
            id = pId;
            type = Enum.GetName(typeof(ComponentType.IdentificationType), ((ComponentType.IdentificationType)Enum.Parse(typeof(ComponentType.IdentificationType), pType)));
            roletype = Enum.GetName(typeof(ComponentType.RoleType), ((ComponentType.RoleType)Enum.Parse(typeof(ComponentType.RoleType), pRoleType)));
        }

        [JsonIgnore]
        public string Roletype => roletype;
    }

    public class IdentificationConverter : JsonConverter
    {
        private readonly Type[] _types;

        public IdentificationConverter(params Type[] types)
        {
            _types = types;
        }

        public override bool CanConvert(Type objectType)
        {
            return _types.Any(t => t == objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object device = null;

            return device;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
