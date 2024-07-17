using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    Inventory inventory = new Inventory();


    public InventoryBackpackUIManager inventoryBackpackManager;
    public Button shootBtn;

    public GameObject fireBulletPrefab;
    public AudioSource fireSound;
    public GameObject weaponInfoPanel;

    public static int weaponCount = 0;
    
    void Start()
    {
        shootBtn.onClick.AddListener(() => Shoot());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            //inventory.AddItem(item);

            // add that item in inventory and the backpack.
            // we need to check that user only holds one weapon at a time.
            if(item.isWeapon && weaponCount < 1)
            {
                weaponCount = 1;
                inventoryBackpackManager.AddItemToInventory(item);
                inventoryBackpackManager.AddItemToBackpack(item);
            }
            else if(!item.isWeapon)
            {
                inventoryBackpackManager.AddItemToInventory(item);
                inventoryBackpackManager.AddItemToBackpack(item);
            }
            else
            {
                StartCoroutine(DisplayWeaponInfoPanel());
                Debug.Log("Player already have a weapon");
            }
        }
    }

    public void Shoot()
    {
        // spawn the bullet from the player's transform and direction.
        Instantiate(fireBulletPrefab, this.transform.position + this.transform.forward, this.transform.rotation);
        fireSound.Play();
        inventoryBackpackManager.DropItemFromSlot("Bullet", true);
    }

    IEnumerator DisplayWeaponInfoPanel()
    {
        weaponInfoPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        weaponInfoPanel.SetActive(false);
    }
}
