#nullable enable

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dota2GSI.Json.Converters;

public abstract class BaseDotaConverter<TNode> : JsonConverter<TNode>
    where TNode : class
{    
    public override void WriteJson(JsonWriter writer, TNode? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override bool CanWrite => false;

    public override bool CanRead => true;

    protected static TNode? GetItem(string key, JToken rootJToken, JsonSerializer serializer)
    {
        var item = rootJToken[key]?.ToObject<TNode>(serializer);

        return item;
    }
}

public abstract class BaseDotaConverter<TRoot, TNode> : JsonConverter<TRoot>
    where TRoot : class
    where TNode : class
{
    public override void WriteJson(JsonWriter writer, TRoot? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override bool CanWrite => false;

    public override bool CanRead => true;

    protected JToken? GetRootToken(string key, JsonReader reader)
    {
        return JObject.Load(reader)[key];
    }

    protected static IEnumerable<TNode> GetItems(string prefix, JsonSerializer serializer, JObject rootJObject)
    {
        var items = new List<TNode>();
        var i = 0;
        var item = GetItem($"{prefix}{i}", rootJObject, serializer);

        while (item is not null)
        {
            items.Add(item);

            i++;
            item = GetItem($"{prefix}{i}", rootJObject, serializer);
        }

        return items;
    }

    protected static TNode? GetItem(string key, JObject rootJObject, JsonSerializer serializer)
    {
        var item = rootJObject[key]?.ToObject<TNode>(serializer);

        return item;
    }
}