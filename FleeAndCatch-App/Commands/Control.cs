using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commands
{
    public class Control : Command
    {
        [JsonProperty("steering")]
        private Steering steering;

        public Control(string pId, string pType, Identification pIdentification) : base(pId, pType, pIdentification)
        {
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
                {"control",  steering.GetJObject()}
            };

            return JsonConvert.SerializeObject(command);
        }
    }

    public enum ControlType
    {
    }
}
