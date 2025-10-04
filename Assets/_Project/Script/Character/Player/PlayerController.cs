using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringConst = StaticData.S_GameManager.StringConst;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;

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

    #region MyAwake
    private bool _isMyAwake;
    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.LogWarning("MyAwake of the PlayerController has already executed", gameObject);
        }
        else
        {
            _isMyAwake = true;
            _gravityMagnitude = Physics.gravity.magnitude;
            _rb = GetComponent<Rigidbody>();

            PlayerManager.Instance.playerGroundCheck.onGroundedChange += SetCanJump;
        }
    }
    #endregion

    private void SetIsMoveOn(bool value) => _isMoveOn = value;
    private void SetCanJump(bool value) => _canJump = value;
    //private void SetSpeed() => _speed = prendere i parametri in giro

    void Update()
    {
        if (_canJump && !_keyJumpPress)
        {
            _keyJumpPress = Input.GetButton(StringConst.Jump);
        }
    }

    void FixedUpdate()
    {
        if (PlayerManager.Instance.playerGroundCheck.isGrounded)
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

    private void MoveDirection() => _rb.MovePosition(_rb.position + _direction * (_speed * Time.fixedDeltaTime));

    private void Jump()
    {
        _keyJumpPress = false;
        _rb.AddForce(Vector3.up * Mathf.Sqrt(2f * _gravityMagnitude * _heightJump), ForceMode.VelocityChange);
        onJump?.Invoke();
    }
}
