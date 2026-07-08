using System.Collections.Generic;
using UnityEngine;

public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private InventoryManager myInventory;
    [SerializeField] private InventoryUI myUIPanel;

    [System.Serializable]
    public struct StartingItem
    {
        public ItemSO item;
        public int amount;
    }

    [SerializeField] private int inventorySize = 10;
    [SerializeField] private List<StartingItem> startingItems = new List<StartingItem>();

    public InventoryManager Inventory { get; private set; }

    private void Awake()
    {
        Inventory = new InventoryManager(inventorySize);

        foreach(var startItem in startingItems)
        {
            if(startItem.item == null) continue;
            Inventory.AddItem(startItem.item, startItem.amount);
        }
    }

    private void Start()
    {
        myUIPanel.SetupInventoryUI(Inventory);
    }
}
