using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringConst = StaticData.S_GameManager.StringConst;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerManager _playerManager;
    private UI_Option _option;

    private bool _isMoveOn = true;
    private float _right;
    private float _forward;
    private float _lenght;
    private Vector3 _direction;
    [SerializeField] private float _speed = 4f;

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
        }
    }

    private void SetIsMoveOn(bool value) => _isMoveOn = value;
    private void SetCanJump(bool value) => _canJump = value;
    //private void SetSpeed() => _speed = prendere i parametri in giro

    void Update()
    {
        if (_canJump && !_keyJumpPress)
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

            //Player
            _yaw += _mouseX * Time.deltaTime;
            transform.eulerAngles = new Vector3(0f, _yaw, 0f);
        }
    }

    void FixedUpdate()
    {
        if (_playerManager.PlayerGroundCheck.isGrounded)
        {
            if (_keyJumpPress)
            {
                Jump();
            }
            else if (_isMoveOn)
            {
                Move();
            }
        }
        else
        {
            MoveDirection();
        }
    }

    private void Move()
    {
        _right = Input.GetAxis(StringConst.Horizontal);
        _forward = Input.GetAxis(StringConst.Vertical);

        _lenght = new Vector3(_right, 0f, _forward).sqrMagnitude;
        if (_lenght > 1f)
        {
            _lenght = Mathf.Sqrt(_lenght);
            _right /= _lenght;
            _forward /= _lenght;
        }
        _direction = new Vector3(_right, 0f, _forward);

        MoveDirection();
    }

    private void MoveDirection() => _rb.MovePosition(_rb.position + transform.rotation * _direction * (_speed * Time.fixedDeltaTime));

    private void Jump()
    {
        _keyJumpPress = false;
        _rb.AddForce(Vector3.up * Mathf.Sqrt(2f * _gravityMagnitude * _heightJump), ForceMode.VelocityChange);
        onJump?.Invoke();
    }
}
