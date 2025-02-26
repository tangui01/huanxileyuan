using BestHTTP.SignalRCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using BestHTTP.PlatformSupport.Memory;
using System.IO;
using UnityEngine;

public class NewtonEncoder : IEncoder
{
    public object ConvertTo(Type toType, object obj)
    {
        
        //string json = JsonConvert.SerializeObject(obj);
        string json = JsonUtility.ToJson(obj);// JsonConvert.SerializeObject(obj);

        //return JsonConvert.DeserializeObject(json, toType);
        return JsonUtility.FromJson(json, toType); 
    }

    public T DecodeAs<T>(BufferSegment buffer)
    {
        using(var reader = new StreamReader(new MemoryStream(buffer.Data, buffer.Offset, buffer.Count)))
        using(var jsonReader = new JsonTextReader(reader))
            return new JsonSerializer().Deserialize<T>(jsonReader);
    }

    public BufferSegment Encode<T>(T value)
    {
        var json =  JsonConvert.SerializeObject(value);

        int len = Encoding.UTF8.GetByteCount(json);
        byte[] buffer = BufferPool.Get(len + 1, true);
        Encoding.UTF8.GetBytes(json, 0, json.Length, buffer, 0);

        buffer[len] = 0x1e;

        return new BufferSegment(buffer, 0, len + 1);
    }
}
