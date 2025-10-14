using System.Collections.Generic;
using UnityEngine;

public class LoadMap : MonoBehaviour
{
    [SerializeField] private List<GameObject> Map = new List<GameObject>();
    [SerializeField] private GameObject TutorialMap;


    void Awake()
    {
        bool ok = false;
        if (SettingManager.Instance._gameSettings._Tutorial)
        {
            Instantiate(TutorialMap, transform.position, Quaternion.identity);
            ok = true;
        }
        else
        {
            Instantiate(Map[Random.Range(0, Map.Count)], transform.position, Quaternion.identity);
            ok = true;
        }
        if (ok)
        {
            gameObject.SetActive(false);
        }
    }
}
