using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidRotations : MonoBehaviour
{
    [Header("Rotation Speed")]
    public float rotationSpeedX = 30f; // Rotation speed around the X axis (degrees per second)
    public float rotationSpeedY = 50f; // Rotation speed around the Y axis (degrees per second)
    public float rotationSpeedZ = 20f; // Rotation speed around the Z axis (degrees per second)

    // Update is called once per frame
    void Update()
    {
        // Calculate the rotation based on the rotation speeds and deltaTime
        float rotationX = rotationSpeedX * Time.deltaTime;
        float rotationY = rotationSpeedY * Time.deltaTime;
        float rotationZ = rotationSpeedZ * Time.deltaTime;

        // Apply the rotation to the object
        transform.Rotate(rotationX, rotationY, rotationZ, Space.Self);
    }
}

