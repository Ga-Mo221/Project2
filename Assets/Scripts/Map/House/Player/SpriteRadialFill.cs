using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRadialFill : MonoBehaviour
{
    [Range(0f, 1f)] public float fillAmount = 1f;
    public bool clockwise = true;
    private Material mat;

    void Start() => mat = GetComponent<SpriteRenderer>().material;

    void Update()
    {
        mat.SetFloat("_FillAmount", fillAmount);
        mat.SetFloat("_Clockwise", clockwise ? 1 : 0);
    }
}
