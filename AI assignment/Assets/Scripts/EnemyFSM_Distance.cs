using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
public class EnemyFSM_Distance : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrolling,
        Chasing,
        Shooting
    }

    public State currentState;
    public GameObject[] waypoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 25f;
    public float shootingRange = 20f;
    public float fireRate = 1f;

    public GameObject bulletPrefab;
    public Transform bulletPoint; // Changed to Transform
    public GameObject player;
    //public GameObject rayPrefab;
    public float bulletSpeed = 50.0f;
    public float turnSpeed = 2f;

    private int currentWaypointIndex;
    private float nextFireTime;
    // private GameObject currentRay;

    public float rotationSpeed = 5f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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

        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check if the player is within detection range and transition states accordingly
        if (distanceToPlayer < detectionRange && currentState != State.Shooting)
        {
            currentState = State.Chasing;
        }
        else if (currentState == State.Chasing && distanceToPlayer >= detectionRange)
        {
            currentState = State.Patrolling;
        }

        if (distanceToPlayer <= shootingRange)
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

//        showRay();

        GameObject targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 targetPosition = targetWaypoint.transform.position;


        agent.destination = targetPosition;

        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void Chase()
    {
       agent.destination = player.transform.position; 
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
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
    }

    void ShootAtPlayerIfVisible()
    {
        // Check if the enemy can fire
        if (Time.time >= nextFireTime)
        {
            Debug.Log("Firing at player");
            nextFireTime = Time.time + 1f / fireRate;

            // Call the Shoot method
            ShootAtPlayer();
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

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
}