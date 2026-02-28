using System.Text.Json;
using System.Text.Json.Serialization;
using IncriElemental.Core.Models;

namespace IncriElemental.Core.Persistence;

public class SaveManager
{
    private const int CurrentVersion = 1;
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static string Serialize(GameState state)
    {
        state.Version = CurrentVersion;
        return JsonSerializer.Serialize(state, _options);
    }

    public static GameState? Deserialize(string json)
    {
        try
        {
            var state = JsonSerializer.Deserialize<GameState>(json, _options);
            if (state != null && state.Version < CurrentVersion)
            {
                state = Migrate(state);
            }
            return state;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deserializing GameState: {ex.Message}");
            return null;
        }
    }

    private static GameState Migrate(GameState state)
    {
        while (state.Version < CurrentVersion)
        {
            switch (state.Version)
            {
                case 0:
                    // Perform migrations from 0 to 1
                    // For example, if a new resource was added, ensure it's in the dictionary.
                    foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
                    {
                        if (!state.Resources.ContainsKey(type)) state.Resources[type] = new Resource(type);
                    }
                    state.Version = 1;
                    break;
                default:
                    state.Version = CurrentVersion;
                    break;
            }
        }
        return state;
    }

    public static void SaveToFile(GameState state, string filePath)
    {
        var json = Serialize(state);
        File.WriteAllText(filePath, json);
    }

    public static GameState? LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath)) return null;
        var json = File.ReadAllText(filePath);
        return Deserialize(json);
    }
}
