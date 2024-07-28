using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrolling,
        Chasing,
        Shooting,
        GoLocation
    }

    public State currentState;
    public GameObject[] waypoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 25f;
    public float shootingRange = 20f;
    public float fireRate = 1f;

    //Ray Casting
    public GameObject OriginRay;
    private LineRenderer[] lineRenderers;
    public float rotationTolerance = 0.1f;  // Tolerance to stop rotating when close enough
    public float rotationSpeed = 5f;
    private Vector3[] directions = new Vector3[45];

    public GameObject bulletPrefab;
    public Transform bulletPoint;
    public GameObject player;
    public float bulletSpeed = 50.0f;
    public float turnSpeed = 2f;

    private int currentWaypointIndex;
    private float nextFireTime;
    private NavMeshAgent agent;

    private Vector3 bulletInitialPosition;
    public bool hasDetectedBullet = false;
    private AudioSource audioSource;

    void Start()
    {

        directions[0] = Vector3.forward;  // Center
        directions[1] = (Vector3.forward + 0.05f * Vector3.up).normalized;  // Top
        directions[2] = (Vector3.forward + 0.05f * Vector3.down).normalized;  // Bottom
        directions[3] = (Vector3.forward + 0.5f * Vector3.left).normalized;  // Left
        directions[4] = (Vector3.forward + 0.5f * Vector3.right).normalized;  // Right
        directions[5] = (Vector3.forward + 0.05f * Vector3.up + 0.05f * Vector3.left).normalized;  // Top left
        directions[6] = (Vector3.forward + 0.05f * Vector3.up + 0.05f * Vector3.right).normalized;  // Top right
        directions[7] = (Vector3.forward + 0.05f * Vector3.down + 0.05f * Vector3.left).normalized;  // Bottom left
        directions[8] = (Vector3.forward + 0.05f * Vector3.down + 0.05f * Vector3.right).normalized;  // Bottom right


        directions[9] = (Vector3.forward + 0.05f * Vector3.up + 0.05f * Vector3.left).normalized;  // Top left
        directions[10] = (Vector3.forward + 0.05f * Vector3.up + 0.15f * Vector3.left).normalized;  // Top left
        directions[11] = (Vector3.forward + 0.05f * Vector3.up + 0.25f * Vector3.left).normalized;  // Top left
        directions[12] = (Vector3.forward + 0.05f * Vector3.up + 0.35f * Vector3.left).normalized;  // Top left
        directions[13] = (Vector3.forward + 0.05f * Vector3.up + 0.45f * Vector3.left).normalized;  // Top left
        directions[14] = (Vector3.forward + 0.05f * Vector3.up + 0.55f * Vector3.left).normalized;  // Top left

        directions[15] = (Vector3.forward + 0.05f * Vector3.left).normalized;  // Top left
        directions[16] = (Vector3.forward + 0.15f * Vector3.left).normalized;  // Top left
        directions[17] = (Vector3.forward + 0.25f * Vector3.left).normalized;  // Top left
        directions[18] = (Vector3.forward + 0.35f * Vector3.left).normalized;  // Top left
        directions[19] = (Vector3.forward + 0.45f * Vector3.left).normalized;  // Top left
        directions[20] = (Vector3.forward + 0.55f * Vector3.left).normalized;  // Top left

        directions[21] = (Vector3.forward + 0.05f * Vector3.down + 0.05f * Vector3.left).normalized;  // Top left
        directions[22] = (Vector3.forward + 0.05f * Vector3.down + 0.15f * Vector3.left).normalized;  // Top left
        directions[23] = (Vector3.forward + 0.05f * Vector3.down + 0.25f * Vector3.left).normalized;  // Top left
        directions[24] = (Vector3.forward + 0.05f * Vector3.down + 0.35f * Vector3.left).normalized;  // Top left
        directions[25] = (Vector3.forward + 0.05f * Vector3.down + 0.45f * Vector3.left).normalized;  // Top left
        directions[26] = (Vector3.forward + 0.05f * Vector3.down + 0.55f * Vector3.left).normalized;  // Top left


        directions[27] = (Vector3.forward + 0.05f * Vector3.up + 0.05f * Vector3.right).normalized;  // Top left
        directions[28] = (Vector3.forward + 0.05f * Vector3.up + 0.15f * Vector3.right).normalized;  // Top left
        directions[29] = (Vector3.forward + 0.05f * Vector3.up + 0.25f * Vector3.right).normalized;  // Top left
        directions[30] = (Vector3.forward + 0.05f * Vector3.up + 0.35f * Vector3.right).normalized;  // Top left
        directions[31] = (Vector3.forward + 0.05f * Vector3.up + 0.45f * Vector3.right).normalized;  // Top left
        directions[32] = (Vector3.forward + 0.05f * Vector3.up + 0.55f * Vector3.right).normalized;  // Top left

        directions[33] = (Vector3.forward + 0.05f * Vector3.right).normalized;  // Top left
        directions[34] = (Vector3.forward + 0.15f * Vector3.right).normalized;  // Top left
        directions[35] = (Vector3.forward + 0.25f * Vector3.right).normalized;  // Top left
        directions[36] = (Vector3.forward + 0.35f * Vector3.right).normalized;  // Top left
        directions[37] = (Vector3.forward + 0.45f * Vector3.right).normalized;  // Top left
        directions[38] = (Vector3.forward + 0.55f * Vector3.right).normalized;  // Top left

        directions[39] = (Vector3.forward + 0.05f * Vector3.down + 0.05f * Vector3.right).normalized;  // Top left
        directions[40] = (Vector3.forward + 0.05f * Vector3.down + 0.15f * Vector3.right).normalized;  // Top left
        directions[41] = (Vector3.forward + 0.05f * Vector3.down + 0.25f * Vector3.right).normalized;  // Top left
        directions[42] = (Vector3.forward + 0.05f * Vector3.down + 0.35f * Vector3.right).normalized;  // Top left
        directions[43] = (Vector3.forward + 0.05f * Vector3.down + 0.45f * Vector3.right).normalized;  // Top left
        directions[44] = (Vector3.forward + 0.05f * Vector3.down + 0.55f * Vector3.right).normalized;  // Top left


        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        // Create LineRenderer instances
        lineRenderers = new LineRenderer[45];
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            GameObject lrObject = new GameObject("LineRenderer_" + i);
            lrObject.transform.SetParent(transform);
            lineRenderers[i] = lrObject.AddComponent<LineRenderer>();
            lineRenderers[i].startWidth = 0.05f;
            lineRenderers[i].endWidth = 0.05f;
            lineRenderers[i].sortingOrder = 5;  // Adjust as needed
            lineRenderers[i].material = new Material(Shader.Find("Sprites/Default"));  // Default material for rendering
        }

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        currentState = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void showRay()
    {


        Vector3 closestHitPoint = Vector3.zero;

        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 direction = transform.TransformDirection(directions[i]);

            if (Physics.Raycast(OriginRay.transform.position, direction, out RaycastHit hitinfo, detectionRange))
            {
                // Check if the hit object is the player
                if (hitinfo.collider.CompareTag("Player"))
                {
                    closestHitPoint = hitinfo.point;

                    // Set the positions for the LineRenderer to visualize the raycast hit
                    lineRenderers[i].SetPosition(0, OriginRay.transform.position);
                    lineRenderers[i].SetPosition(1, hitinfo.point);

                    // Set the color of the line to red
                    lineRenderers[i].startColor = Color.red;
                    lineRenderers[i].endColor = Color.red;

                    // var playerAttributes = hitinfo.collider.GetComponent<AttributeManager>();
                    // if (playerAttributes != null)
                    // {
                    //     AttributeManager.DealDamage(hitinfo.collider.gameObject);
                    // }
                }
                else
                {
                    // Hit something else (e.g., wall), draw to the hit point
                    lineRenderers[i].SetPosition(0, OriginRay.transform.position);
                    lineRenderers[i].SetPosition(1, hitinfo.point);

                    // Set the color of the line to yellow
                    lineRenderers[i].startColor = Color.yellow;
                    lineRenderers[i].endColor = Color.yellow;
                }
            }
            else
            {
                // If no hit, draw the full length ray
                lineRenderers[i].SetPosition(0, OriginRay.transform.position);
                lineRenderers[i].SetPosition(1, OriginRay.transform.position + direction * detectionRange);

                // Set the color of the line to green
                lineRenderers[i].startColor = Color.green;
                lineRenderers[i].endColor = Color.green;
            }
        }
    }

    void Update()
    {
        showRay();

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
            case State.GoLocation:
                GoLocation();
                break;
        }

        GameObject bullet = GameObject.FindGameObjectWithTag("PlayerBullet");

        // Calculate the direction to the player
        Vector3 directionToPlayer = player.transform.position - transform.position;

        // Calculate the angle between the enemy's forward direction and the direction to the player
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Define the maximum angle at which the player is considered in front of the enemy
        float maxAngle = 90f;

        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check if the bullet is not null
        if (bullet != null && !hasDetectedBullet)
        {
            hasDetectedBullet = true;
            bulletInitialPosition = bullet.transform.position;
            currentState = State.GoLocation; // Transition to GoLocation state when bullet is detected
            Debug.Log("Detected bullet at: " + bulletInitialPosition);
        }

        if (hasDetectedBullet && currentState == State.GoLocation)
        {
            agent.destination = bulletInitialPosition;
            Debug.Log("Following bullet at: " + bulletInitialPosition);

            // Check if the enemy has reached the bullet's initial position
            if (Vector3.Distance(transform.position, bulletInitialPosition) < 5f)
            {
                hasDetectedBullet = false; // Reset bullet detection
                currentState = State.Patrolling; // Transition back to patrolling
            }
        }

        // Check if the player is within shooting range
        if (distanceToPlayer <= shootingRange)
        {
            currentState = State.Shooting;
        }
        // Check if the player is within detection range and in front of the enemy
        else if (distanceToPlayer < detectionRange && angleToPlayer <= maxAngle)
        {
            currentState = State.Chasing;
        }
        else if (currentState == State.Chasing && distanceToPlayer >= detectionRange)
        {
            currentState = State.Patrolling;
        }
    }

    public void SetBulletInitialPosition(Vector3 position)
    {
        bulletInitialPosition = position;
        hasDetectedBullet = true;
    }

    void Idle()
    {
        currentState = State.Patrolling;
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        GameObject targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 targetPosition = targetWaypoint.transform.position;

        agent.destination = targetPosition;

        if (Vector3.Distance(transform.position, targetPosition) < 6f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void Chase()
    {
        agent.destination = player.transform.position;

        if (Vector3.Distance(transform.position, player.transform.position) <= shootingRange)
        {
            currentState = State.Shooting;
        }
    }

    void GoLocation()
    {
        if (hasDetectedBullet && agent != null)
        {
            agent.destination = bulletInitialPosition;

            Vector3 direction = bulletInitialPosition - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
        }
    }

    void Shoot()
    {
        if (player != null)
        {
            agent.destination = player.transform.position;
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
        Vector3 direction = (player.transform.position - bulletPoint.position).normalized;
        Ray ray = new Ray(bulletPoint.position, direction);
        RaycastHit hit;

        Debug.DrawRay(bulletPoint.position, direction * detectionRange, Color.red, 0.1f);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject == player)
            {
                if (Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + 1f / fireRate;
                    ShootAtPlayer();
                }
            }
        }
    }

    void ShootAtPlayer()
    {
        Debug.Log("Enemy Shooting");

        Vector3 direction = (player.transform.position - bulletPoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = direction * bulletSpeed;

        Destroy(bullet, 2.0f);
    }
}

