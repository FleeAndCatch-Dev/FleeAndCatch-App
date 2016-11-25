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
        [JsonProperty(PropertyName = "id")]
        protected string id;
        [JsonProperty(PropertyName = "apiid")]
        protected string apiid;
        [JsonProperty(PropertyName = "errorhandling")]
        public string errorhandling;

        protected Command()
        {
            this.errorhandling = "ignoreerrors";
            this.apiid = "@@fleeandcatch@@";
        }

        public abstract string GetCommand();

        public string Id => id;
        public string ApiId => apiid;
        public string ErrorHandling => errorhandling;
    }

    public class CommandType
    {
        protected string Name;
        protected Type CmdType;

        public enum Type
        {
            Connection
        }

        public CommandType(Type pType)
        {
            this.Name = pType.ToString();
            this.CmdType = pType;
        }
    }
}
