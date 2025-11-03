using UnityEngine;

public class GoldMine : Item
{
    [SerializeField] private GameObject[] _goldList = new GameObject[5];
    [SerializeField] private bool _small = false;
    [SerializeField] private bool _die = false;

    protected override void Start()
    {
        base.Start();
        AstarPath.active.Scan();
    }

    public override void farm(PlayerAI _playerAI)
    {
        _audio.PlayFarmOrHitDamageSound();
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
                var script = _goldList[i].GetComponent<PickUp>();
                _goldList[i].SetActive(true);
                script._anim.SetBool("Die", true);
                if (_value >= _valueOneDrop)
                {
                    _value -= _valueOneDrop;
                    script.setDropItem(_type, _valueOneDrop, _playerAI);

                    if (_value <= 0)
                    {
                        _playerAI.target = null;
                        _playerAI._canAction = false;
                        _value = 0;
                        for (int j = _Farmlist.Count - 1; j >= 0; j--)
                        {
                            var hit = _Farmlist[j];
                            hit.resetItemSelect();
                            hit.target = null;
                        }
                        _die = true;
                        SetDie(_die);
                        _audio.PlayDieSound();
                    }
                }
            }
            if (_value <= _maxValue / 3)
            {
                _small = true;
                SmallGold(_small);
            }
        }
    }

    void OnEnable()
    {
        SmallGold(_small);
        SetDie(_die);
    }

    private void SmallGold(bool anount)
    {
        _anim.SetBool("Small", anount);
    }
    private void SetDie(bool amount)
    {
        _anim.SetBool("Die", amount);
    }
}
