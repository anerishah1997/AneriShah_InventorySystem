using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public bool isWeapon;
    public int amount;
    [HideInInspector]
    public int weaponCount;

    public void SetItemIcon(Image iconComponent)
    {
        if (itemIcon != null)
        {
            iconComponent.sprite = itemIcon;
        }
    }
}
