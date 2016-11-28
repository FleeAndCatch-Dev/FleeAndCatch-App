using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComponentType;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commands
{
    public class Robot
    {
        [JsonProperty("id")]
        private int id;
        [JsonProperty("type")]
        private string type;

        /// <summary>
        /// Create an object of an robot for a json command.
        /// </summary>
        /// <param name="pId">Id of the robot in communication.</param>
        public Robot(int pId)
        {
            this.id = pId;
            this.type = "null";
        }

        /// <summary>
        /// Get a json obejct of the robot.
        /// </summary>
        /// <returns>Json object.</returns>
        public JObject GetRobot()
        {
            var jsonclient = new JObject
            {
                {"id", id},
                { "type", type},
            };
            return jsonclient;
        }

        public int Id => id;
        public string Type => type;
    }
}
