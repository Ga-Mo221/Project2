using UnityEngine;
using TMPro;

[RequireComponent(typeof(Component))]
public class LocalizedText : MonoBehaviour
{
    public string key;

    TextMeshProUGUI tmpText;

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        ApplyText();
        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.OnLanguageChanged += ApplyText;

        if (SettingManager.Instance != null)
            SettingManager.Instance.addText(this);
    }

    void OnDestroy()
    {
        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.OnLanguageChanged -= ApplyText;
    }

    public void ApplyText()
    {
        string txt = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get(key) : $"[{key}]";
        tmpText.text = txt;
    }
}
