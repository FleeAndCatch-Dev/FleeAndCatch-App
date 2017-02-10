using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models;
using FleeAndCatch.Commands.Models.Szenarios;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands
{
    public class SzenarioCommand : Command
    {
        [JsonProperty("szenario")]
        private Szenario szenario;

        public SzenarioCommand(string pId, string pType, ClientIdentification pIdentification, Szenario pSzenario) : base(pId, pType, pIdentification)
        {
            this.szenario = pSzenario;
        }

        [JsonIgnore]
        public Szenario Szenario => szenario;
    }

    public enum SzenarioCommandType
    {
        Control, Synchron, Follow
    }
}
