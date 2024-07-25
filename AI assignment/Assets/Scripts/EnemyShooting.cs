using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletPoint; // Change to Transform
    public GameObject player;
    public GameObject rayPrefab;


    public float bulletSpeed = 500.0f;
    public float turnSpeed = 2f;
    public float fireRate = 1f; // Bullets per second

    private float nextFireTime;
    
    private GameObject currentRay;



    void Start()
    {
    }

    void Update()
    {
        if (player != null)
        {
            RotateTowardsPlayer();
            ShootAtPlayerIfVisible();
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = -(player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
    }

    void ShootAtPlayerIfVisible()
    {
        // Raycast from the enemy to the player
        Ray ray = new Ray(bulletPoint.position, (player.transform.position - bulletPoint.position).normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the raycast hits the player
            if (hit.transform.gameObject == player)
            {
                // Comment out the timing logic for future references
                 if (Time.time >= nextFireTime)
                 {
                     nextFireTime = Time.time + 1f / fireRate;

                    // Call the Shoot method if the player is visible
                    ShootAtPlayer();
                 }
            }
        }
    }

    void ShootAtPlayer()
    {
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

    void showRay()
    {

        // Destroy the previous ray
        if (currentRay != null)
        {
            Destroy(currentRay);
        }

        // Instantiate the ray prefab
        currentRay = Instantiate(rayPrefab, bulletPoint.position, Quaternion.identity);

        // Set the positions of the LineRenderer to show the ray
        LineRenderer lr = currentRay.GetComponent<LineRenderer>();
        lr.SetPosition(0, bulletPoint.position);
        lr.SetPosition(1, player.transform.position);

        // Check if the ray intersects with the player
        if (Vector3.Distance(player.transform.position, bulletPoint.position) < lr.bounds.size.magnitude)
        {
            ShootAtPlayer();
        }


    }


}

