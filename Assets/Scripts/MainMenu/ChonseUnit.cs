using UnityEngine;

public class ChonseUnit : MonoBehaviour
{
    [SerializeField] private UnitInventory _unit;
    [SerializeField] private InventoryManager _inventory_manager;
    [SerializeField] private LoadInventoryUnit _load;

    public void openInventory()
    {
        _inventory_manager.OpenInventory();
        _inventory_manager.setUnitInventory(_unit);
        _load.setUnit(_unit);
    }
}
