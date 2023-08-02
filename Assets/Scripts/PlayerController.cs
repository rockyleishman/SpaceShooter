using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseShip
{
    

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        //controller joystick input test
        //Debug.Log("LX: " + Input.GetAxis("LeftStickX") + " LY: " + Input.GetAxis("LeftStickY") + " RX: " + Input.GetAxis("RightStickX") + " RY: " + Input.GetAxis("RightStickY"));

        DirectionalMovement();
    }

    private void DirectionalMovement()
    {
        float moveX = Input.GetAxis("LeftStickX");
        float moveY = Input.GetAxis("LeftStickY");

        if (moveX != 0.0f || moveY != 0.0f)
        {
            //steer ship
            RotationTarget.rotation = Quaternion.Euler(RotationTarget.eulerAngles.x, RotationTarget.eulerAngles.y, Vector2.SignedAngle(Vector2.up, new Vector2(moveX, moveY)));

            //orient ship        
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.SmoothDampAngle(transform.eulerAngles.z, RotationTarget.eulerAngles.z, ref _currentRotationalVelocity, _finalRotationTime, _finalRotationSpeed, Time.deltaTime));

            //accelerate
            _velocity += transform.up * new Vector2(moveX, moveY).magnitude * Time.deltaTime;

            //clamp velocity at max speed
            _velocity = Vector2.ClampMagnitude(_velocity, _finalMaxSpeed);
        }

        if (Input.GetButton("Brake"))
        {
            //manual brake
            _velocity = Vector2.Lerp(_velocity, Vector2.zero, _finalManualBrakingPower * Time.deltaTime);

            Debug.Log("Braking");
        }
        else if (moveX == 0.0f && moveY == 0.0f)
        {
            //inertia brake
            _velocity = Vector2.Lerp(_velocity, Vector2.zero, _finalInertiaBrakingPower * Time.deltaTime);
        }

        //move ship
        transform.Translate(_velocity * Time.deltaTime, Space.World);
    }
}
