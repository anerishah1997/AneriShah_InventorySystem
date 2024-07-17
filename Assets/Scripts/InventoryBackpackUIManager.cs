using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*using static UnityEditor.Progress;*/

public class InventoryBackpackUIManager : MonoBehaviour
{
    public Transform inventorySlotsParent;
    public Transform backpackSlotsParent;
    public Transform itemPrefabsParent;
    public Transform buttetItemPrefabsParent;
    public Button shootBtn;
    public GameObject[] itemSlotPrefabs; // array to hold the item prefabs.
    public AudioClip dropItemAudio; 
    public AudioClip pickItemAudio; 
    

    private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private List<InventorySlot> backpackSlots = new List<InventorySlot>();

    private bool isWeaponInBackpack;
    private bool IsBulletPickedUp;

    private bool isSameItemFound;



    void Start()
    {
        InitializeSlots(inventorySlotsParent, inventorySlots);
        InitializeSlots(backpackSlotsParent, backpackSlots);

    }

    void Update()
    {
        // the bullets should be appear on the ground when atleast one weapon is there in the backpack.
        if(isWeaponInBackpack)
        {
            buttetItemPrefabsParent.gameObject.SetActive(true);
        }
        else
        {
            buttetItemPrefabsParent.gameObject.SetActive(false);
        }

        // the shoot button activates only when there is weapon and bullet both present in the player's backpack
        if(isWeaponInBackpack && IsBulletPickedUp)
            shootBtn.gameObject.SetActive(true);
        else
            shootBtn.gameObject.SetActive(false);

    }

    private void InitializeSlots(Transform parent, List<InventorySlot> slotlist)
    {
        foreach (Transform slot in parent)
        {
            InventorySlot slotComponent = slot.GetComponent<InventorySlot>();
            slotlist.Add(slotComponent);
           /* Debug.Log("Added :- " +slotComponent.itemName.text);*/
        }
    }

    public void AddItemToInventory(Item item)
    {
        // when item  is picked up , either goes in inventory or backpack, only one time sound should be played
        this.GetComponent<AudioSource>().PlayOneShot(pickItemAudio); 

        foreach (InventorySlot slot in inventorySlots)
        {
            if(item.itemName.Trim().Equals(slot.itemName.text.Trim()))
            {
                slot.AddItem(item);
                break;
            }
            if(string.IsNullOrEmpty(slot.itemName.text))
            {
                slot.SetItem(item);
                break;
            }
        }
        /*foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemName.text == null)
            {
                slot.SetItem(item);
                break;
            }
        }*/
    }

    public void AddItemToBackpack(Item item)
    {
        isSameItemFound = false; // initializing the bool

        foreach (InventorySlot slot in backpackSlots)
        {
            if (item.itemName.Trim().Equals(slot.itemName.text.Trim()))
            {
                isSameItemFound = true; // this is to check the condition for checking open slots
                Debug.Log("Backpack Item: " + item.itemName);
                slot.AddItem(item);
                break;
            }   
        }


        // only find null slots when the same item is not found.
        if (!isSameItemFound)
        {
            foreach(InventorySlot slot in backpackSlots)
            {
                if (string.IsNullOrEmpty(slot.itemName.text))
                {
                    // When any type of weapon is picked up, this bool gets true. Reflects in Update's logic
                    if (item.itemName.Equals("HandGun") || item.itemName.Equals("Rifle"))
                        isWeaponInBackpack = true;

                    // when the player collects bullet for the first time, this bool gets true.
                    if (item.itemName.Equals("Bullet"))
                        IsBulletPickedUp = true;


                    slot.SetItem(item);
                    break;
                }
            }
           
        }
        /*foreach (InventorySlot slot in backpackSlots)
        {
            if (slot.itemName.text == null)
            {
                Debug.Log("Backpack Null");
                slot.SetItem(item);
                break;
            }
        }*/
    }

    public void DropItemFromSlot(string itemName, bool isBulletFired)
    {
        // when the item is dropped the sound is played.
        this.GetComponent<AudioSource>().PlayOneShot(dropItemAudio);

        foreach(InventorySlot slot in backpackSlots)
        {
            if(itemName.Trim().Equals(slot.itemName.text.Trim()))
            {
                int count = int.Parse(slot.itemAmountTxt.text);
                /*Debug.Log("Item : " + slot.itemName.text + " Amount: " + count);*/
                // check if the amount is greater than one, i.e how many items are present in the slot of backpack
                if(count > 1)
                {
                    count -= 1;
                    slot.itemAmountTxt.text = count.ToString();
                }
                else
                {
                    // As soon as the weapon is dropped, the bool is set false.
                    if (itemName.Equals("HandGun") || itemName.Equals("Rifle"))
                        isWeaponInBackpack = false;

                    // As soon as the last bullet is dropped, the bool is set false.
                    if (itemName.Equals("Bullet"))
                        IsBulletPickedUp = false;

                    slot.ClearSlot();
                }
            }
        }

        foreach(InventorySlot slot in inventorySlots)
        {
            if (itemName.Trim().Equals(slot.itemName.text.Trim()))
            {
                int count = int.Parse(slot.itemAmountTxt.text);
                
                // check if the amount is greater than one, i.e how many items are present in the slot of inventorySlot
                if (count > 1)
                {
                    count -= 1;
                    slot.itemAmountTxt.text = count.ToString();
                }
                else
                {
                    slot.ClearSlot();
                }
            }
        }

        // if the bullet prefab is fired, we are calling this function of Drop item,
        // so to prevent spwaning the bullet in case of fire, this condition is applied before calling spawn func.
        if(!isBulletFired)
            SpawnedDroppedItem(itemName);
    }

    // now to spwan the item prefab randomly according to the itemName passed.
    public void SpawnedDroppedItem(string itemName)
    {
        foreach(GameObject itemPrefab in itemSlotPrefabs)
        {
            if(itemPrefab.GetComponent<Item>().itemName.Trim().Equals(itemName))
            {
                // if the weapon is dropped then change the total weapon count to 0
                if (itemName.Equals("HandGun") || itemName.Equals("Rifle"))
                    PlayerInteraction.weaponCount = 0;

                // setting the boundaries
                float minX = -30f, maxX = 50f;
                float minZ = -25f, maxZ = 30f;
                float y = 1.5f;

                Vector3 spawnedPosition = new Vector3(Random.Range(minX, maxX), y, Random.Range(minZ, maxZ));
                GameObject spawned_itemPrefab = Instantiate(itemPrefab, spawnedPosition, Quaternion.identity);
                if(itemName.Equals("Bullet"))
                {
                    spawned_itemPrefab.transform.SetParent(buttetItemPrefabsParent);
                    buttetItemPrefabsParent.SetParent(itemPrefabsParent);
                }
                else
                {
                    spawned_itemPrefab.transform.SetParent(itemPrefabsParent);
                }
                
                Debug.Log("Item Spawned: " + spawned_itemPrefab.GetComponent<Item>().itemName);
            }
        }
    }

}
