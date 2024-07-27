//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using UnityEngine;
//using UnityEngine.AI;

//public class EnemyFSM : MonoBehaviour
//{
//    public enum State
//    {
//        Idle,
//        Patrolling,
//        Chasing,
//        Shooting,
//        GoLocation
//    }

//    public State currentState;
//    public GameObject[] waypoints;
//    public float patrolSpeed = 2f;
//    public float chaseSpeed = 4f;
//    public float detectionRange = 25f;
//    public float shootingRange = 20f;
//    public float fireRate = 1f;

//    public GameObject bulletPrefab;
//    public Transform bulletPoint; // Changed to Transform
//    public GameObject player;
//    //public GameObject rayPrefab;
//    public float bulletSpeed = 50.0f;
//    public float turnSpeed = 2f;

//    private int currentWaypointIndex;
//    private float nextFireTime;
//    // private GameObject currentRay;


//    //Ray Casting
//    public GameObject OriginRay;
//    private LineRenderer[] lineRenderers;
//    public float rotationTolerance = 0.1f;  // Tolerance to stop rotating when close enough
//    public float rotationSpeed = 5f;
//    private Vector3[] directions = new Vector3[45];

//    private NavMeshAgent agent;


//    private Vector3 bulletInitialPosition;

//    private bool hasBulletPosition = false;
//    private AudioSource audioSource;
//    // private float soundDelay = 0.03f;

//    private bool hasDetectedBullet = false;


//    public void SetBulletInitialPosition(Vector3 position)
//    {
//        bulletInitialPosition = position;
//        hasBulletPosition = true;
//        currentState = State.GoLocation; // Change state to GoLocation when bullet position is set
//    }

//    void Start()
//    {
//        // Define the directions for each ray

//        // DO NOT TOUCH THESE
//        directions[0] = Vector3.forward;  // Center
//        directions[1] = (Vector3.forward + 0.05f * Vector3.up).normalized;  // Top
//        directions[2] = (Vector3.forward + 0.05f * Vector3.down).normalized;  // Bottom
//        directions[3] = (Vector3.forward + 0.5f * Vector3.left).normalized;  // Left
//        directions[4] = (Vector3.forward + 0.5f * Vector3.right).normalized;  // Right
//        directions[5] = (Vector3.forward + 0.05f * Vector3.up + 0.05f * Vector3.left).normalized;  // Top left
//        directions[6] = (Vector3.forward + 0.05f * Vector3.up + 0.05f * Vector3.right).normalized;  // Top right
//        directions[7] = (Vector3.forward + 0.05f * Vector3.down + 0.05f * Vector3.left).normalized;  // Bottom left
//        directions[8] = (Vector3.forward + 0.05f * Vector3.down + 0.05f * Vector3.right).normalized;  // Bottom right


//        directions[9] = (Vector3.forward + 0.05f * Vector3.up + 0.05f * Vector3.left).normalized;  // Top left
//        directions[10] = (Vector3.forward + 0.05f * Vector3.up + 0.15f * Vector3.left).normalized;  // Top left
//        directions[11] = (Vector3.forward + 0.05f * Vector3.up + 0.25f * Vector3.left).normalized;  // Top left
//        directions[12] = (Vector3.forward + 0.05f * Vector3.up + 0.35f * Vector3.left).normalized;  // Top left
//        directions[13] = (Vector3.forward + 0.05f * Vector3.up + 0.45f * Vector3.left).normalized;  // Top left
//        directions[14] = (Vector3.forward + 0.05f * Vector3.up + 0.55f * Vector3.left).normalized;  // Top left

//        directions[15] = (Vector3.forward + 0.05f * Vector3.left).normalized;  // Top left
//        directions[16] = (Vector3.forward + 0.15f * Vector3.left).normalized;  // Top left
//        directions[17] = (Vector3.forward + 0.25f * Vector3.left).normalized;  // Top left
//        directions[18] = (Vector3.forward + 0.35f * Vector3.left).normalized;  // Top left
//        directions[19] = (Vector3.forward + 0.45f * Vector3.left).normalized;  // Top left
//        directions[20] = (Vector3.forward + 0.55f * Vector3.left).normalized;  // Top left

//        directions[21] = (Vector3.forward + 0.05f * Vector3.down + 0.05f * Vector3.left).normalized;  // Top left
//        directions[22] = (Vector3.forward + 0.05f * Vector3.down + 0.15f * Vector3.left).normalized;  // Top left
//        directions[23] = (Vector3.forward + 0.05f * Vector3.down + 0.25f * Vector3.left).normalized;  // Top left
//        directions[24] = (Vector3.forward + 0.05f * Vector3.down + 0.35f * Vector3.left).normalized;  // Top left
//        directions[25] = (Vector3.forward + 0.05f * Vector3.down + 0.45f * Vector3.left).normalized;  // Top left
//        directions[26] = (Vector3.forward + 0.05f * Vector3.down + 0.55f * Vector3.left).normalized;  // Top left


//        directions[27] = (Vector3.forward + 0.05f * Vector3.up + 0.05f * Vector3.right).normalized;  // Top left
//        directions[28] = (Vector3.forward + 0.05f * Vector3.up + 0.15f * Vector3.right).normalized;  // Top left
//        directions[29] = (Vector3.forward + 0.05f * Vector3.up + 0.25f * Vector3.right).normalized;  // Top left
//        directions[30] = (Vector3.forward + 0.05f * Vector3.up + 0.35f * Vector3.right).normalized;  // Top left
//        directions[31] = (Vector3.forward + 0.05f * Vector3.up + 0.45f * Vector3.right).normalized;  // Top left
//        directions[32] = (Vector3.forward + 0.05f * Vector3.up + 0.55f * Vector3.right).normalized;  // Top left

//        directions[33] = (Vector3.forward + 0.05f * Vector3.right).normalized;  // Top left
//        directions[34] = (Vector3.forward + 0.15f * Vector3.right).normalized;  // Top left
//        directions[35] = (Vector3.forward + 0.25f * Vector3.right).normalized;  // Top left
//        directions[36] = (Vector3.forward + 0.35f * Vector3.right).normalized;  // Top left
//        directions[37] = (Vector3.forward + 0.45f * Vector3.right).normalized;  // Top left
//        directions[38] = (Vector3.forward + 0.55f * Vector3.right).normalized;  // Top left

//        directions[39] = (Vector3.forward + 0.05f * Vector3.down + 0.05f * Vector3.right).normalized;  // Top left
//        directions[40] = (Vector3.forward + 0.05f * Vector3.down + 0.15f * Vector3.right).normalized;  // Top left
//        directions[41] = (Vector3.forward + 0.05f * Vector3.down + 0.25f * Vector3.right).normalized;  // Top left
//        directions[42] = (Vector3.forward + 0.05f * Vector3.down + 0.35f * Vector3.right).normalized;  // Top left
//        directions[43] = (Vector3.forward + 0.05f * Vector3.down + 0.45f * Vector3.right).normalized;  // Top left
//        directions[44] = (Vector3.forward + 0.05f * Vector3.down + 0.55f * Vector3.right).normalized;  // Top left


//        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
//        audioSource = GetComponent<AudioSource>();

//        // Create LineRenderer instances
//        lineRenderers = new LineRenderer[45];
//        for (int i = 0; i < lineRenderers.Length; i++)
//        {
//            GameObject lrObject = new GameObject("LineRenderer_" + i);
//            lrObject.transform.SetParent(transform);
//            lineRenderers[i] = lrObject.AddComponent<LineRenderer>();
//            lineRenderers[i].startWidth = 0.05f;
//            lineRenderers[i].endWidth = 0.05f;
//            lineRenderers[i].sortingOrder = 5;  // Adjust as needed
//            lineRenderers[i].material = new Material(Shader.Find("Sprites/Default"));  // Default material for rendering
//        }

//        currentState = State.Idle;
//        player = GameObject.FindGameObjectWithTag("Player");
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
//            case State.GoLocation:
//                GoLocation();
//                break;
//        }


//        GameObject bullet = GameObject.FindGameObjectWithTag("PlayerBullet");

//        // Calculate the direction to the player
//        Vector3 directionToPlayer = player.transform.position - transform.position;

//        // Calculate the angle between the enemy's forward direction and the direction to the player
//        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

//        // Define the maximum angle at which the player is considered in front of the enemy
//        float maxAngle = 90f; // Adjust this value based on your needs

//        // Calculate the distance to the player
//        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);


//        // Check if the bullet is not null
//        if (bullet != null)
//        {
//            if (!hasDetectedBullet)
//            {
//                // Capture the initial position of the bullet
//                bulletInitialPosition = bullet.transform.position;
//                hasDetectedBullet = true;

//                // Optional: Log the initial position for debugging
//                Debug.Log("Bullet initial position captured at: " + bulletInitialPosition);
//            }

//            // Set the NavMeshAgent destination to the initial bullet's position
//            agent.destination = bulletInitialPosition;

//            // Optional: Log the action for debugging
//            Debug.Log("Moving to Bullet's initial position at: " + bulletInitialPosition);
//        }
//        else if (hasDetectedBullet)
//        {

//            // Ensure the agent continues to move towards the last known position of the bullet
//            agent.destination = bulletInitialPosition;

//            // Optional: Log the action for debugging
//            Debug.Log("Bullet destroyed, moving to last known position: " + bulletInitialPosition);

//        }

//        // Check if the player is within shooting range
//        if (distanceToPlayer <= shootingRange)
//        {
//            currentState = State.Shooting;
//        }
//        // Check if the player is within detection range and in front of the enemy
//        else if (distanceToPlayer < detectionRange && angleToPlayer <= maxAngle)
//        {
//            currentState = State.Chasing;
//        }
//        else if (currentState == State.Chasing && distanceToPlayer >= detectionRange)
//        {
//            currentState = State.Patrolling;
//        }
//    }
//    void Idle()
//    {
//        // Logic for idle state
//        currentState = State.Patrolling; // Transition to patrolling after some time or condition
//    }

//    void Patrol()
//    {
//        // Logic for patrolling state
//        if (waypoints.Length == 0) return;

//        showRay();

//        GameObject targetWaypoint = waypoints[currentWaypointIndex];
//        Vector3 targetPosition = targetWaypoint.transform.position;


//        agent.destination = targetPosition;

//        if (Vector3.Distance(transform.position, targetPosition) < 6f)
//        {
//            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
//        }
//    }

//    void Chase()
//    {
//        // Logic for chasing state

//        showRay();

//        //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime);

//        //RotateTowardsPlayer();

//        agent.destination = player.transform.position;

//        if (Vector3.Distance(transform.position, player.transform.position) <= shootingRange)
//        {
//            currentState = State.Shooting;
//        }
//    }

//    void GoLocation()
//    {
//        if (hasBulletPosition && agent != null)
//        {
//            agent.destination = bulletInitialPosition;

//            Vector3 direction = bulletInitialPosition - transform.position;
//            if (direction != Vector3.zero) // Avoid zero direction
//            {
//                Quaternion targetRotation = Quaternion.LookRotation(direction);
//                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
//            }

//            if (Vector3.Distance(transform.position, bulletInitialPosition) <= 6f)
//            {
//                hasBulletPosition = false; // Reset after reaching the position
//                StartCoroutine(FollowBulletForSeconds(3f)); // Follow the bullet's position for 3 seconds
//            }
//        }
//    }

//    private IEnumerator FollowBulletForSeconds(float seconds)
//    {
//        yield return new WaitForSeconds(seconds);
//        currentState = State.Patrolling;
//    }

//    //void GoLocation()
//    //{
//    //    if (hasBulletPosition && agent != null)
//    //    {
//    //        agent.destination = bulletInitialPosition;

//    //        // Rotate towards the destination
//    //        Vector3 direction = bulletInitialPosition - transform.position;
//    //        if (direction != Vector3.zero) // Avoid zero direction
//    //        {
//    //            Quaternion targetRotation = Quaternion.LookRotation(direction);
//    //            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
//    //        }

//    //        if (Vector3.Distance(transform.position, bulletInitialPosition) <= 6f)
//    //        {
//    //            hasBulletPosition = false; // Reset after reaching the position
//    //            StartCoroutine(FollowBulletForSeconds(3f)); // Follow the bullet's position for 3 seconds
//    //        }
//    //    }
//    //}

//    void Shoot()
//    {
//        // Logic for shooting state
//        if (player != null)
//        {
//            showRay();
//            agent.destination = player.transform.position;
//            RotateTowardsPlayer();
//            ShootAtPlayerIfVisible();
//        }

//        if (Vector3.Distance(transform.position, player.transform.position) > shootingRange)
//        {
//            currentState = State.Chasing;
//        }
//    }



//    void RotateTowardsPlayer()
//    {
//        Vector3 direction = (player.transform.position - transform.position).normalized;
//        Quaternion lookRotation = Quaternion.LookRotation(direction);
//        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
//    }

//    void ShootAtPlayerIfVisible()
//    {
//        // Raycast from the enemy to the player
//        Vector3 direction = (player.transform.position - bulletPoint.position).normalized;
//        Ray ray = new Ray(bulletPoint.position, (player.transform.position - bulletPoint.position).normalized);
//        RaycastHit hit;

//        // Draw the ray in the Scene view for debugging
//        Debug.DrawRay(bulletPoint.position, direction * detectionRange, Color.red, 0.1f);
//        // Debug.Log("Raycasting to detect player");



//        if (Physics.Raycast(ray, out hit))
//        {
//            //Debug.Log("Ray hit: " + hit.transform.name);

//            // Check if the raycast hits the player
//            if (hit.transform.gameObject == player)
//            {
//                // Debug.Log("Player detected");

//                // Check if the enemy can fire
//                if (Time.time >= nextFireTime)
//                {
//                    nextFireTime = Time.time + 1f / fireRate;

//                    // Call the Shoot method if the player is visible
//                    ShootAtPlayer();
//                }
//            }
//        }
//    }

//    void ShootAtPlayer()
//    {
//        Debug.Log("Enemy Shooting");

//        // Calculate the direction from the bullet spawn point to the player
//        Vector3 direction = (player.transform.position - bulletPoint.position).normalized;

//        // Instantiate the bullet
//        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.position, Quaternion.identity);

//        // Apply force to the bullet to move it towards the player
//        Rigidbody rb = bullet.GetComponent<Rigidbody>();
//        rb.velocity = direction * bulletSpeed;

//        // Destroy the bullet after 5 seconds
//        Destroy(bullet, 2.0f);
//    }

//    void showRay()
//    {


//        Vector3 closestHitPoint = Vector3.zero;

//        for (int i = 0; i < directions.Length; i++)
//        {
//            Vector3 direction = transform.TransformDirection(directions[i]);

//            if (Physics.Raycast(OriginRay.transform.position, direction, out RaycastHit hitinfo, detectionRange))
//            {
//                // Check if the hit object is the player
//                if (hitinfo.collider.CompareTag("Player"))
//                {
//                    closestHitPoint = hitinfo.point;

//                    // Set the positions for the LineRenderer to visualize the raycast hit
//                    lineRenderers[i].SetPosition(0, OriginRay.transform.position);
//                    lineRenderers[i].SetPosition(1, hitinfo.point);

//                    // Set the color of the line to red
//                    lineRenderers[i].startColor = Color.red;
//                    lineRenderers[i].endColor = Color.red;

//                    // var playerAttributes = hitinfo.collider.GetComponent<AttributeManager>();
//                    // if (playerAttributes != null)
//                    // {
//                    //     AttributeManager.DealDamage(hitinfo.collider.gameObject);
//                    // }
//                }
//                else
//                {
//                    // Hit something else (e.g., wall), draw to the hit point
//                    lineRenderers[i].SetPosition(0, OriginRay.transform.position);
//                    lineRenderers[i].SetPosition(1, hitinfo.point);

//                    // Set the color of the line to yellow
//                    lineRenderers[i].startColor = Color.yellow;
//                    lineRenderers[i].endColor = Color.yellow;
//                }
//            }
//            else
//            {
//                // If no hit, draw the full length ray
//                lineRenderers[i].SetPosition(0, OriginRay.transform.position);
//                lineRenderers[i].SetPosition(1, OriginRay.transform.position + direction * detectionRange);

//                // Set the color of the line to green
//                lineRenderers[i].startColor = Color.green;
//                lineRenderers[i].endColor = Color.green;
//            }
//        }
//    }

//}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class EnemyFSM : MonoBehaviour
//{
//    public enum State
//    {
//        Idle,
//        Patrolling,
//        Chasing,
//        Shooting,
//        GoLocation
//    }

//    public State currentState;
//    public GameObject[] waypoints;
//    public float patrolSpeed = 2f;
//    public float chaseSpeed = 4f;
//    public float detectionRange = 25f;
//    public float shootingRange = 20f;
//    public float fireRate = 1f;

//    public GameObject bulletPrefab;
//    public Transform bulletPoint;
//    public GameObject player;
//    public float bulletSpeed = 50.0f;
//    public float turnSpeed = 2f;

//    private int currentWaypointIndex;
//    private float nextFireTime;
//    private NavMeshAgent agent;

//    private Vector3 bulletInitialPosition;
//    private GameObject currentBullet;
//    private bool hasBulletPosition = false;
//    private AudioSource audioSource;
//    private bool hasDetectedBullet = false;

//    void Start()
//    {
//        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
//        audioSource = GetComponent<AudioSource>();
//        currentState = State.Idle;
//        player = GameObject.FindGameObjectWithTag("Player");
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
//            case State.GoLocation:
//                GoLocation();
//                break;
//        }

//        GameObject bullet = GameObject.FindGameObjectWithTag("PlayerBullet");

//        // Calculate the direction to the player
//        Vector3 directionToPlayer = player.transform.position - transform.position;

//        // Calculate the angle between the enemy's forward direction and the direction to the player
//        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

//        // Define the maximum angle at which the player is considered in front of the enemy
//        float maxAngle = 90f;

//        // Calculate the distance to the player
//        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

//        // Check if the bullet is not null
//        if (bullet != null)
//        {
//            currentBullet = bullet;
//            if (!hasDetectedBullet)
//            {
//                hasDetectedBullet = true;
//            }
//        }

//        if (hasDetectedBullet && currentBullet != null)
//        {
//            agent.destination = currentBullet.transform.position;
//            Debug.Log("Following bullet at: " + currentBullet.transform.position);
//        }

//        // Check if the player is within shooting range
//        if (distanceToPlayer <= shootingRange)
//        {
//            currentState = State.Shooting;
//        }
//        // Check if the player is within detection range and in front of the enemy
//        else if (distanceToPlayer < detectionRange && angleToPlayer <= maxAngle)
//        {
//            currentState = State.Chasing;
//        }
//        else if (currentState == State.Chasing && distanceToPlayer >= detectionRange)
//        {
//            currentState = State.Patrolling;
//        }
//    }

//    public void SetBulletInitialPosition(Vector3 position)
//    {
//        bulletInitialPosition = position;
//        hasDetectedBullet = true;
//    }
//    void Idle()
//    {
//        currentState = State.Patrolling;
//    }

//    void Patrol()
//    {
//        if (waypoints.Length == 0) return;

//        GameObject targetWaypoint = waypoints[currentWaypointIndex];
//        Vector3 targetPosition = targetWaypoint.transform.position;

//        agent.destination = targetPosition;

//        if (Vector3.Distance(transform.position, targetPosition) < 6f)
//        {
//            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
//        }
//    }

//    void Chase()
//    {
//        agent.destination = player.transform.position;

//        if (Vector3.Distance(transform.position, player.transform.position) <= shootingRange)
//        {
//            currentState = State.Shooting;
//        }
//    }

//    void GoLocation()
//    {
//        if (hasBulletPosition && agent != null)
//        {
//            agent.destination = bulletInitialPosition;

//            Vector3 direction = bulletInitialPosition - transform.position;
//            if (direction != Vector3.zero)
//            {
//                Quaternion targetRotation = Quaternion.LookRotation(direction);
//                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
//            }

//            if (Vector3.Distance(transform.position, bulletInitialPosition) <= 6f)
//            {
//                hasBulletPosition = false;
//                StartCoroutine(FollowBulletForSeconds(3f));
//            }
//        }
//    }

//    private IEnumerator FollowBulletForSeconds(float seconds)
//    {
//        yield return new WaitForSeconds(seconds);
//        currentState = State.Patrolling;
//    }

//    void Shoot()
//    {
//        if (player != null)
//        {
//            agent.destination = player.transform.position;
//            RotateTowardsPlayer();
//            ShootAtPlayerIfVisible();
//        }

//        if (Vector3.Distance(transform.position, player.transform.position) > shootingRange)
//        {
//            currentState = State.Chasing;
//        }
//    }

//    void RotateTowardsPlayer()
//    {
//        Vector3 direction = (player.transform.position - transform.position).normalized;
//        Quaternion lookRotation = Quaternion.LookRotation(direction);
//        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
//    }

//    void ShootAtPlayerIfVisible()
//    {
//        Vector3 direction = (player.transform.position - bulletPoint.position).normalized;
//        Ray ray = new Ray(bulletPoint.position, direction);
//        RaycastHit hit;

//        Debug.DrawRay(bulletPoint.position, direction * detectionRange, Color.red, 0.1f);

//        if (Physics.Raycast(ray, out hit))
//        {
//            if (hit.transform.gameObject == player)
//            {
//                if (Time.time >= nextFireTime)
//                {
//                    nextFireTime = Time.time + 1f / fireRate;
//                    ShootAtPlayer();
//                }
//            }
//        }
//    }

//    void ShootAtPlayer()
//    {
//        Debug.Log("Enemy Shooting");

//        Vector3 direction = (player.transform.position - bulletPoint.position).normalized;
//        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.position, Quaternion.identity);
//        Rigidbody rb = bullet.GetComponent<Rigidbody>();
//        rb.velocity = direction * bulletSpeed;

//        Destroy(bullet, 2.0f);
//    }
//}

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
    private bool hasDetectedBullet = false;
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

        //// Check if the bullet is not null
        //if (bullet != null)
        //{
        //    hasDetectedBullet = true;
        //    bulletInitialPosition = bullet.transform.position;
        //}

        //if (hasDetectedBullet)
        //{
        //    agent.destination = bulletInitialPosition;
        //    Debug.Log("Following bullet at: " + bulletInitialPosition);
        //}

        //// Check if the player is within shooting range
        //if (distanceToPlayer <= shootingRange)
        //{
        //    currentState = State.Shooting;
        //}
        //// Check if the player is within detection range and in front of the enemy
        //else if (distanceToPlayer < detectionRange && angleToPlayer <= maxAngle)
        //{
        //    currentState = State.Chasing;
        //}
        //else if (currentState == State.Chasing && distanceToPlayer >= detectionRange)
        //{
        //    currentState = State.Patrolling;
        //}
        // Check if the bullet is not null
        if (bullet != null)
        {
            hasDetectedBullet = true;
            bulletInitialPosition = bullet.transform.position;
            currentState = State.GoLocation; // Transition to GoLocation state when bullet is detected
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


