using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletPoint;
    public GameObject player;



    public float bulletSpeed = 500.0f;
    public float turnSpeed = 2f;
    public float fireRate = 1f; // Bullets per second

    private float nextFireTime;




    void Start()
    {

    }


    void Update()
    {
        if (player != null)
        {
            RotateTowardsPlayer();
            ShootAtPlayer();
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = -(player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
    }

    void ShootAtPlayer()
    {


        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;

            Debug.Log("Enemy Shooting");

            // Calculate the direction from the bullet spawn point to the player
            Vector3 direction = (player.transform.position - bulletPoint.transform.position).normalized;

            // Instantiate the bullet
            GameObject bullet = Instantiate(bulletPrefab, bulletPoint.transform.position, Quaternion.identity);

            // Apply force to the bullet to move it towards the player
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = direction * bulletSpeed;

            // Destroy the bullet after 5 seconds
            Destroy(bullet, 5.0f);
        }


    }
}

