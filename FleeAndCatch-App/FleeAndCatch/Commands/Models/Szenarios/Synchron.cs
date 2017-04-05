using System.Collections.Generic;
using FleeAndCatch.Commands.Models.Devices.Apps;
using FleeAndCatch.Commands.Models.Devices.Robots;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands.Models.Szenarios
{
    public class Synchron : Szenario
    {
        public Synchron(int pId, string pType, string pCommand, string pMode, List<App> pApps, List<Robot> pRobots, Steering pSteering) : base(pId, pType, pCommand, pMode, pApps, pRobots, pSteering)
        {
        }
    }

    public enum SynchronType
    {
        Undefinied, Begin, Start, Stop, Control
    }
}
