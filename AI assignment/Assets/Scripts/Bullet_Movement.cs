using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Movement : MonoBehaviour
{
    public float bulletSpeed;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Destroy(gameObject, 5f); // Destroy the bullet after 5 seconds to clean up
    }

    public void Initialize(Vector3 direction)
    {
        rb.velocity = direction * bulletSpeed;
    }

}
