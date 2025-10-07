using System.Collections.Generic;
using UnityEngine;

public class DisplayPlayerDie : MonoBehaviour
{
    [SerializeField] private Transform Pos;
    [SerializeField] private GameObject _diePrefab;

    [SerializeField] private List<PlayerDie> _listPrefab = new List<PlayerDie>();

    public void Add(UnitType type)
    {
        // B1: Dời tất cả prefab hiện có lên trên (nhường chỗ cho cái mới)
        foreach (var die in _listPrefab)
        {
            if (die.gameObject.activeSelf)
                die.UP();
        }

        // B2: Tìm prefab rảnh (không active) để tái sử dụng
        PlayerDie freeObj = _listPrefab.Find(x => !x.gameObject.activeSelf);

        // B3: Nếu không có, tạo mới
        if (freeObj == null)
        {
            GameObject newObj = Instantiate(_diePrefab, Pos);
            freeObj = newObj.GetComponent<PlayerDie>();
            freeObj.getColor();
            _listPrefab.Add(freeObj);
        }

        // B4: Kích hoạt và hiển thị thông tin
        freeObj.gameObject.SetActive(true);
        freeObj.respawn(Pos.position, type);
    }
}
