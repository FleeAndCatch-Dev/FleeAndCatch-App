using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models;
using Newtonsoft.Json;

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

        /// <summary>
        /// Get the command as json string.
        /// </summary>
        /// <returns>Json string.</returns>
        public abstract string GetCommand();

        public string Id => id;
        public string Type => type;
        public string ApiId => apiid;
        public string ErrorHandling => errorhandling;
        public ClientIdentification Identification => identification;
    }

    public enum CommandType
    {
        Connection, Synchronization, Control
    }
}
