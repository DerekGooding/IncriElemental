using System;

namespace IncriElemental.Core.Engine;

public static class EventBus
{
    public static event Action<string, double>? ResourceGained;
    public static event Action<string>? DiscoveryUnlocked;
    public static event Action<string>? ThingManifested;

    public static void PublishResourceGained(string type, double amount) => ResourceGained?.Invoke(type, amount);
    public static void PublishDiscoveryUnlocked(string key) => DiscoveryUnlocked?.Invoke(key);
    public static void PublishThingManifested(string id) => ThingManifested?.Invoke(id);
}
