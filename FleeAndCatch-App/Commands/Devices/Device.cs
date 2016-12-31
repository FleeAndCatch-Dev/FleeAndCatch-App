using Newtonsoft.Json.Linq;

namespace Commands.Devices
{
    public interface Device
    {
        JObject GetJObject();
    }
}
