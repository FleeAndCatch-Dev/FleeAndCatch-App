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

        protected Command(string pId, string pType)
        {
            this.id = pId;
            this.type = pType;
            this.errorhandling = "ignoreerrors";
            this.apiid = "@@fleeandcatch@@";
        }

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

        public enum Type
        {
            Connection
        }

        public CommandType(Type pType)
        {
            this.name = pType.ToString();
            this.type = pType;
        }
    }
}
