using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoreAspShop.Extensions
{
    public static class SessionExtension
    {
        // Добавляем динамически типизированный SET метод
        public static void Set<T>(this ISession session, string key, T value) 
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Добавляем динамически типизированный GET метод
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value is null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
