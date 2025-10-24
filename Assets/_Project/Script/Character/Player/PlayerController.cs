using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringConst = StaticData.S_GameManager.StringConst;
using GWM = GameWorldManager;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerManager _playerManager;
    private UI_Option _option;

    public bool IsRun { get; private set; }
    private bool _isMoveOn = true;
    private float _right;
    private float _forward;
    private float _lenght;
    private Vector3 _direction;
    [SerializeField] private float _walkSpeed = 4f;
    [SerializeField] private float _runSpeed = 6f;

    [SerializeField] private float _heightJump = 1f;
    private float _gravityMagnitude;
    private bool _keyJumpPress;
    private bool _canJump = true;
    public event Action onJump;

    private float _mouseX;
    private float _mouseY;
    private float _pitch;   //Rotation long axis X
    private float _yaw;     //Rotation long axis Y
    private const float _sensitivityX = 800f;
    private const float _sensitivityY = 400f;
    [SerializeField] private float _pitchLimit = 80f;

    public float WalkTime { get; private set; }
    private float _walkTimeProcessed;
    public float RunTime { get; private set; }
    private float _runTimeProcessed;
    public int JumpNumber { get; private set; }
    private int _jumpNumberProcessed;

    public float EnergyToProcess { get; private set; }

    private bool _isMyAwake;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;
            _gravityMagnitude = Physics.gravity.magnitude;
            _rb = GetComponent<Rigidbody>();
            _playerManager = GameWorldManager.Instance.PlayerManager;
            _option = GameWorldManager.Instance.UIPause.UIOption;

            _playerManager.PlayerGroundCheck.onGroundedChange += SetCanJump;
            GWM.Instance.TimeManager.onPriority += EnergyToProcessInNormalPriority;
        }
    }

    private void SetIsMoveOn(bool value) => _isMoveOn = value;
    private void SetCanJump(bool value) => _canJump = value;
    //private void SetSpeed() => _speed = prendere i parametri in giro

    void Update()
    {
        if (GWM.Instance.IsGamePause)
        {

        }
        else
        {
            if (GWM.Instance.UIInventory.gameObject.activeSelf)
            {
                _right = 0f;
                _forward = 0f;
            }
            else
            {
                if (_canJump && !_keyJumpPress && _playerManager.Stamina.Value > _playerManager.Stamina.DecreaseJump)
                {
                    _keyJumpPress = Input.GetButton(StringConst.Jump);
                }

                //Rotation Input
                _mouseX = Input.GetAxis(StringConst.MouseX) * _sensitivityX * _option.MouseSensitivity * (_option.InvertMouseX ? -1 : 1);
                _mouseY = Input.GetAxis(StringConst.MouseY) * _sensitivityY * _option.MouseSensitivity * (_option.InvertMouseY ? -1 : 1);

                if (_mouseX != 0f || _mouseY != 0f)
                {
                    //Camera
                    _pitch += _mouseY * Time.deltaTime;
                    _pitch = Mathf.Clamp(_pitch, -_pitchLimit, _pitchLimit);
                    _playerManager.Camera.transform.eulerAngles = new Vector3(_pitch, _yaw, 0f);
                    _playerManager.Head.rotation = _playerManager.Camera.transform.rotation;

                    //Player
                    _yaw += _mouseX * Time.deltaTime;
                    transform.eulerAngles = new Vector3(0f, _yaw, 0f);
                }

                //MoveInput
                _right = Input.GetAxis(StringConst.Horizontal);
                _forward = Input.GetAxis(StringConst.Vertical);
                if (Input.GetButton(StringConst.Run) && _playerManager.Stamina.Value > 0f)
                {
                    IsRun = true;
                }
                else
                {
                    IsRun = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (GWM.Instance.IsGamePause)
        {

        }
        else
        {
            if (_playerManager.PlayerGroundCheck.isGrounded)
            {
                if (_keyJumpPress)
                {
                    Jump();
                }
                else if (_isMoveOn)
                {
                    DirectionMove();
                }
            }
            else
            {
                Move();
            }
        }
    }

    private void DirectionMove()
    {
        if (_right != 0f || _forward != 0f)
        {
            _lenght = new Vector3(_right, 0f, _forward).sqrMagnitude;
            if (_lenght > 1f)
            {
                _lenght = Mathf.Sqrt(_lenght);
                _right /= _lenght;
                _forward /= _lenght;
            }
            _direction = new Vector3(_right, 0f, _forward);

            Move();
        }
        else
        {
            _direction = Vector3.zero;
        }
    }

    private void Move()
    {
        if (IsRun)
        {
            RunTime += Time.fixedDeltaTime;
            Run();
        }
        else
        {
            WalkTime += Time.fixedDeltaTime;
            Walk();
        }
    }

    private void Walk() => _rb.MovePosition(_rb.position + transform.rotation * _direction * (_walkSpeed * Time.fixedDeltaTime));
    private void Run() => _rb.MovePosition(_rb.position + transform.rotation * _direction * (_runSpeed * Time.fixedDeltaTime));

    private void Jump()
    {
        _keyJumpPress = false;
        ++JumpNumber;
        _rb.AddForce(Vector3.up * Mathf.Sqrt(2f * _gravityMagnitude * _heightJump), ForceMode.VelocityChange);
        onJump?.Invoke();
    }

    private void EnergyToProcessInNormalPriority(float timeDelay)
    {
        //Jump
        EnergyToProcess = (JumpNumber - _jumpNumberProcessed) * 4;
        _jumpNumberProcessed = JumpNumber;
        //Walk
        EnergyToProcess += WalkTime - _walkTimeProcessed;
        _walkTimeProcessed = WalkTime;
        //Run
        EnergyToProcess += (RunTime - _runTimeProcessed) * 2f;
        _runTimeProcessed = RunTime;
    }
}
