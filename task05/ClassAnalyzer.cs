using System;
using System.Reflection;
using System.Collections.Generic;

public class ClassAnalyzer
{
    private Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }
    public IEnumerable<string> GetPublicMethods()
    => _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
    .Select(method => method.Name);

    public IEnumerable<string> GetMethodParams(string methodname)
    {
        var method = _type.GetMethod(methodname, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        if (method == null)
        {
            return Array.Empty<string>();
        }
        var paramNames = method.GetParameters().Select(param => param.Name!);
        return paramNames.Append(method.ReturnType.Name);
    }               

    public IEnumerable<string> GetAllFields()
    => _type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
    .Select(field => field.Name);

    public IEnumerable<string> GetProperties()
    => _type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
    .Select(property => property.Name);

    public bool HasAttribute<T>() where T : Attribute
    => _type.IsDefined(typeof(T), false);
}


