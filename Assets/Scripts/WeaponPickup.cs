using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                inventory.AddWeapon(gameObject.name);

                InventoryUI inventoryUI = FindFirstObjectByType<InventoryUI>();

                if (inventoryUI != null)
                {
                    inventoryUI.weaponIcon.SetActive(true);
                }

                Debug.Log("Pistolet ramassé !");

                gameObject.SetActive(false);
            }
        }
    }
}