using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log("Vie restante : " + health);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}