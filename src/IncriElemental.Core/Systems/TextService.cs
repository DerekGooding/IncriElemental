using System.Text.Json;

namespace IncriElemental.Core.Systems;

public class TextService
{
    private Dictionary<string, string> _strings = [];
    private static TextService? _instance;
    public static TextService Instance => _instance ??= new TextService();

    private TextService() { }

    public void LoadStrings(string json)
    {
        _strings = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? [];
    }

    public string Get(string key, params object[] args)
    {
        if (!_strings.TryGetValue(key, out var value))
        {
            return $"[{key}]";
        }

        if (args.Length > 0)
        {
            return string.Format(value, args);
        }

        return value;
    }

    public bool Has(string key) => _strings.ContainsKey(key);
}
