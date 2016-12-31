using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commands
{
    public class Steering
    {
        [JsonProperty("direction")]
        private DirectionType direction;
        [JsonProperty("speed")]
        private SpeedType speed;

        public Steering(int pDirection, int pSpeed)
        {
            this.direction = (DirectionType) pDirection;
            this.speed = (SpeedType) pSpeed;
        }

        public JObject GetJObject()
        {
            var jsonIdentification = new JObject
            {
                {"direction", (int)direction},
                {"speed", (int)speed}
            };
            return jsonIdentification;
        }

        public DirectionType _Directiond => direction;
        public SpeedType _Speed => speed;
    }

    public enum SpeedType
    {
        Slower = -1, Equal = 0, Faster = 1
    }

    public enum DirectionType
    {
        Left = -1, Equal = 0, Right = 1
    }

}