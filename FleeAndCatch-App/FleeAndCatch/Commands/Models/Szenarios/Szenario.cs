﻿using System;
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
        [JsonProperty("steering")]
        private Steering steering;

        protected Szenario(int pId, string pType, string pCommand, string pMode, List<App> pApps, List<Robot> pRobots, Steering pSteering)
        {
            this.id = pId;
            this.type = pType;
            this.command = pCommand;
            this.mode = pMode;
            this.apps = pApps;
            this.robots = pRobots;
            this.steering = pSteering;
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

        [JsonIgnore]
        public Steering Steering
        {
            get { return steering; }
            set { steering = value; }
        }
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
        return (objectType == typeof(Szenario));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartArray)
        {
            List<Szenario> szenarios = new List<Szenario>();
            var jsonArray = JArray.Load(reader);

            foreach (var t in jsonArray)
            {
                if (t["type"] == null) throw new System.Exception("Szenario is not implemented");
                switch (t["type"].ToString())
                {
                    case "Control":
                        szenarios.Add(t.ToObject<Control>());
                        break;
                    case "Synchron":
                        szenarios.Add(t.ToObject<Synchron>());
                        break;
                    case "Follow":
                        szenarios.Add(t.ToObject<Follow>());
                        break;
                    case "Flee":
                        szenarios.Add(t.ToObject<Flee>());
                        break;
                    case "Catch":
                        szenarios.Add(t.ToObject<Catch>());
                        break;
                }
            }

            return szenarios;
        }
        else if(reader.TokenType == JsonToken.StartObject)
        {
            Szenario szenario = null;
            var jsonObject = JObject.Load(reader);

            if (jsonObject["type"] == null) throw new System.Exception("Szenario is not implemented");
            switch (jsonObject["type"].ToString())
            {
                case "Control":
                    szenario = jsonObject.ToObject<Control>();
                    break;
                case "Synchron":
                    szenario = jsonObject.ToObject<Synchron>();
                    break;
                case "Follow":
                    szenario = jsonObject.ToObject<Follow>();
                    break;
                case "Flee":
                    szenario = jsonObject.ToObject<Flee>();
                    break;
                case "Catch":
                    szenario = jsonObject.ToObject<Catch>();
                    break;
            }
            return szenario;
        }
        throw new Exception("Not defined JsonToken");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var t = JToken.FromObject(value);
        if (t.Type != JTokenType.Object)
            t.WriteTo(writer);
        else
        {
            var o = (JObject)t;
            o.WriteTo(writer);
        }
    }
}
