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

    public InventoryManager Inventory 
    {
        get
        {
            if(myInventory == null)
            {
                myInventory = new InventoryManager();
                myInventory.initializeSlots(inventorySize);
                myInventory.SetOwner(gameObject);
            }
            return myInventory;
        }
        private set => myInventory = value;
    }

    private void Start()
    {
        foreach (var startItem in startingItems)
        {
            if (startItem.item == null) continue;
            Inventory.AddItem(startItem.item, startItem.amount);
        }

        myUIPanel.SetupInventoryUI(Inventory);
    }
}
