using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models;
using FleeAndCatch.Commands.Models.Devices.Robots;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands
{
    public class Synchronization : Command
    {
        [JsonProperty("robots")]
        private List<Robot> robots;

        /// <summary>
        /// Create an object of the synchronisation command.
        /// </summary>
        /// <param name="pId">Id as command type.</param>
        /// <param name="pType">Type as synchronisation type.</param>
        /// <param name="pClient">Client of represeenting the device.</param>
        public Synchronization(string pId, string pType, ClientIdentification pIdentification, List<Robot> pRobots) : base(pId, pType, pIdentification)
        {
            robots = pRobots;
        }

        [JsonIgnore]
        public List<Robot> Robots => robots;
    }

    public enum SynchronizationCommandType
    {
        All, Current
    }
}
