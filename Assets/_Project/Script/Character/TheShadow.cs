using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaticData;
using GWM = GameWorldManager;

[RequireComponent(typeof(Rigidbody))]
public class TheShadow : MonoBehaviour
{
    private bool _isMyAwake;
    [SerializeField] private bool _startEnable;

    private PlayerManager _playerManager;
    private Rigidbody _rb;
    private const float _startDistanceFromPlayer = 10f;
    private const float _maxHeightCheck = 100f;
    private Quaternion _deltaRotation;

    private bool _canMove;
    private float _speed = 2f;
    private float _deltaSpeed;
    private float _distanceSqr;
    private const float _minDistanceSqr = 2f;
    private Vector3 _direction;
    private RaycastHit _hit;
    private Campfire _campfire;
    private Vector3 _playerProiect;

    private IEnumerator _addModifierTime;
    private float _timeModifier = 1f;
    private WaitForSeconds _time;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            _rb = GetComponent<Rigidbody>();
            _playerManager = GWM.Instance.PlayerManager;
            SpawnPoint();
            _deltaRotation = Quaternion.Euler(0f, -90f, 0f);
            gameObject.SetActive(_startEnable);
            _canMove = _startEnable;

            GWM.Instance.TimeManager.onNight += WakeUp;
            GWM.Instance.TimeManager.onDawn += Sleep;
            _time = new WaitForSeconds(_timeModifier);
        }
    }

    void OnEnable()
    {
        if (_isMyAwake)
        {
            _canMove = true;
            _rb.velocity = Vector3.zero;
            SpawnPoint();
        }
    }
    void OnDisable() => _canMove = false;

    private void WakeUp() => gameObject.SetActive(true);
    private void Sleep() => gameObject.SetActive(false);

    private void SpawnPoint()
    {
        do
        {
            transform.position = _playerManager.transform.position * Random.value * _startDistanceFromPlayer;
            Physics.Linecast(transform.position + Vector3.up * _maxHeightCheck, transform.position + Vector3.down * _maxHeightCheck, out _hit, GWM.Instance.GroundLayerMask, GWM.Instance.Qti);
        }
        while (_hit.point == Vector3.zero);
        transform.position = _hit.point;
    }

    private void OnTriggerEnter(Collider other)
    {
        _campfire = other.GetComponent<Campfire>();
    }

    private void OnTriggerExit(Collider other)
    {
        Campfire campfire = other.GetComponent<Campfire>();
        if (campfire == _campfire)
        {
            _campfire = null;
        }
    }

    void FixedUpdate()
    {
        if (_canMove)
        {
            //Speed Limit
            if (_rb.velocity.sqrMagnitude > 10f)
            {
                SpawnPoint();
            }

            //Horizontal
            _deltaSpeed = _speed * Time.fixedDeltaTime * GWM.Instance.TimeManager.AccelerationMoltiplier;
            _direction = _playerManager.transform.position - transform.position;
            if (_campfire != null)
            {
                if (_campfire.IsOn)
                {
                    MoveAroundCampfire(_campfire.transform.position);   
                }
                else
                {
                    Move();
                }
            }
            else
            {
                Move();
            }

            //Rotation
            transform.LookAt(_playerManager.transform.position);
            transform.rotation *= _deltaRotation;
        }
    }

    private void MoveAroundCampfire(Vector3 obstaclePosition)
    {
        _direction.Normalize();
        _playerProiect = _playerManager.transform.position - _direction * Vector3.Dot(_playerManager.transform.position - obstaclePosition, _direction);
        _direction = (_playerProiect - obstaclePosition);
        _rb.MovePosition(_rb.position + _direction * _deltaSpeed);
    }

    private void Move()
    {
        _distanceSqr = _direction.sqrMagnitude;
        if (_distanceSqr < _minDistanceSqr)
        {
            PlayerLoseLife();
        }
        else
        {
            _direction.Normalize();
            _rb.MovePosition(_rb.position + _direction * _deltaSpeed);
        }
    }

    private void PlayerLoseLife()
    {
        if (_addModifierTime == null)
        {
            Debug.Log("Qui");
            _addModifierTime = AddModifierTime();
            StartCoroutine(_addModifierTime);
        }
    }

    private IEnumerator AddModifierTime()
    {
        _playerManager.Life.AddModifier(0.05f, false, 1f);
        yield return _time;
        _addModifierTime = null;
    }
}
