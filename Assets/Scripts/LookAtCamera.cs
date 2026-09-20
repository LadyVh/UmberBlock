using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform head;
    public float height = 0.5f;

    void LateUpdate()
    {
        if (head != null)
        {
            transform.position = head.position + Vector3.up * height;
        }

        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}