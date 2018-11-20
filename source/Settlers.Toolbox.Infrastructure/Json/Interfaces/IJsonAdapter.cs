namespace Settlers.Toolbox.Infrastructure.Json.Interfaces
{
    /// <summary>
    /// Defines methods to deserialize JSON objects.
    /// </summary>
    public interface IJsonAdapter
    {
        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="json">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        T DeserializeObject<T>(string json);
    }
}