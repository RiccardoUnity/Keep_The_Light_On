using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;

public class PlayerGroundCheck : MonoBehaviour
{
    private PlayerManager _playerManager;
    private bool _isFrameCheck;
    private float _radius = 0.1f;
    private Vector3 _offset = new Vector3(0f, 0.075f, 0f);
    [SerializeField] private LayerMask _groundLayerMask = (1 << 0);
    private QueryTriggerInteraction _qti = QueryTriggerInteraction.Ignore;

    private bool _isJumpStart;
    public bool isGrounded { get; private set; }
    public event Action<bool> onGroundedChange;

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

            _playerManager = GameWorldManager.Instance.PlayerManager;
            _playerManager.PlayerController.onJump += SetIsInJump;
        }
    }

    private void SetIsInJump()
    {
        if (!_isJumpStart)
        {
            _isJumpStart = true;
            isGrounded = false;
            onGroundedChange?.Invoke(isGrounded);
        }
    }

    void Update()
    {
        if (GWM.Instance.IsGamePause)
        {

        }
        else
        {
            if (_isFrameCheck)
            {
                if (Physics.CheckSphere(transform.position + _offset, _radius, _groundLayerMask, _qti))
                {
                    if (!isGrounded && !_isJumpStart)
                    {
                        isGrounded = true;
                        onGroundedChange?.Invoke(isGrounded);
                    }
                }
                else
                {
                    if (_isJumpStart)
                    {
                        _isJumpStart = false;
                    }
                    if (isGrounded)
                    {
                        isGrounded = false;
                        onGroundedChange?.Invoke(isGrounded);
                    }
                }
            }
            else
            {
                //In this frame, the ground doesn't check
            }
            _isFrameCheck = !_isFrameCheck;
        }
    }
}
