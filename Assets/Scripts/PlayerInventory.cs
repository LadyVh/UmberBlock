using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<string> weapons = new List<string>();
    public string equippedWeapon = "";


    public void AddWeapon(string weaponName)
    {
        weapons.Add(weaponName);
        Debug.Log(weaponName + " ajouté à l'inventaire !");
    }

    public void EquipWeapon(string weaponName)
    {
        equippedWeapon = weaponName;
        Debug.Log(weaponName + " est maintenant équipé !");
    }
}