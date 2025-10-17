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
            if (_script.getUpTower()) return;
            bool _aply = false;
            if (_script._rock > 0 && CastleManager.Instance.Castle._rock < CastleManager.Instance.Castle._maxRock)
            {
                CastleManager.Instance.Castle._rock += _script._rock;
                GameManager.Instance._rock += _script._rock;
                _script._rock = 0;
                _aply = true;
            }
            if (_script._wood > 0 && CastleManager.Instance.Castle._wood < CastleManager.Instance.Castle._maxWood)
            {
                CastleManager.Instance.Castle._wood += _script._wood;
                GameManager.Instance._wood += _script._wood;
                _script._wood = 0;
                _aply = true;
            }
            if (_script._gold > 0 && CastleManager.Instance.Castle._gold < CastleManager.Instance.Castle._maxGold)
            {
                CastleManager.Instance.Castle._gold += _script._gold;
                GameManager.Instance._gold += _script._gold;
                _script._gold = 0;
                _aply = true;
            }
            if (_script._meat > 0 && CastleManager.Instance.Castle._meat < CastleManager.Instance.Castle._maxMeat)
            {
                CastleManager.Instance.Castle._meat += _script._meat;
                GameManager.Instance._meat += _script._meat;
                _script._meat = 0;
                _aply = true;
            }
            if (_aply)
            {
                _aply = false;
                StartCoroutine(ResetPlayer(_script));
                GameManager.Instance.UIupdateReferences();
                GameManager.Instance.UIupdateCreateUnitButton();
                GameManager.Instance.UIupdateInfoUpgrade();
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
