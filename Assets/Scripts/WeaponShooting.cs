using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class WeaponShooting : MonoBehaviour
{
    public GameObject weaponInHand;
    public float recoilDistance = 0.08f;
    public float recoilSpeed = 15f;

    private Vector3 originalWeaponPosition;
    private bool isRecoiling = false;
    public ParticleSystem muzzleFlash;
    public AudioClip gunshotSound;
    public AudioSource audioSource;
    public int ammo = 12;
    public int maxAmmo = 12;
    public float reloadTime = 3.5f;
    public bool isReloading = false;
    public TMP_Text ammoText;
    public Animator playerAnimator;
    public GameObject bulletImpactPrefab;


    void Start()
    {
        originalWeaponPosition = weaponInHand.transform.localPosition;
    }

    void Update()
    {
        bool isAiming = Gamepad.current != null &&
                Gamepad.current.leftTrigger.isPressed &&
                !isReloading;

        ammoText.gameObject.SetActive(weaponInHand.activeSelf);

        // Tir avec L2 + R2
        if (Gamepad.current != null &&
            Gamepad.current.rightTrigger.wasPressedThisFrame &&
            weaponInHand.activeSelf &&
            isAiming && ammo > 0 && !isReloading)
        {
            isRecoiling = true;
            muzzleFlash.gameObject.SetActive(true);

            muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            muzzleFlash.Play();
            audioSource.PlayOneShot(gunshotSound);

            Shoot();
            ammo--;
            ammoText.text = ammo + "/" + maxAmmo;

            //StartCoroutine(StopMuzzleFlash());
        }

        // Recharger avec Carré
        if (Gamepad.current != null &&
            Gamepad.current.buttonWest.wasPressedThisFrame &&
            weaponInHand.activeSelf &&
            ammo < maxAmmo && !isReloading)
        {
            StartCoroutine(Reload());
        }

        // Recul du pistolet
        if (isRecoiling)
        {
            Vector3 recoilPosition =
                originalWeaponPosition + Vector3.left * recoilDistance;

            weaponInHand.transform.localPosition = Vector3.Lerp(
                weaponInHand.transform.localPosition,
                recoilPosition,
                recoilSpeed * Time.deltaTime
            );

            if (Vector3.Distance(
                    weaponInHand.transform.localPosition,
                    recoilPosition) < 0.01f)
            {
                isRecoiling = false;
            }
        }
        // Retour du pistolet à sa position normale
        else
        {
            weaponInHand.transform.localPosition = Vector3.Lerp(
                weaponInHand.transform.localPosition,
                originalWeaponPosition,
                recoilSpeed * Time.deltaTime
            );
        }
    }

    void Shoot()
    {
        Camera mainCamera = Camera.main;

        Ray ray = new Ray(
            mainCamera.transform.position,
            mainCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Debug.Log("Touché : " + hit.collider.gameObject.name);
            Debug.Log("Point d'impact : " + hit.point);

            // Crée l'impact de balle
            GameObject impact = Instantiate(
                bulletImpactPrefab,
                hit.point + hit.normal * 0.01f,
                Quaternion.LookRotation(-hit.normal)
            );

            // Supprime l'impact après 30 secondes
            Destroy(impact, 10f);

            // Vérifie si l'objet touché peut recevoir des dégâts
            TargetHealth target = hit.collider.GetComponent<TargetHealth>();

            if (target != null)
            {
                target.TakeDamage(20);
            }
        }
    }
    IEnumerator Reload()
    {
        isReloading = true;

        playerAnimator.SetTrigger("Reload");

        // Attend pendant la recharge
        yield return new WaitForSeconds(reloadTime);

        // Remplit le chargeur
        ammo = maxAmmo;
        ammoText.text = ammo + "/" + maxAmmo;

        isReloading = false;

        Debug.Log("Pistolet rechargé : " + ammo + " balles");
    }

    IEnumerator StopMuzzleFlash()
    {
        yield return new WaitForSeconds(0.08f);

        muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}