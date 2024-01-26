using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace ElysiaFramework;

public class AppEx : Application
{
    public static IHost? Host;

    public static T GetService<T>()
    {
        var s = Host?.Services.GetService(typeof(T));
        if (s != null)
        {
            return (T)s;
        }

        throw new ArgumentException($"Service {typeof(T)} is null!");
    }
}