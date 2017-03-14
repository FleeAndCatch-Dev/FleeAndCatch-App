using System;
using System.Collections.Generic;
using FleeAndCatch.Commands.Models.Devices;
using FleeAndCatch.Commands.Models.Devices.Apps;
using FleeAndCatch.Commands.Models.Devices.Robots;
using FleeAndCatch.Commands.Models.Szenarios;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Commands.Models.Szenarios
{
    public abstract class Szenario
    {
        [JsonProperty("id")]
        protected int id;
        [JsonProperty("type")]
        protected string type;
        [JsonProperty("command")]
        protected string command;
        [JsonProperty("mode")]
        protected string mode;
        [JsonProperty("apps")]
        protected List<App> apps;
        [JsonProperty("robots")]
        protected List<Robot> robots;

        protected Szenario(int pId, string pType, string pCommand, string pMode, List<App> pApps, List<Robot> pRobots)
        {
            this.id = pId;
            this.type = pType;
            this.command = pCommand;
            this.mode = pMode;
            this.apps = pApps;
            this.robots = pRobots;
        }

        [JsonIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [JsonIgnore]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        [JsonIgnore]
        public string Command
        {
            get { return command; }
            set { command = value; }
        }

        [JsonIgnore]
        public string Mode => mode;
        [JsonIgnore]
        public List<App> Apps => apps;
        [JsonIgnore]
        public List<Robot> Robots => robots;
    }

    public enum SzenarioMode
    {
        Single, Multi
    }
}

public class SzenarioJsonConverter : JsonConverter
{
    private Type[] _types;

    public SzenarioJsonConverter()
    {

    }

    public SzenarioJsonConverter(params Type[] types)
    {
        _types = types;
    }

    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Device));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartArray)
        {
            List<Szenario> szenarios = new List<Szenario>();
            var jsonArray = JArray.Load(reader);

            foreach (var t in jsonArray)
            {
                if (t["type"] == null) throw new System.Exception("Devie is not implemented");
                switch (t["type"].ToString())
                {
                    case "Control":
                        szenarios.Add(t.ToObject<Control>());
                        break;
                    case "Synchron":
                        szenarios.Add(t.ToObject<Synchron>());
                        break;
                }
            }

            return szenarios;
        }
        else if(reader.TokenType == JsonToken.StartObject)
        {
            Szenario szenario = null;
            var jsonObject = JObject.Load(reader);

            if (jsonObject["type"] == null) throw new System.Exception("Devie is not implemented");
            switch (jsonObject["type"].ToString())
            {
                case "Control":
                    szenario = jsonObject.ToObject<Control>();
                    break;
                case "Synchron":
                    szenario = jsonObject.ToObject<Synchron>();
                    break;
            }
            return szenario;
        }
        throw new Exception("Not defined JsonToken");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        //refactor
        var t = JToken.FromObject(value);
        if (t.Type != JTokenType.Object)
            t.WriteTo(writer);
        else
        {
            //var szenario = value as Szenario;
            var o = (JObject)t;
            /*if (szenario == null) return;
            if (szenario is Control)
            {
            }*/
            o.WriteTo(writer);
        }
    }
}
