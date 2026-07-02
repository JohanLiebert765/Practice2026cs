using System;
using System.Reflection;

namespace task07
{
    public static class ReflectionHelper
    {
        public static void PrintTypeInfo(Type type)
        {
            var displayname = type.GetCustomAttribute<DisplayNameAttribute>();
            if(displayname != null)
            {
                Console.WriteLine($"Класс: {displayname.DisplayName}");
            }
            else
            {
                Console.WriteLine($"Класс: {type.Name}");
            }
            var version = type.GetCustomAttribute<VersionAttribute>();
            if (version != null)
            {
                Console.WriteLine($"Версия: {version.Major}.{version.Minor}");
            }
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                var MethodDisplayName = method.GetCustomAttribute<DisplayNameAttribute>();
                if (MethodDisplayName != null)
                {
                    Console.WriteLine($"Метод: {method.Name} ({MethodDisplayName.DisplayName})");
                }
            }
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach(var property in properties)
            {
                var PropertyDisplayName = property.GetCustomAttribute<DisplayNameAttribute>();
                if(PropertyDisplayName != null)
                {
                    Console.WriteLine($"Свойство: {property.Name} ({PropertyDisplayName.DisplayName})");
                }
            }
        }
    }
}

