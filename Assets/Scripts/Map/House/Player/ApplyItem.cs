using System.Collections;
using UnityEngine;

public class ApplyItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Apply(collision);
    }

    private void Apply(Collider2D collision)
    {
        if (collision == null) return;
        if (PlayerTag.checkTagFarm(collision.tag))
        {
            var _script = collision.GetComponent<PlayerAI>();
            bool _aply = false;
            if (_script._rock > 0 && Castle.Instance._rock < Castle.Instance._maxRock)
            {
                Castle.Instance._rock += _script._rock;
                _script._rock = 0;
                _aply = true;
            }
            if (_script._wood > 0 && Castle.Instance._wood < Castle.Instance._maxWood)
            {
                Castle.Instance._wood += _script._wood;
                _script._wood = 0;
                _aply = true;
            }
            if (_script._gold > 0 && Castle.Instance._gold < Castle.Instance._maxGold)
            {
                Castle.Instance._gold += _script._gold;
                _script._gold = 0;
                _aply = true;
            }
            if (_script._meat > 0 && Castle.Instance._meat < Castle.Instance._maxMeat)
            {
                Castle.Instance._meat += _script._meat;
                _script._meat = 0;
                _aply = true;
            }
            if (_aply)
            {
                _aply = false;
                StartCoroutine(ResetPlayer(_script));
                GameManager.Instance.UIupdateReferences();
            }
        }
    }

    private IEnumerator ResetPlayer(PlayerAI _script)
    {
        yield return new WaitForSeconds(1f);
        _script.setIsAI(true);
        _script.resetItemSelect();
        _script.target = null;
    }
}
