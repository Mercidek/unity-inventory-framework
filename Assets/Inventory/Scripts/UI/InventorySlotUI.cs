using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image slotIcon;
    [SerializeField] private TextMeshProUGUI slotAmountText;

    public void UpdateSlotUI(ItemSO item, int amount)
    {
        if(item != null)
        {
            if(slotIcon != null)
            {
                slotIcon.sprite = item.ItemIcon;
                slotIcon.enabled = true;
            }
            else
            {
                slotIcon.enabled = false;
            }
            slotAmountText.text    = amount.ToString();
            slotAmountText.enabled = amount > 1;
        }
        else
        {
            slotIcon.enabled = false;
            slotAmountText.enabled = false;
        }
    }
}
