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
    public GameObject bulletPoint;
    public float bulletSpeed = 500.0f;

    private int currentWaypointIndex;
    private Transform player;
    private float nextFireTime;

    void Start()
    {
        currentState = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
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

        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            currentState = State.Chasing;
        }
        else if (currentState == State.Chasing && Vector3.Distance(transform.position, player.position) >= detectionRange)
        {
            currentState = State.Patrolling;
        }

        if (Vector3.Distance(transform.position, player.position) <= shootingRange)
        {
            currentState = State.Shooting;
        }
    }

    void Idle()
    {
        // Logic for idle state
        // Transition to patrolling after some time or condition
        currentState = State.Patrolling;
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
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.position) <= shootingRange)
        {
            currentState = State.Shooting;
        }
    }

    void Shoot()
    {
        // Logic for shooting state
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;

            Vector3 direction = (player.position - bulletPoint.transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, bulletPoint.transform.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = direction * bulletSpeed;
            Destroy(bullet, 5.0f);
        }

        if (Vector3.Distance(transform.position, player.position) > shootingRange)
        {
            currentState = State.Chasing;
        }
    }
}