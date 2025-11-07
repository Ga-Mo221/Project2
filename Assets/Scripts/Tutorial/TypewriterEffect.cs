using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private string fullText;
    [SerializeField] private float typeSpeed = 0.05f; // Thời gian giữa mỗi ký tự (giây)
    [SerializeField] private UnitAudio _audio;

    public float getSpeed() => typeSpeed;

    public IEnumerator TypeText(string content, System.Func<bool> skipCheck)
    {
        textUI.text = "";
        _audio.PlaySunSound();
        Debug.Log("doc");
        foreach (char c in content)
        {
            // Nếu người chơi nhấn skip => hiện toàn bộ text
            if (skipCheck())
            {
                textUI.text = content;
                _audio.StopWeatherSound();
                yield break;
            }

            textUI.text += c;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }
        _audio.StopWeatherSound();
    }
}
