using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace FleeAndCatch.Commands
{
    public abstract class Command
    {
        [JsonProperty("id")]
        protected string id;
        [JsonProperty("type")]
        protected string type;
        [JsonProperty("apiid")]
        protected string apiid;
        [JsonProperty("errorhandling")]
        protected string errorhandling;
        [JsonProperty("identification")]
        protected ClientIdentification identification;

        /// <summary>
        /// Create an object of the command.
        /// </summary>
        /// <param name="pId">Id of command, as command type.</param>
        /// <param name="pType">Type of command, as different command subtype.</param>
        protected Command(string pId, string pType, ClientIdentification pIdentification)
        {
            this.id = pId;
            this.type = pType;
            this.errorhandling = "ignoreerrors";
            this.apiid = "@@fleeandcatch@@";
            this.identification = pIdentification;
        }

        public virtual string ToJsonString()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
            try
            {
                var json = JsonConvert.SerializeObject(this, Formatting.None, settings);
                return json;
            }
            catch (Exception ex)
            {
                throw new Exception(300, "Json string could not parse");
            }
        }

        [JsonIgnore]
        public string Id => id;
        [JsonIgnore]
        public string Type => type;
        [JsonIgnore]
        public string ApiId => apiid;
        [JsonIgnore]
        public string ErrorHandling => errorhandling;
        [JsonIgnore]
        public ClientIdentification Identification => identification;
    }

    public enum CommandType
    {
        Undefined, Connection, Synchronization, Szenario, Exception
    }
}
