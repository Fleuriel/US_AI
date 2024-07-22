using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public Vector2 turn;
    public float movementSpeed = 10f;
    public float turnSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from WASD keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        

        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");


        // Combine input to form movement vector
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        // Normalize vector to ensure consistent speed in all directions
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        // Move the player
        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
        transform.localRotation = Quaternion.Euler(-turn.y * turnSpeed, turn.x * turnSpeed, 0);
    }
}
