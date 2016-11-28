using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commands
{
    public class Synchronisation : Command
    {
        [JsonProperty("client")]
        private Commands.Client client;
        [JsonProperty("robots")]
        private List<Commands.Robot> robots;

        /// <summary>
        /// Create an object of the synchronisation command.
        /// </summary>
        /// <param name="pId">Id as command type.</param>
        /// <param name="pType">Type as synchronisation type.</param>
        /// <param name="pClient">Client of represeenting the device.</param>
        public Synchronisation(string pId, string pType, Commands.Client pClient) : base(pId, pType)
        {
            this.client = pClient;
            this.robots = new List<Robot>();
        }

        /// <summary>
        /// Get json string of the command.
        /// </summary>
        /// <returns>Json string.</returns>
        public override string GetCommand()
        {
            var array = new JArray();
            for (var i = 0; i < robots.Count; i++)
            {
                var robot = new JObject()
                {
                    {"id", id},
                    {"type", type}
                };
                array.Add(robot);
            }

            var command = new JObject
            {
                {"id", id},
                {"type", type},
                {"apiid", apiid},
                {"errorhandling", errorhandling},
                {"client", client.GetClient()},
                {"robots", array}
            };

            return JsonConvert.SerializeObject(command);
        }

        public Client Client => client;
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
