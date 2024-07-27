using UnityEngine;

public class BulletShooting : MonoBehaviour
{
    // Store the bullet's initial position
    public Vector3 initialPosition;

    void Start()
    {
        // Capture the initial position when the bullet is instantiated
        initialPosition = transform.position;

        // Optional: Destroy the bullet after some time to avoid clutter
        //Destroy(gameObject, 5f); // Adjust time as needed
    }
}