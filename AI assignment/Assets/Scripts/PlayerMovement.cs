using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 turn;
    public float movementSpeed = 10f;
    public float turnSpeed = 1.0f;

    public GameObject PlayerPosition;
    public GameObject PositionOne;
    public GameObject PositionTwo;
    public GameObject PositionThree;


    void Start()
    {
    }

    void Update()
    {
        // Get input from WASD keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Get input from mouse for rotation
        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");

        // Update player rotation
        transform.localRotation = Quaternion.Euler(-turn.y * turnSpeed, turn.x * turnSpeed, 0);

        // Combine input to form movement vector relative to the player's rotation
        Vector3 forward = transform.forward * moveVertical;
        Vector3 right = transform.right * moveHorizontal;
        Vector3 movement = (forward + right).normalized * movementSpeed * Time.deltaTime;

        // Move the player
        transform.Translate(movement, Space.World);




        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerPosition.transform.position = PositionOne.transform.position;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerPosition.transform.position = PositionTwo.transform.position;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerPosition.transform.position = PositionThree.transform.position;
        }
    }
}
