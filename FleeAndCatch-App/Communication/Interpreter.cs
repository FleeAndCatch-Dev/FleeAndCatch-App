using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Data.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Communication
{
    public class Interpreter
    {
        private Client _client;

        /// <summary>
        /// Create an object of the class interpreter.
        /// </summary>
        /// <param name="pClient"></param>
        public Interpreter(Client pClient)
        {
            this._client = pClient;
        }

        /// <summary>
        /// Interpret a command and returns a result as string.
        /// </summary>
        /// <param name="pCommand">Json command</param>
        /// <returns></returns>
        public string Interpret(string pCommand)
        {
           var jsonObject = JObject.Parse(pCommand);
            var apiid = Convert.ToString(jsonObject.SelectToken("apiid"));
            if (apiid == "@@fleeandcatch@@")
            {
                var id = Convert.ToString(jsonObject.SelectToken("id")).ToLower();
                var type = Convert.ToString(jsonObject.SelectToken("type")).ToCharArray();
                var typeCmd = new string(type, 0, 3);

                if (typeCmd == "Get")
                {
                    if (id == "client")
                    {
                        typeCmd = new string(type.Skip(3).Take(type.Length - 3).ToArray()).ToLower();
                        if (typeCmd == "type")
                        {
                            _client.SendCmd("{\"id\":\"Client\",\"type\":\"SetType\",\"apiid\":\"@@fleeandcatch@@\",\"errorhandling\":\"ignoreerrors\",\"client\":{\"id\":" + _client.Id + ",\"type\":\"App\"}}");
                            return null;
                        }
                    }
                }
                else if (typeCmd == "Set")
                {
                    jsonObject = jsonObject.SelectToken(id) as JObject;
                    typeCmd = new string(type.Skip(3).Take(type.Length - 3).ToArray()).ToLower();
                    var result = Convert.ToString(jsonObject.SelectToken(typeCmd));
                    return result;
                }
                else if(new string(type) == "Disconnect")
                {
                    _client.Disconnect();
                    return null;
                }
                throw new Exception("Something is going wrong");
            }
            throw new Exception("Wrong apiid of the command");
        }
    }
}
