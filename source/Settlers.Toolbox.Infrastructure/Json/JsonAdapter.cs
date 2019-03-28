using System;

using Newtonsoft.Json;

using Settlers.Toolbox.Infrastructure.Json.Interfaces;

namespace Settlers.Toolbox.Infrastructure.Json
{
    public class JsonAdapter : IJsonAdapter
    {
        public T DeserializeObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json)) throw new ArgumentNullException(nameof(json));

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}