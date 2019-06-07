using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace PostToPhp
{
    // Token: 0x02000002 RID: 2
    public class JsonClass
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public static string JSONSerialize<T>(T obj)
        {
            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(obj.GetType());
            string @string;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                dataContractJsonSerializer.WriteObject(memoryStream, obj);
                @string = Encoding.Default.GetString(memoryStream.ToArray());
            }
            return @string;
        }
    }
}
