using System.IO;
using UnityEngine;

public class Sheep : AnimalAI
{
    [SerializeField] private SpriteRenderer _spriteRender;

    protected override void Start()
    {
        base.Start();
        if (!_spriteRender)
        {
            Debug.LogError("[Sheep] chua gan 'spriterender'");
        }
    }

    protected override void Update()
    {
        base.Update();
        _spriteRender.sortingOrder = -(int)(transform.position.y * 100);

        GameObject player = GameObject.FindGameObjectWithTag("Player"); 
        if (player != null)
        {
           
            float dist = Vector2.Distance(transform.position, player.transform.position);
            if (dist < 5f) // 5f là khoảng phát hiện player
            {
                 Debug.Log("da phat hien player");
                FleeFrom(player); // gọi hàm trong AnimalAI
            }
        }
    }
}
