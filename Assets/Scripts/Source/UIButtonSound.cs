using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler, 
    IDeselectHandler
{
    [Header("Sound Clips")]
    private AudioClip hoverClip;
    private AudioClip clickClip;
    private AudioClip valueChangeClip; // Dành cho Slider/Toggle

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float hoverVolume = 1f;
    [Range(0f, 1f)] public float clickVolume = 1f;
    [Range(0f, 1f)] public float valueChangeVolume = 1f;

    [Header("Optional Audio Source (leave empty to auto-create)")]
    private AudioSource audioSource;

    private Button button;
    private Toggle toggle;
    private Slider slider;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        audioSource.spatialBlend = 0f; // 2D sound

        // Tìm các component có thể tương tác
        button = GetComponent<Button>();
        toggle = GetComponent<Toggle>();
        slider = GetComponent<Slider>();

        // Gắn event listener động cho toggle và slider
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        if (slider != null)
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    void Start()
    {
        hoverClip = SettingManager.Instance._button_hover;
        clickClip = SettingManager.Instance._button_click;
        valueChangeClip = SettingManager.Instance._valuchangeClip;
    }

    private void OnDestroy()
    {
        // Dọn listener khi destroy
        if (toggle != null)
            toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySFX(hoverClip, hoverVolume);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySFX(clickClip, clickVolume);
    }

    public void OnSelect(BaseEventData eventData)
    {
        PlaySFX(hoverClip, hoverVolume);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // Có thể thêm âm thanh rời chọn nếu muốn
    }

    private void OnToggleValueChanged(bool value)
    {
        PlaySFX(valueChangeClip != null ? valueChangeClip : clickClip, valueChangeVolume);
    }

    private void OnSliderValueChanged(float value)
    {
        PlaySFX(valueChangeClip != null ? valueChangeClip : clickClip, valueChangeVolume);
    }

    private void PlaySFX(AudioClip clip, float volume)
    {
        if (clip == null) return;

        // Lấy volume hệ thống
        Debug.Log(1);
        float overall = SettingManager.Instance._gameSettings._overall_Volume;
        float sfx = SettingManager.Instance._gameSettings._SFX_volume;

        audioSource.PlayOneShot(clip, volume * sfx * overall);
    }
}
