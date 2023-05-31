﻿
namespace FinalProject.Extensions
{
    public static class SessionExtensions
    {
        public static void SetBool(this ISession session, string key, bool value)
        {
            session.SetInt32(key, value ? 1 : 0);
        }

        public static bool? GetBool(this ISession session, string key)
        {
            int? value = session.GetInt32(key);
            return value.HasValue ? (value == 1) : (bool?)null;
        }
    }

}
