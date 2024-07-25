using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float turnSpeed = 5.0f;
    public float verticalClampAngle = 80.0f;

    private float verticalTurn;

    void Update()
    {
        // Get input from mouse for vertical rotation
        verticalTurn -= Input.GetAxis("Mouse Y") * turnSpeed;

        // Clamp vertical rotation to avoid over-rotation
        verticalTurn = Mathf.Clamp(verticalTurn, -verticalClampAngle, verticalClampAngle);

        // Update camera vertical rotation
        transform.localRotation = Quaternion.Euler(verticalTurn, 0, 0);
    }
}
