using UnityEngine;

public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private InventoryManager myInventory;
    [SerializeField] private InventoryUI myUIPanel;

    private void Start()
    {
        myUIPanel.SetupInventoryUI(myInventory);
    }
}
