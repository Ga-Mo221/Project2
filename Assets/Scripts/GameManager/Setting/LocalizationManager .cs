using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    public string defaultLanguage = "vi";
    public LocalizationFile currentLocalization;
    private Dictionary<string, string> dict = new Dictionary<string, string>();

    public event Action OnLanguageChanged;

    const string PLAYERPREFS_KEY = "game_language";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        string saved = PlayerPrefs.GetString(PLAYERPREFS_KEY, defaultLanguage);
        LoadLanguage(saved);
    }

    void Start()
    {
        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.defaultLanguage = SettingManager.Instance._gameSettings._language;
        string saved = PlayerPrefs.GetString(PLAYERPREFS_KEY, defaultLanguage);
        LoadLanguage(saved);
    }

    // Load from Resources/Localization/<langCode>.json
    public bool LoadLanguage(string langCode)
    {
        if (string.IsNullOrEmpty(langCode)) langCode = defaultLanguage;
        TextAsset asset = Resources.Load<TextAsset>($"Localization/{langCode}");
        if (asset == null)
        {
            Debug.LogWarning($"[Localization] File not found: Localization/{langCode}.json");
            return false;
        }

        try
        {
            // Unity's JsonUtility doesn't parse Dictionary directly; use MiniJSON-like or simple wrapper.
            // We'll use JsonUtility to parse into wrapper with array, or use SimpleJSON.
            // For simplicity here, use JsonUtility with a small helper below to parse strings as array of pairs.
            currentLocalization = JsonUtility.FromJson<LocalizationFile>(asset.text);

            // If JsonUtility can't parse Dictionary directly, parse manually:
            // Attempt naive parse with MiniJson (you can replace with your favorite JSON lib)
            dict = ParseToDictionary(asset.text);

            PlayerPrefs.SetString(PLAYERPREFS_KEY, langCode);
            PlayerPrefs.Save();

            OnLanguageChanged?.Invoke();
            Debug.Log($"[Localization] Loaded language: {langCode}");
            defaultLanguage = langCode;
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Localization] Failed parse: {ex.Message}\n{asset.text}");
            return false;
        }
    }

    // Get localized string with fallback to key
    public string Get(string key, params object[] args)
    {
        if (string.IsNullOrEmpty(key)) return "";
        if (dict != null && dict.TryGetValue(key, out string value))
        {
            if (args != null && args.Length > 0)
                return string.Format(value, args);
            return value;
        }
        // fallback: if not found, return key to help debug
        return $"[{key}]";
    }

    // Simple dictionary parser using UnityEngine.JsonUtility limitations.
    // This is a small helper to parse a JSON of structure { "strings": { "k":"v", ... } }
    private Dictionary<string, string> ParseToDictionary(string json)
    {
        var result = new Dictionary<string, string>();

        // crude but effective: find the "strings" object and parse pairs
        int idx = json.IndexOf("\"strings\"");
        if (idx < 0) return result;
        int start = json.IndexOf('{', idx);
        if (start < 0) return result;

        // find matching closing brace for strings object
        int depth = 0;
        int end = -1;
        for (int i = start; i < json.Length; i++)
        {
            if (json[i] == '{') depth++;
            else if (json[i] == '}')
            {
                depth--;
                if (depth == 0)
                {
                    end = i;
                    break;
                }
            }
        }
        if (end < 0) return result;

        string obj = json.Substring(start + 1, end - start - 1).Trim(); // inside strings { ... }

        // naive split by commas that are not inside quotes - safer to scan char by char
        int pos = 0;
        while (pos < obj.Length)
        {
            // skip whitespace
            while (pos < obj.Length && char.IsWhiteSpace(obj[pos])) pos++;
            if (pos >= obj.Length) break;

            // read key "..."
            if (obj[pos] != '"') break;
            pos++;
            int keyStart = pos;
            while (pos < obj.Length && obj[pos] != '"') pos++;
            string key = obj.Substring(keyStart, pos - keyStart);
            pos++; // skip closing quote
            // skip spaces and colon
            while (pos < obj.Length && (char.IsWhiteSpace(obj[pos]) || obj[pos] == ':')) pos++;

            // value can be "..." or contains escaped quotes
            if (pos >= obj.Length || obj[pos] != '"') break;
            pos++;
            //int valStart = pos;
            bool escaped = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            while (pos < obj.Length)
            {
                char c = obj[pos++];
                if (c == '\\' && !escaped) { escaped = true; continue; }
                if (c == '"' && !escaped) break;
                sb.Append(c);
                escaped = false;
            }
            string value = sb.ToString();
            result[key] = value;

            // move to next comma
            while (pos < obj.Length && obj[pos] != ',') pos++;
            if (pos < obj.Length && obj[pos] == ',') pos++;
        }

        return result;
    }
}

[Serializable]
public class LocalizationFile
{
    public string languageCode;
    public string languageName;
    public Dictionary<string, string> strings;
}
