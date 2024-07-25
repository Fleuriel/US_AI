using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 turn;
    public float movementSpeed = 10f;
    public float turnSpeed = 1.0f;
    //    private float verticalClampAngle = 80.0f; // Adjust this value to set the maximum pitch angle
    private float horizontalTurn;

    public GameObject PlayerPosition;
    public GameObject PositionOne;
    public GameObject PositionTwo;
    public GameObject PositionThree;


    bool Pos1;
    bool Pos2;
    bool Pos3;

    void Start()
    {
    }

    void Update()
    {
        // Get input from WASD keys
        // Get input from WASD keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Get input from mouse for horizontal rotation
        horizontalTurn += Input.GetAxis("Mouse X") * turnSpeed;

        // Update player horizontal rotation
        transform.localRotation = Quaternion.Euler(0, horizontalTurn, 0);

        // Combine input to form movement vector relative to the player's rotation
        Vector3 forward = transform.forward * moveVertical;
        Vector3 right = transform.right * moveHorizontal;
        Vector3 movement = (forward + right).normalized * movementSpeed * Time.deltaTime;

        // Move the player
        transform.Translate(movement, Space.World);


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Pos1 = true;
            Pos3 = Pos2 = false;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Pos2 = true;
            Pos1 = Pos3 = false;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Pos3 = true;
            Pos1 = Pos2 = false;
        }


        if (Pos1)
        {

            PlayerPosition.transform.position = PositionOne.transform.position;
            Pos1 = false;
        }
        if (Pos2)
        {

            PlayerPosition.transform.position = PositionTwo.transform.position;
            Pos2 = false;
        }
        if (Pos3)
        {

            PlayerPosition.transform.position = PositionThree.transform.position;
            Pos3 = false;

        }


    }

}
