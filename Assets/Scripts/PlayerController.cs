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
    [Header("Targeting")]
    [SerializeField] public Transform Reticle;
    private Vector2 _aimDirection;
    private Vector3 _currentRecticleVelocity; //used by Vector3.SmoothDamp
    [SerializeField] public LineRenderer AimLine;
    [Space(5)]
    [SerializeField] [Range(0.0f, 20.0f)] float DefaultReticleDistance = 2.5f;
    [SerializeField] [Range(0.0f, 20.0f)] float MaxReticleDistance = 10.0f;
    [SerializeField] [Range(0.0f, 1.0f)] float ReticleResetTime = 0.1f;
    [SerializeField] [Range(0.0f, 1.0f)] float AimLineDotSize = 0.1f;

    [Header("Controller Game Pad")]
    [SerializeField] GamePadType ControllerType = GamePadType.None;
    private string _moveXAxis;
    private string _moveYAxis;
    private string _aimXAxis;
    private string _aimYAxis;
    private string _brakeAxis;
    private string _boostAxis;
    private string _fire1Axis;
    private string _fire2Axis;
    private string _swapAxis;
    private string _pauseAxis;
    private string _menuSelectAxis;
    private string _menuBackAxis;
    private string _menuNavXAxis;
    private string _menuNavYAxis;

    protected override void Start()
    {
        base.Start();

        //init velocity and max speed
        _velocity = Vector2.zero;
        _currentMaxSpeed = _finalMaxSpeed;

        //init aim direction
        _aimDirection = Vector2.up;

        //init aim line
        AimLine.material.mainTextureScale = new Vector2(1.0f / AimLineDotSize, 1.0f);
        AimLine.startWidth = AimLineDotSize;

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
            _brakeAxis = "Brake_PS";
            _boostAxis = "Boost_PS";
            _fire1Axis = "Fire1_PS";
            _fire2Axis = "Fire2_PS";
            _swapAxis = "Swap_PS";
            _pauseAxis = "Pause_PS";
            _menuSelectAxis = "MenuSelect_PS";
            _menuBackAxis = "MenuBack_PS";
            _menuNavXAxis = "MenuNavX_PS";
            _menuNavYAxis = "MenuNavY_PS";
        }
        else if (ControllerType == GamePadType.XBox)
        {
            _moveXAxis = "MoveX_XB";
            _moveYAxis = "MoveY_XB";
            _aimXAxis = "AimX_XB";
            _aimYAxis = "AimY_XB";
            _brakeAxis = "Brake_XB";
            _boostAxis = "Boost_XB";
            _fire1Axis = "Fire1_XB";
            _fire2Axis = "Fire2_XB";
            _swapAxis = "Swap_XB";
            _pauseAxis = "Pause_XB";
            _menuSelectAxis = "MenuSelect_XB";
            _menuBackAxis = "MenuBack_XB";
            _menuNavXAxis = "MenuNavX_XB";
            _menuNavYAxis = "MenuNavY_XB";
        }
        else if (ControllerType == GamePadType.NintendoSwitchProController)
        {
            _moveXAxis = "MoveX_NSP";
            _moveYAxis = "MoveY_NSP";
            _aimXAxis = "AimX_NSP";
            _aimYAxis = "AimY_NSP";
            _brakeAxis = "Brake_NSP";
            _boostAxis = "Boost_NSP";
            _fire1Axis = "Fire1_NSP";
            _fire2Axis = "Fire2_NSP";
            _swapAxis = "Swap_NSP";
            _pauseAxis = "Pause_NSP";
            _menuSelectAxis = "MenuSelect_NSP";
            _menuBackAxis = "MenuBack_NSP";
            _menuNavXAxis = "MenuNavX_NSP";
            _menuNavYAxis = "MenuNavY_NSP";
        }
        else
        {
            _moveXAxis = "MoveX_KBM";
            _moveYAxis = "MoveY_KBM";
            _aimXAxis = "AimX_KBM";
            _aimYAxis = "AimY_KBM";
            _brakeAxis = "Brake_KBM";
            _boostAxis = "Boost_KBM";
            _fire1Axis = "Fire1_KBM";
            _fire2Axis = "Fire2_KBM";
            _swapAxis = "Swap_KBM";
            _pauseAxis = "Pause_KBM";
            _menuSelectAxis = "Null";
            _menuBackAxis = "Null";
            _menuNavXAxis = "Null";
            _menuNavYAxis = "Null";
        }
    }

    private void Update()
    {
        DirectionalMovement();
        Aim();
        FireWeapons();
    }

    private void DirectionalMovement()
    {
        //boost
        if (Input.GetAxis(_boostAxis) > 0.0f && _currentBoostPoints > 0)
        {
            //use boost
            _currentAcceleration = _finalAcceleration * _finalBoostMultiplier;
            _currentMaxSpeed = _finalMaxSpeed * _finalBoostMultiplier;
            _currentBoostPoints -= Time.deltaTime;
        }
        else
        {
            //not using boost
            _currentAcceleration = _finalAcceleration;
            //boost decay
            _currentMaxSpeed = Mathf.SmoothDamp(_currentMaxSpeed, _finalMaxSpeed, ref _currentBoostDecayVelocity, _finalBoostDecayTime);
        }

        //regenerate boost
        if (Input.GetAxis(_boostAxis) <= 0.0f)
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

        float moveX = Input.GetAxis(_moveXAxis);
        float moveY = Input.GetAxis(_moveYAxis);

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
        if (Input.GetAxis(_brakeAxis) > 0.0f)
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

    private void Aim()
    {
        if (ControllerType == GamePadType.None)
        {
            //aim with reticle
            Reticle.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Reticle.position = new Vector3(Reticle.position.x, Reticle.position.y, 0.0f);

            //update direction
            _aimDirection = Reticle.localPosition.normalized;
        }
        else
        {
            //aim with direction
            Vector2 direction = new Vector2(Input.GetAxis(_aimXAxis), Input.GetAxis(_aimYAxis));
            if (direction != Vector2.zero)
            {
                //change direction with right stick
                _aimDirection = direction.normalized;

                //update reticle
                Reticle.transform.position = transform.position + new Vector3(direction.x, direction.y, 0.0f) * MaxReticleDistance;
            }
            else
            {
                //return to standard position
                Reticle.transform.localPosition = Vector3.SmoothDamp(Reticle.transform.localPosition, Vector3.up * DefaultReticleDistance, ref _currentRecticleVelocity, ReticleResetTime);
            }
        }

        //unrotate reticle
        Reticle.transform.rotation = Quaternion.Euler(Vector3.zero);

        //draw aim line
        AimLine.SetPosition(0, new Vector3(Reticle.transform.position.x, Reticle.transform.position.y, -90.0f));
        AimLine.SetPosition(1, new Vector3(this.transform.position.x, this.transform.position.y, -90.0f));
    }

    private void FireWeapons()
    {
        //fire primary weapon
        if (Input.GetAxis(_fire1Axis) > 0.0f)
        {
            Fire1();
        }

        //fire secondary weapon (special)
        if (Input.GetAxis(_fire2Axis) > 0.0f)
        {
            Fire2();
        }
    }
}
