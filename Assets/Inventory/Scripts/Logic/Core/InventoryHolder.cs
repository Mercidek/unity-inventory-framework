using System.Collections.Generic;
using UnityEngine;

public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private ItemDatabaseSO itemDatabase;
    [SerializeField] private string saveFileName;
    [SerializeField] private GameObject inventoryCanvasPrefab;

    private InventoryManager myInventory;
    private GameObject myCanvas;
    private InventoryUI myUIPanel;
    private bool uiCreated = false;

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
                myInventory = new InventoryManager(saveFileName);
                myInventory.initializeSlots(inventorySize);
                myInventory.SetOwner(gameObject);
            }
            return myInventory;
        }
        private set => myInventory = value;
    }

    private void Awake()
    {
        if(myCanvas == null) SpawnInventoryCanvas();
    }
    private void Start()
    {
        // If this inventory has a save file then load it, otherwise load with the starting items
        if(Inventory.HasSaveFile())
        {
            Inventory.LoadInventory(itemDatabase);
        }
        else
        {
            Inventory.EmptyInventory();

            foreach (var startItem in startingItems)
            {
                if (startItem.item == null) continue;
                Inventory.AddItem(startItem.item, startItem.amount);
            }
        }

        if(uiCreated) myUIPanel.SetupInventoryUI(Inventory);
    }

    private void OnDestroy()
    {
        if(myCanvas != null) Destroy(myCanvas);
    }

    private void SpawnInventoryCanvas()
    {
        if(inventoryCanvasPrefab == null) return;

        myCanvas = Instantiate(inventoryCanvasPrefab);
        myCanvas.name = $"{gameObject.name}{inventoryCanvasPrefab.name}";

        myUIPanel = myCanvas.GetComponentInChildren<InventoryUI>();
        if(myUIPanel != null ) uiCreated = true;
    }
}
