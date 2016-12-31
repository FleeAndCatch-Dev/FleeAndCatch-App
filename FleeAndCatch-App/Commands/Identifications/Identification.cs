using System;
using Commands.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commands.Identifications
{
    public abstract class Identification
    {
        [JsonProperty("id")]
        protected int id;

        public abstract JObject GetJObject();

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }

    public class ClientIdentification : Identification
    {
        [JsonProperty("type")]
        private string type;
        [JsonProperty("address")]
        private string address;
        [JsonProperty("port")]
        private int port;

        public ClientIdentification(int pId, string pType, string pAddress, int pPort)
        {
            id = pId;
            type = Enum.GetName(typeof(ComponentType.IdentificationType), ((ComponentType.IdentificationType)Enum.Parse(typeof(ComponentType.IdentificationType), pType)));   //Parse string to enum and check that the parameter is a part of the enum
            port = pPort;
            address = pAddress;
        }

        public override JObject GetJObject()
        {
            var jsonIdentification = new JObject
            {
                {"id", id},
                {"address", address},
                {"port", port},
                {"type", type}
            };
            return jsonIdentification;
        }

        public string Type => type;
        public string Address => address;
        public int Port => port;
    }

    public class RobotIdentification : Identification
    {
        [JsonProperty("subtype")]
        private string subtype;
        [JsonProperty("roletype")]
        private string roletype;

        public RobotIdentification(int pId, string pSubType, string pRoleType)
        {
            id = pId;
            subtype = Enum.GetName(typeof(ComponentType.RobotType), ((ComponentType.RobotType)Enum.Parse(typeof(ComponentType.RobotType), pSubType)));
            roletype = Enum.GetName(typeof(ComponentType.RoleType), ((ComponentType.RoleType)Enum.Parse(typeof(ComponentType.RoleType), pRoleType)));
        }

        public override JObject GetJObject()
        {
            var jsonIdentification = new JObject
            {
                {"id", id},
                {"subtype", subtype},
                {"roletype", roletype}
            };
            return jsonIdentification;
        }

        public string Subtype => subtype;
        public string Roletype => roletype;
    }

    public class AppIdentification : Identification
    {
        [JsonProperty("roletype")]
        private string roletype;

        public AppIdentification(int pId, string pRoleType)
        {
            id = pId;
            roletype = Enum.GetName(typeof(ComponentType.RoleType), ((ComponentType.RoleType)Enum.Parse(typeof(ComponentType.RoleType), pRoleType)));
        }

        public override JObject GetJObject()
        {
            var jsonIdentification = new JObject
            {
                {"id", id},
                {"roletype", roletype}
            };
            return jsonIdentification;
        }

        public string Roletype => roletype;
    }
}
