using TMPro;
using UnityEngine;

public class Select5Unit_Image : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _warrior;
    [SerializeField] private TextMeshProUGUI _Archer;
    [SerializeField] private TextMeshProUGUI _Lancer;
    [SerializeField] private TextMeshProUGUI _Healer;
    [SerializeField] private TextMeshProUGUI _TNT;



    public void onWarrior()
    {
        _warrior.gameObject.SetActive(true);
    }

    public void onArcher()
    {
        _Archer.gameObject.SetActive(true);
    }

    public void onLancer()
    {
        _Lancer.gameObject.SetActive(true);
    }

    public void onHealer()
    {
        _Healer.gameObject.SetActive(true);
    }

    public void onTNT()
    {
        _TNT.gameObject.SetActive(true);
    }


    public void offWarrior()
    {
        _warrior.gameObject.SetActive(false);
    }

    public void offArcher()
    {
        _Archer.gameObject.SetActive(false);
    }

    public void offLancer()
    {
        _Lancer.gameObject.SetActive(false);
    }

    public void offHealer()
    {
        _Healer.gameObject.SetActive(false);
    }

    public void offTNT()
    {
        _TNT.gameObject.SetActive(false);
    }
}
