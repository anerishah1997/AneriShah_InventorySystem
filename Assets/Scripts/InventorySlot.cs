using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemAmountTxt;
    public TextMeshProUGUI itemName;
    public InventoryBackpackUIManager inventoryBackpackUIManager;

    public bool backpackSlotUI;


    private int count;
    private Button dropBtn;

    private void Start()
    {
        if(backpackSlotUI)
        {
            dropBtn = GetComponent<Button>();
            dropBtn.onClick.AddListener(() => DropBtnClicked()); // when the drop button clicked, this method will be called
        }
            
    }

    // when an inventory or the backpack slot is null, then set the new item which is passed to that slot
    public void SetItem(Item item)
    {
        item.SetItemIcon(itemIcon);

        itemIcon.enabled = true;

        itemAmountTxt.text = item.amount.ToString();

        itemName.text = item.itemName.ToString();

        Destroy(item.gameObject);
    }
    
    // when the item is already present in an inventory or backpack, then increase the amount of it.
    public void AddItem(Item item)
    {
        Debug.Log("Amount Before: " + itemAmountTxt.text);
        count = int.Parse(itemAmountTxt.text);
        count += item.amount;
        itemAmountTxt.text = count.ToString();
        Debug.Log("Amount After: " + itemAmountTxt.text);

        Destroy(item.gameObject);
    }

    public void ClearSlot()
    {
        Debug.Log("Clear Slot called");
        itemName.text = "";
        itemIcon.enabled = false;
        itemAmountTxt.text = "0";
    }

    public void DropBtnClicked()
    {
        Debug.Log("SLot Button CLicked");
        // we call the DropItem method of this script passing the clicked button's itemName.
        /*FindObjectOfType<InventoryBackpackUIManager>().DropItemFromSlot(itemName.text);*/
        inventoryBackpackUIManager.DropItemFromSlot(itemName.text, false);
    }
}
