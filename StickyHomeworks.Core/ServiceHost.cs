using Microsoft.Extensions.Hosting;

namespace StickyHomeworks.Core;

public static class ServiceHost
{
    public static IHost? Host { get; set; }

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