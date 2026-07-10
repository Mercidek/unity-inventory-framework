using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
{
    [SerializeField] private Image slotIcon;
    [SerializeField] private TextMeshProUGUI slotAmountText;
    private int myIndex;

    public event Action<int> OnSlotHovered;
    public event Action<int, int> OnSlotClicked;

    public void InitSlot(int index)
    {
        myIndex = index;
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
        OnSlotHovered?.Invoke(myIndex);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        TooltipManager.Instance.UpdatePosition(eventData.position);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSlotClicked?.Invoke(myIndex, 1);
    }
}
