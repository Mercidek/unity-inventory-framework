using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private Image slotIcon;
    [SerializeField] private TextMeshProUGUI slotAmountText;
    private InventoryUI myParent;
    private int myIndex;

    public void InitSlot(int index, InventoryUI UIParent)
    {
        myIndex   = index;
        myParent  = UIParent;
    }

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        myParent.HoverSlot(myIndex);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        TooltipManager.Instance.UpdatePosition(eventData.position);
    }
}
