using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commands
{
    public class Synchronisation : Command
    {
        [JsonProperty("identification")]
        private Identification identification;
        [JsonProperty("robots")]
        private List<Robot> robots;

        /// <summary>
        /// Create an object of the synchronisation command.
        /// </summary>
        /// <param name="pId">Id as command type.</param>
        /// <param name="pType">Type as synchronisation type.</param>
        /// <param name="pClient">Client of represeenting the device.</param>
        public Synchronisation(string pId, string pType, Identification pIdentification, List<Robot> pRobots) : base(pId, pType)
        {
            identification = pIdentification;
            robots = pRobots;
        }

        /// <summary>
        /// Get json string of the command.
        /// </summary>
        /// <returns>Json string.</returns>
        public override string GetCommand()
        {
            var array = new JArray();
            foreach (var t in robots)
                array.Add(t.GetJObject());

            var command = new JObject
            {
                {"id", id},
                {"type", type},
                {"apiid", apiid},
                {"errorhandling", errorhandling},
                {"identification", identification.GetJObject()},
                {"robots", array}
            };

            return JsonConvert.SerializeObject(command);
        }

        public Identification Identification => identification;
        public List<Robot> Robots => robots;
    }

    public class SynchronisationType : CommandType
    {
        /// <summary>
        /// Enumeration of synchronisation type.
        /// </summary>
        public new enum Type
        {
            GetRobots, SetRobots
        }

        /// <summary>
        /// Create an object of the synchronisation command.
        /// </summary>
        /// <param name="pType">Type as synchronisation type.</param>
        public SynchronisationType(Type pType) : base((CommandType.Type) pType)
        {
        }
    }
}
