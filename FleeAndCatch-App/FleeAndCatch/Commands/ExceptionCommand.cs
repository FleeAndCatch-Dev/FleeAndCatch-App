using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands
{
    public class ExceptionCommand : Command
    {
        [JsonProperty("exception")]
        private Models.Exception exception;

        public ExceptionCommand(string pId, string pType, ClientIdentification pIdentification, Models.Exception pException) : base(pId, pType, pIdentification)
        {
            this.exception = pException;
        }

        public override string GetCommand()
        {
            var command = new JObject
            {
                {"id", id},
                {"type", type},
                {"apiid", apiid},
                {"errorhandling", errorhandling},
                {"identification", identification.GetJObject()},
                {"exception", exception.GetJObject()}
            };

            return JsonConvert.SerializeObject(command);
        }

        public Models.Exception Exception => exception;
    }

    public enum ExceptionCommandType
    {
        Undefined, UnhandeldDisconnection
    }
}
