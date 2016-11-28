using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commands
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
        public string errorhandling;

        /// <summary>
        /// Create an object of the command.
        /// </summary>
        /// <param name="pId">Id of command, as command type.</param>
        /// <param name="pType">Type of command, as different command subtype.</param>
        protected Command(string pId, string pType)
        {
            this.id = pId;
            this.type = pType;
            this.errorhandling = "ignoreerrors";
            this.apiid = "@@fleeandcatch@@";
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
    }

    public class CommandType
    {
        protected string name;
        protected Type type;

        /// <summary>
        /// Enumeration of the command types.
        /// </summary>
        public enum Type
        {
            Connection, Synchronisation
        }

        /// <summary>
        /// Create an object of the command type.
        /// </summary>
        /// <param name="pType">Type of the command.</param>
        public CommandType(Type pType)
        {
            this.name = pType.ToString();
            this.type = pType;
        }
    }
}
