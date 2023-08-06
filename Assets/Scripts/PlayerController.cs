using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePadType
{
    None,
    DualShock4,
    DualSense,
    XBox,
    NintendoSwitchProController
}

public class PlayerController : BaseShip
{
    [Header("Controller Game Pad")]
    [SerializeField] GamePadType ControllerType = GamePadType.None;
    private string _moveXAxis;
    private string _moveYAxis;
    private string _aimXAxis;
    private string _aimYAxis;
    private string _breakAxis;
    private string _boostAxis;
    private string _fire1Axis;
    private string _fire2Axis;
    private string _pauseAxis;
    private string _selectAxis;
    private string _backAxis;

    protected override void Start()
    {
        base.Start();

        //init velocity and max speed
        _velocity = Vector2.zero;
        _currentMaxSpeed = _finalMaxSpeed;

        MapGamePad();
    }

    private void MapGamePad()
    {
        if (ControllerType == GamePadType.DualShock4 || ControllerType == GamePadType.DualSense)
        {
            _moveXAxis = "MoveX_PS";
            _moveYAxis = "MoveY_PS";
            _aimXAxis = "AimX_PS";
            _aimYAxis = "AimY_PS";
            _breakAxis = "Break_PS";
            _boostAxis = "Boost_PS";
            _fire1Axis = "Null";
            _fire2Axis = "Null";
            _pauseAxis = "Null";
            _selectAxis = "Null";
            _backAxis = "Null";
        }
        else if (ControllerType == GamePadType.XBox)
        {
            _moveXAxis = "MoveX_XB";
            _moveYAxis = "MoveY_XB";
            _aimXAxis = "AimX_XB";
            _aimYAxis = "AimY_XB";
            _breakAxis = "Break_XB";
            _boostAxis = "Boost_XB";
            _fire1Axis = "Null";
            _fire2Axis = "Null";
            _pauseAxis = "Null";
            _selectAxis = "Null";
            _backAxis = "Null";
        }
        else if (ControllerType == GamePadType.NintendoSwitchProController)
        {
            _moveXAxis = "MoveX_NSP";
            _moveYAxis = "MoveY_NSP";
            _aimXAxis = "AimX_NSP";
            _aimYAxis = "AimY_NSP";
            _breakAxis = "Break_NSP";
            _boostAxis = "Boost_NSP";
            _fire1Axis = "Null";
            _fire2Axis = "Null";
            _pauseAxis = "Null";
            _selectAxis = "Null";
            _backAxis = "Null";
        }
        else
        {
            _moveXAxis = "Null";
            _moveYAxis = "Null";
            _aimXAxis = "Null";
            _aimYAxis = "Null";
            _breakAxis = "Null";
            _boostAxis = "Null";
            _fire1Axis = "Null";
            _fire2Axis = "Null";
            _pauseAxis = "Null";
            _selectAxis = "Null";
            _backAxis = "Null";
        }
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

        //boost
        if (Input.GetButton("Boost") && _currentBoostPoints > 0)
        {
            //use boost
            _currentAcceleration = _finalAcceleration * _finalBoostMultiplier;
            _currentMaxSpeed = _finalMaxSpeed * _finalBoostMultiplier;
            _currentBoostPoints -= Time.deltaTime;

            Debug.Log("Boosting");////////
        }
        else
        {
            //not using boost
            _currentAcceleration = _finalAcceleration;
            //boost decay
            _currentMaxSpeed = Mathf.SmoothDamp(_currentMaxSpeed, _finalMaxSpeed, ref _currentBoostDecayVelocity, _finalBoostDecayTime);
        }

        //regenerate boost
        if (!Input.GetButton("Boost"))
        {
            if (_currentBoostPoints < _finalMaxBoostPoints)
            {
                float boostPercentUp = _finalMaxBoostPoints * _finalMinBoostPercentRegen / 100.0f * Time.deltaTime;
                float boostPointsUp = _finalMinBoostPointsRegen * Time.deltaTime;
                if (boostPercentUp > boostPointsUp)
                {
                    _currentBoostPoints += boostPercentUp;
                }
                else
                {
                    _currentBoostPoints += boostPointsUp;
                }
                //clamp at max
                if (_currentBoostPoints > _finalMaxBoostPoints)
                {
                    _currentBoostPoints = _finalMaxBoostPoints;
                }
            }            
        }

        if (moveX != 0.0f || moveY != 0.0f)
        {
            //steer ship
            Helm.rotation = Quaternion.Euler(Helm.eulerAngles.x, Helm.eulerAngles.y, Vector2.SignedAngle(Vector2.up, new Vector2(moveX, moveY)));

            //orient ship        
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.SmoothDampAngle(transform.eulerAngles.z, Helm.eulerAngles.z, ref _currentTurningVelocity, _finalMinTurningTime, _finalMaxTurningSpeed * k_turningSpeedDegreeModifier, Time.deltaTime));

            //accelerate
            _velocity += transform.up * new Vector2(moveX, moveY).magnitude * _currentAcceleration * Time.deltaTime;

            //clamp velocity at max speed
            _velocity = Vector2.ClampMagnitude(_velocity, _currentMaxSpeed);
        }

        //brake
        if (Input.GetButton("Brake"))
        {
            //manual brake
            _velocity = Vector2.Lerp(_velocity, Vector2.zero, _finalManualBrakingPower * Time.deltaTime);
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
