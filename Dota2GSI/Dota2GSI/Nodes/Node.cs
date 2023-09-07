using System;
using Newtonsoft.Json.Linq;

namespace Dota2GSI.Nodes
{
    public class Node
    {
        protected JObject _ParsedData;

        internal Node(string jsonData)
        {
            if (jsonData.Equals(""))
            {
                jsonData = "{}";
            }
            _ParsedData = JObject.Parse(jsonData);
        }

        internal string GetString(string Name)
        {
            JToken value;
            
            if(_ParsedData.TryGetValue(Name, out value))
                return value.ToString();
            else
                return "";
        }

        internal int GetInt(string Name)
        {
            JToken value;
            
            if(_ParsedData.TryGetValue(Name, out value))
                return Convert.ToInt32(value.ToString());
            else
                return -1;
        }

        internal long GetLong(string Name)
        {
            JToken value;

            if (_ParsedData.TryGetValue(Name, out value))
                return Convert.ToInt64(value.ToString());
            else
                return -1;
        }

        internal T GetEnum<T>(string Name)
        {
            JToken value;
            
            if(_ParsedData.TryGetValue(Name, out value) && !String.IsNullOrWhiteSpace(value.ToString()))
                return (T)Enum.Parse(typeof(T), value.ToString(), true);
            else
                return (T)Enum.Parse(typeof(T), "Undefined", true);
        }

        internal bool GetBool(string Name)
        {
            JToken value;
            
            if(_ParsedData.TryGetValue(Name, out value) && value.ToObject<bool>())
                return value.ToObject<bool>();
            else
                return false;
        }
    }
}
