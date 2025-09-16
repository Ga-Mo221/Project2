using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private ItemType _type;
    [SerializeField] private int _value;
    [SerializeField] private PlayerAI _script;
    public Animator _anim;

    public void setDropItem(ItemType type, int value, PlayerAI playerAI)
    {
        _type = type;
        _value = value;
        _script = playerAI;
    }

    public void addScore()
    {
        if (_type == ItemType.Tree)
        {
            if (_script._wood < _script._maxWood)
            {
                _script._wood += _value;
                if (_script._wood > _script._maxWood)
                {
                    _script._wood = _script._maxWood;
                    _script._canAction = false;
                }
                _script.resetItemSelect();
                _script.target = null;
                gameObject.SetActive(false);
            }
        }
        if (_type == ItemType.Rock)
        {
            if (_script._rock < _script._maxRock)
            {
                _script._rock += _value;
                if (_script._rock > _script._maxRock)
                {
                    _script._rock = _script._maxRock;
                    _script._canAction = false;
                }
                _script.resetItemSelect();
                _script.target = null;
                gameObject.SetActive(false);
            }
        }
        if (_type == ItemType.Gold)
        {
            if (_script._gold < _script._maxGold)
            {
                _script._gold += _value;
                if (_script._gold > _script._maxGold)
                {
                    _script._gold = _script._maxGold;
                    _script._canAction = false;
                }
                _script.resetItemSelect();
                _script.target = null;
                _anim.SetBool("Die", false);
                gameObject.SetActive(false);
            }
        }
    }
}
