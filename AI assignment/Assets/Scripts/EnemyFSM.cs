//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyFSM : MonoBehaviour
//{
//    public enum State
//    {
//        Idle,
//        Patrolling,
//        Chasing,
//        Shooting
//    }

//    public State currentState;
//    public Transform[] waypoints;
//    public float patrolSpeed = 2f;
//    public float chaseSpeed = 4f;
//    public float detectionRange = 10f;
//    public float shootingRange = 5f;
//    public float fireRate = 1f;

//    public GameObject bulletPrefab;
//    public GameObject bulletPoint;
//    public float bulletSpeed = 500.0f;

//    private int currentWaypointIndex;
//    private Transform player;
//    private float nextFireTime;

//    void Start()
//    {
//        currentState = State.Idle;
//        player = GameObject.FindGameObjectWithTag("Player").transform;
//    }

//    void Update()
//    {
//        switch (currentState)
//        {
//            case State.Idle:
//                Idle();
//                break;
//            case State.Patrolling:
//                Patrol();
//                break;
//            case State.Chasing:
//                Chase();
//                break;
//            case State.Shooting:
//                Shoot();
//                break;
//        }

//        if (Vector3.Distance(transform.position, player.position) < detectionRange)
//        {
//            currentState = State.Chasing;
//        }
//        else if (currentState == State.Chasing && Vector3.Distance(transform.position, player.position) >= detectionRange)
//        {
//            currentState = State.Patrolling;
//        }

//        if (Vector3.Distance(transform.position, player.position) <= shootingRange)
//        {
//            currentState = State.Shooting;
//        }
//    }

//    void Idle()
//    {
//        // Logic for idle state
//        // Transition to patrolling after some time or condition
//        currentState = State.Patrolling;
//    }

//    void Patrol()
//    {
//        // Logic for patrolling state
//        if (waypoints.Length == 0) return;

//        Transform targetWaypoint = waypoints[currentWaypointIndex];
//        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, patrolSpeed * Time.deltaTime);

//        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
//        {
//            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
//        }
//    }

//    void Chase()
//    {
//        // Logic for chasing state
//        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

//        if (Vector3.Distance(transform.position, player.position) <= shootingRange)
//        {
//            currentState = State.Shooting;
//        }
//    }

//    void Shoot()
//    {
//        // Logic for shooting state
//        if (Time.time >= nextFireTime)
//        {
//            nextFireTime = Time.time + 1f / fireRate;

//            Vector3 direction = (player.position - bulletPoint.transform.position).normalized;
//            GameObject bullet = Instantiate(bulletPrefab, bulletPoint.transform.position, Quaternion.identity);
//            Rigidbody rb = bullet.GetComponent<Rigidbody>();
//            rb.velocity = direction * bulletSpeed;
//            Destroy(bullet, 5.0f);
//        }

//        if (Vector3.Distance(transform.position, player.position) > shootingRange)
//        {
//            currentState = State.Chasing;
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrolling,
        Chasing,
        Shooting
    }

    public State currentState;
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 10f;
    public float shootingRange = 5f;
    public float fireRate = 1f;

    public GameObject bulletPrefab;
    public Transform bulletPoint; // Changed to Transform
    public GameObject player;
    public GameObject rayPrefab;
    public float bulletSpeed = 500.0f;
    public float turnSpeed = 2f;

    private int currentWaypointIndex;
    private float nextFireTime;
    private GameObject currentRay;

    void Start()
    {
        currentState = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Patrolling:
                Patrol();
                break;
            case State.Chasing:
                Chase();
                break;
            case State.Shooting:
                Shoot();
                break;
        }

        if (Vector3.Distance(transform.position, player.transform.position) < detectionRange)
        {
            currentState = State.Chasing;
        }
        else if (currentState == State.Chasing && Vector3.Distance(transform.position, player.transform.position) >= detectionRange)
        {
            currentState = State.Patrolling;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= shootingRange)
        {
            currentState = State.Shooting;
        }
    }

    void Idle()
    {
        // Logic for idle state
        currentState = State.Patrolling; // Transition to patrolling after some time or condition
    }

    void Patrol()
    {
        // Logic for patrolling state
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, patrolSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void Chase()
    {
        // Logic for chasing state
        RotateTowardsPlayer();  // Ensure the enemy faces the player
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.transform.position) <= shootingRange)
        {
            currentState = State.Shooting;
        }
    }

    void Shoot()
    {
        // Logic for shooting state
        if (player != null)
        {
            RotateTowardsPlayer();
            ShootAtPlayerIfVisible();
        }

        if (Vector3.Distance(transform.position, player.transform.position) > shootingRange)
        {
            currentState = State.Chasing;
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
        Vector3 direction = (player.transform.position - bulletPoint.position).normalized;
        Ray ray = new Ray(bulletPoint.position, (player.transform.position - bulletPoint.position).normalized);
        RaycastHit hit;

        // Draw the ray in the Scene view for debugging
        Debug.DrawRay(bulletPoint.position, direction * detectionRange, Color.red, 0.1f);
        Debug.Log("Raycasting to detect player");

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Ray hit: " + hit.transform.name);

            // Check if the raycast hits the player
            if (hit.transform.gameObject == player)
            {
                Debug.Log("Player detected");

                // Check if the enemy can fire
                if (Time.time >= nextFireTime)
                {
                    Debug.Log("Firing at player");
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
        Vector3 direction = (player.transform.position - bulletPoint.position).normalized;

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.position, Quaternion.identity);

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



