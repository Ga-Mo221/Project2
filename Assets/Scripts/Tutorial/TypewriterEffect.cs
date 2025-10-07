using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private string fullText;
    [SerializeField] private float typeSpeed = 0.05f; // Thời gian giữa mỗi ký tự (giây)

    private Coroutine typingCoroutine;

    public void StartTyping(string text)
    {
        fullText = text;
        if (!gameObject.activeInHierarchy) return;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    public float getSpeed() => typeSpeed;

    private IEnumerator TypeText()
    {
        textMeshPro.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            textMeshPro.text += fullText[i];
            yield return new WaitForSecondsRealtime(typeSpeed); // ✅ Dùng Realtime để hoạt động khi Time.timeScale = 0
        }
    }
}
