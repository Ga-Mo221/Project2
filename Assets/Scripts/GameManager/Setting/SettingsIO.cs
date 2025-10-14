using System.IO;
using UnityEngine;

public static class SettingsIO
{
    private static string FilePath => Path.Combine(Application.persistentDataPath, "game_settings.json");

    public static void Save(GameSettings settings)
    {
        try
        {
            string json = JsonUtility.ToJson(settings, true);
            File.WriteAllText(FilePath, json);
            Debug.Log($"[SettingsIO] Saved settings to: {FilePath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[SettingsIO] Failed to save settings: {ex.Message}");
        }
    }

    public static GameSettings Load()
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                Debug.Log("[SettingsIO] No existing settings file found. Using default settings.");
                GameSettings defaultSettings = GameSettings.Default();

                // 🔹 Tự động lưu file mặc định để tạo file mới
                Save(defaultSettings);

                return defaultSettings;
            }

            string json = File.ReadAllText(FilePath);
            GameSettings settings = JsonUtility.FromJson<GameSettings>(json);
            return settings;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[SettingsIO] Failed to load settings: {ex.Message}");
            return GameSettings.Default();
        }
    }
}
