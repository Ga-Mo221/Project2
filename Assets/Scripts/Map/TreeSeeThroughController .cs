using UnityEngine;

public class TreeSeeThroughController : MonoBehaviour
{
    public Transform player;
    public Material treeMat;
    public float radius = 1f;
    public float fade = 0.5f;
    public float seeThroughAlpha = 0.1f; // 0 = trong suốt, 1 = bình thường

    void Start()
    {
        treeMat = Instantiate(treeMat);
        GetComponent<SpriteRenderer>().material = treeMat;
    }

    void Update()
    {
        if (player == null || treeMat == null) return;

        treeMat.SetVector("_PlayerPos", player.position);
        treeMat.SetFloat("_Radius", radius);
        treeMat.SetFloat("_Fade", fade);
        treeMat.SetFloat("_SeeThroughStrength", seeThroughAlpha);
    }
}
