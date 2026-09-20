using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    public float downedHealth = 20f;

    public bool isDowned = false;
    public Animator playerAnimator;
    public float recoverySpeed = 2f;
    public GameObject downedIndicator;
    public bool isDead = false;
    public GameObject deathPanel;
    public TMP_Text respawnButtonText;
    public float respawnDelay = 5f;
    public Button respawnButton;
    public Transform respawnPoint;
    private Vector3 deathPosition;
    public GameObject heavyPistolPickupPrefab;
    public PlayerInventory playerInventory;

    void Start()
    {
        downedIndicator.SetActive(false);
        deathPanel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
        {
            TakeDamage(20f);
        }

        if (isDowned && currentHealth < 30f)
        {
            currentHealth += recoverySpeed * Time.deltaTime;

            Debug.Log("Récupération : " + currentHealth);
        }

        if (isDowned && currentHealth >= 30f)
        {
            currentHealth = 30f;
            isDowned = false;

            playerAnimator.SetBool("isDowned", false);

            downedIndicator.SetActive(false);


            Debug.Log("Le joueur peut se relever !");
        }

    }

    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;

        Debug.Log("Vie du joueur : " + currentHealth);

        // Le joueur était déjà à terre et atteint 0 HP = mort
        if (isDowned && currentHealth <= 0)
        {
            Debug.Log("TEST : bloc de mort exécuté");
            currentHealth = 0;
            isDead = true;

            // Enregistrer l'endroit où le joueur est mort
            deathPosition = transform.position;
            Debug.Log("Nombre d'armes dans l'inventaire : " + playerInventory.weapons.Count);

            // Afficher les armes présentes dans l'inventaire
            foreach (string weapon in playerInventory.weapons)
            {
                Debug.Log("Arme dans inventaire : " + weapon);
            }

            // Faire apparaître le pistolet à l'endroit de la mort
            if (playerInventory.weapons.Contains("SM_Wep_Pistol_Heavy_01"))
            {
                Instantiate(
                    heavyPistolPickupPrefab,
                    deathPosition + Vector3.up * 0.5f,
                    Quaternion.identity
                );

                Debug.Log("Pistolet déposé au sol !");
            }

            // Afficher l'écran de mort
            deathPanel.SetActive(true);

            // Démarrer le compte à rebours
            StartCoroutine(RespawnCountdown());

            Debug.Log("Le joueur est mort !");

            return;
        }

        // Le joueur atteint 20 HP = à terre
        if (currentHealth <= downedHealth && !isDowned)
        {
            isDowned = true;

            playerAnimator.SetBool("isDowned", true);
            downedIndicator.SetActive(true);

            Debug.Log("Le joueur est à terre !");
        }
    }


    IEnumerator RespawnCountdown()
    {
        respawnButton.interactable = false;

        float timeLeft = respawnDelay;

        while (timeLeft > 0)
        {
            respawnButtonText.text = "Respawn (" + Mathf.CeilToInt(timeLeft) + ")";
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        respawnButtonText.text = "Respawn";
        respawnButton.interactable = true;
    }

    public void Respawn()
    {
        currentHealth = maxHealth;
        isDead = false;
        isDowned = false;

        playerAnimator.SetBool("isDowned", false);

        CharacterController controller = GetComponent<CharacterController>();

        controller.enabled = false;
        transform.position = respawnPoint.position;
        controller.enabled = true;

        deathPanel.SetActive(false);

        Debug.Log("Respawn !");
    }
}