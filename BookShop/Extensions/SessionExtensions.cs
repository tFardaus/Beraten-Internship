using System.Text.Json;

namespace BookShop.Extensions
{
    /// <summary>
    /// Extension methods to store/retrieve complex objects in Session
    /// Session natively only supports string/int/byte[]
    /// These methods serialize objects to JSON for storage
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Store any object in session by converting to JSON
        /// </summary>
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        /// <summary>
        /// Retrieve object from session by deserializing JSON
        /// Returns null if key doesn't exist
        /// </summary>
        public static T? GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
