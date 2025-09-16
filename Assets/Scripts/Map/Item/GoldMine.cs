using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GoldMine : Item
{
    [SerializeField] private GameObject[] _goldList = new GameObject[5];
    protected override void Start()
    {
        base.Start();
    }

    public override void farm(PlayerAI _playerAI)
    {
        _stack--;
        if (_value > 0)
        {
            if (_stack <= 0)
            {
                _stack = _maxStack;
                int i = 0;
                do
                {
                    i = Random.Range(0, 5);
                } while (_goldList[i].activeSelf);
                _goldList[i].SetActive(true);
                var script = _goldList[i].GetComponent<PickUp>();
                script._anim.SetBool("Die", true);
                if (_value >= _valueOneDrop)
                {
                    _value -= _valueOneDrop;
                    script.setDropItem(_type, _valueOneDrop, _playerAI);
                }
                else
                {
                    script.setDropItem(_type, _value, _playerAI);
                    _playerAI.target = null;
                    _playerAI._canAction = false;
                    _value = 0;
                    _anim.SetBool("Die", true);
                }
            }
            if (_value <= _maxValue / 3)
            {
                _anim.SetBool("Small", true);
            }
        } 
    }
}
