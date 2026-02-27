using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using IncriElemental.Core.Models;

namespace IncriElemental.Core.Persistence;

public class SaveManager
{
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static string Serialize(GameState state)
    {
        return JsonSerializer.Serialize(state, _options);
    }

    public static GameState? Deserialize(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<GameState>(json, _options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deserializing GameState: {ex.Message}");
            return null;
        }
    }

    public static void SaveToFile(GameState state, string filePath)
    {
        string json = Serialize(state);
        File.WriteAllText(filePath, json);
    }

    public static GameState? LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath)) return null;
        string json = File.ReadAllText(filePath);
        return Deserialize(json);
    }
}
