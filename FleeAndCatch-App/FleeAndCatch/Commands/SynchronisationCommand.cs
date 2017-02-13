using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models;
using FleeAndCatch.Commands.Models.Devices.Robots;
using FleeAndCatch.Commands.Models.Szenarios;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands
{
    public class Synchronization : Command
    {
        [JsonProperty("szenarios")]
        [JsonConverter(typeof(SzenarioJsonConverter))]
        private List<Szenario> szenarios;
        [JsonProperty("robots")]
        private List<Robot> robots;

        /// <summary>
        /// Create an object of the synchronisation command.
        /// </summary>
        /// <param name="pId">Id as command type.</param>
        /// <param name="pType">Type as synchronisation type.</param>
        /// <param name="pClient">Client of represeenting the device.</param>
        public Synchronization(string pId, string pType, ClientIdentification pIdentification, List<Szenario> pSzenarios , List<Robot> pRobots) : base(pId, pType, pIdentification)
        {
            szenarios = pSzenarios;
            robots = pRobots;
        }

        [JsonIgnore]
        public List<Szenario> Szenarios => szenarios;
        [JsonIgnore]
        public List<Robot> Robots => robots;
    }

    public enum SynchronizationCommandType
    {
        AllRobots, CurrentRobot, AllSzenarios, CurrentSzenario
    }
}
