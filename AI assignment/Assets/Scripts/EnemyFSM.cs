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
using System.Security.Cryptography;
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


    //Ray Casting
    public GameObject OriginRay;
    private LineRenderer[] lineRenderers;
    public float rotationTolerance = 0.1f;  // Tolerance to stop rotating when close enough
    public float rotationSpeed = 5f;
    private Vector3[] directions = new Vector3[9];



    void Start()
    {

        // Define the directions for each ray
        directions[0] = Vector3.forward;  // Center
        directions[1] = (Vector3.forward + 0.05f * Vector3.up ).normalized;  // Top
        directions[2] = (Vector3.forward + 0.05f * Vector3.left).normalized;  // Left
        directions[3] = (Vector3.forward + 0.05f * Vector3.down).normalized;  // Bottom
        directions[4] = (Vector3.forward + 0.05f * Vector3.right).normalized;  // Right
        directions[5] = (Vector3.forward + 0.05f * Vector3.up  + 0.05f* Vector3.left).normalized;  // Top left
        directions[6] = (Vector3.forward + 0.05f * Vector3.up  + 0.05f* Vector3.right ).normalized;  // Top right
        directions[7] = (Vector3.forward + 0.05f * Vector3.down + 0.05f * Vector3.left ).normalized;  // Bottom left
        directions[8] = (Vector3.forward + 0.05f * Vector3.down + 0.05f * Vector3.right).normalized;  // Bottom right


        // Create LineRenderer instances
        lineRenderers = new LineRenderer[9];
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



        // Calculate the direction to the player
        Vector3 directionToPlayer = player.transform.position - transform.position;

        // Calculate the angle between the enemy's forward direction and the direction to the player
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Define the maximum angle at which the player is considered in front of the enemy
        float maxAngle = 90f; // Adjust this value based on your needs

        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check if the player is within detection range and in front of the enemy
        if (distanceToPlayer < detectionRange && angleToPlayer <= maxAngle)
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

        showRay();

        GameObject targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 targetPosition = targetWaypoint.transform.position;

        // Calculate direction to the target waypoint
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        // Rotate towards the direction to the target waypoint
        if (directionToTarget != Vector3.zero) // Avoid zero direction
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move towards the target waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        // Update waypoint index if close enough to the target waypoint
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void Chase()
    {
        // Logic for chasing state

        showRay();
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
            showRay();
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
        // Raycast from the enemy to the player
        Vector3 direction = (player.transform.position - bulletPoint.position).normalized;
        Ray ray = new Ray(bulletPoint.position, (player.transform.position - bulletPoint.position).normalized);
        RaycastHit hit;

        // Draw the ray in the Scene view for debugging
        Debug.DrawRay(bulletPoint.position, direction * detectionRange, Color.red, 0.1f);
       // Debug.Log("Raycasting to detect player");

       

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
        Destroy(bullet, 2.0f);
    }



    void showRay()
    {

        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitinfo, 20f))
        //{
        //    Debug.Log("Detected");

        //    // Set the positions for the LineRenderer to visualize the raycast hit
        //    lineRenderer.SetPosition(0, transform.position);
        //    lineRenderer.SetPosition(1, hitinfo.point);

        //    // Set the color of the line to red
        //    lineRenderer.startColor = Color.blue;
        //    lineRenderer.endColor = Color.red;
        //}
        //else
        //{
        //    Debug.Log("No Detection");

        //    // Set the positions for the LineRenderer to visualize the full length of the ray
        //    lineRenderer.SetPosition(0, transform.position);
        //    lineRenderer.SetPosition(1, transform.position + transform.TransformDirection(Vector3.forward) * 20f);

        //    // Set the color of the line to green
        //    lineRenderer.startColor = Color.green;
        //    lineRenderer.endColor = Color.green;
        //}



        Vector3 closestHitPoint = Vector3.zero;

        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 direction = transform.TransformDirection(directions[i]);

            if (Physics.Raycast(OriginRay.transform.position, direction, out RaycastHit hitinfo, detectionRange) && hitinfo.collider.CompareTag("Player"))
            {
                
                closestHitPoint = hitinfo.point;

                // Set the positions for the LineRenderer to visualize the raycast hit
                lineRenderers[i].SetPosition(0, OriginRay.transform.position);
                lineRenderers[i].SetPosition(1, hitinfo.point);

                // Set the color of the line to red
                lineRenderers[i].startColor = Color.red;
                lineRenderers[i].endColor = Color.red;
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




    // // Destroy the previous ray
    // if (currentRay != null)
    // {
    //     Destroy(currentRay);
    // }

    // Instantiate the ray prefab
    // currentRay = Instantiate(rayPrefab, bulletPoint.position, Quaternion.identity);
    //
    // // Set the positions of the LineRenderer to show the ray
    // LineRenderer lr = currentRay.GetComponent<LineRenderer>();
    // lr.SetPosition(0, bulletPoint.position);
    // lr.SetPosition(1, player.transform.position);

    // Check if the ray intersects with the player
    //if (Vector3.Distance(player.transform.position, bulletPoint.position) < lr.bounds.size.magnitude)
    //{
    //    ShootAtPlayer();
    //}
}




