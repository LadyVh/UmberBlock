using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject weaponIcon;
    public GameObject weaponDetailsPanel;
    public PlayerInventory playerInventory;
    public GameObject hotbarWeaponIcon;
    public GameObject weaponInHand;
    public Animator playerAnimator;
    public GameObject aimCamera;
    public StarterAssets.ThirdPersonController thirdPersonController;
    public GameObject crosshair;
    public WeaponShooting weaponShooting;
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        weaponIcon.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }

        if (Keyboard.current != null && Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SelectHotbarWeapon();
        }

        if (Gamepad.current != null)
        {
            bool isAiming = Gamepad.current.leftTrigger.isPressed
                && weaponInHand.activeSelf
                && !weaponShooting.isReloading;

            playerAnimator.SetBool("IsAiming", isAiming);
            thirdPersonController.isAiming = isAiming;
            crosshair.SetActive(isAiming);

            if (isAiming)
            {
                aimCamera.GetComponent<Unity.Cinemachine.CinemachineCamera>().Priority = 20;
            }
            else
            {
                aimCamera.GetComponent<Unity.Cinemachine.CinemachineCamera>().Priority = 5;
            }
        }
    }

    public void ToggleInventory()
    {
        bool isOpen = !inventoryPanel.activeSelf;

        inventoryPanel.SetActive(isOpen);

        Cursor.visible = isOpen;

        if (isOpen)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SelectWeapon()
    {
        inventoryPanel.SetActive(false);
        weaponDetailsPanel.SetActive(true);
    }

    public void EquipWeapon()
    {
        playerInventory.EquipWeapon("SM_Wep_Pistol_Heavy_01");
        hotbarWeaponIcon.SetActive(true);
    }

    public void BackToInventory()
    {
        weaponDetailsPanel.SetActive(false);
        inventoryPanel.SetActive(true);
    }

    public void SelectHotbarWeapon()
    {
        if (playerInventory.equippedWeapon == "SM_Wep_Pistol_Heavy_01")
        {
            weaponInHand.SetActive(!weaponInHand.activeSelf);
        }
    }
}