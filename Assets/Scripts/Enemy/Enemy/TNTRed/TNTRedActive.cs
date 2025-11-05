using UnityEngine;

public class TNTRedActive : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    public void setActive()
    {
        _enemyAI.setDie(true);
        transform.parent.gameObject.SetActive(false);
    }

    public void PlayerDieSoundEnemy() => _enemyAI.playDieSound();
}
