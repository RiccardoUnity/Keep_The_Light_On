using UnityEngine;
using GWM = GameWorldManager;

//View of Item in the scene
public class Prefab_Item : Interactable
{
    [SerializeField] private bool _debug;
    private bool _hasAnError;
    private int _key;

    public Data_Item DataItem { get => _dataItem; }
    private Data_Item _dataItem;

    public SO_Item SOItem { get => _soItem; }
    [SerializeField] private SO_Item _soItem;

    [SerializeField] private Vector3 _offsetLeaves;
    [SerializeField] private bool _startOnGround = true;

    [Header("Only New Game")]
    [Range(0f, 1f)][SerializeField] private float _limitConditionZero = 0.5f;
    [Range(0f, 1f)][SerializeField] private float _limitConditionOne = 1f;
    private float _condition;
    [SerializeField] private ItemState _state = ItemState.New;

    private RaycastHit _hit;
    void Awake()
    {
        if (SOItem == null)
        {
            _hasAnError = true;
            Debug.LogError("SO_Item missing", gameObject);
        }
    }

    //New Game or Load Game or Instantiate
    void Start()
    {
        if (!_hasAnError)
        {
            //Load Game
            if (S_SaveSystem.HasALoading)
            {
                GWM.Instance.PoolManager.AddPrefabItemToPool(this, _key);
            }
            else
            {
                //New Game
                if (_dataItem == null)
                {
                    if (_debug)
                    {
                        Debug.Log($"New Game - Start - {typeof(Prefab_Item)}", gameObject);
                    }
                    if (_limitConditionZero < _limitConditionOne)
                    {
                        _condition = Random.Range(_limitConditionZero, _limitConditionOne);
                    }
                    else
                    {
                        _condition = Random.Range(_limitConditionOne, _limitConditionZero);
                    }

                    _dataItem = GWM.Instance.PoolManager.RemoveDataItemFromPool(SOItem, _condition, _state, this);
                    if (_debug && _key == 0)
                    {
                        Debug.LogError("Key not assigned", gameObject);
                    }
                }
                //Instantiate
                else
                {

                }
            }
            if (_startOnGround)
            {
                Leaves();
            }
            if (_debug)
            {
                Debug.Log($"End Start - {typeof(Prefab_Item)}", gameObject);
            }
        }
    }

    //After OutPool or before the Start of Unity after Instantiate
    public void MyAwake(Data_Item dataItem, int key)
    {
        if (_dataItem == null)
        {
            _dataItem = dataItem;
            _key = key;
            //Ricordati che va spostato!!!
        }
        if (_debug)
        {
            Debug.Log("MyAwake complete", gameObject);
        }
    }

    #region Pooling
    public bool InPool(int key)
    {
        if (key == _key)
        {
            gameObject.SetActive(false);
            _dataItem = null;
            _key = 0;
            return true;
        }
        return false;
    }

    public void OutPool()
    {
        if (_key == 0)
        {
            gameObject.SetActive(true);
        }
    }
    #endregion

    public void Leaves()
    {
        Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out _hit, 1f, GWM.Instance.GroundLayerMask, GWM.Instance.Qti);
        transform.position = _hit.point + _offsetLeaves;
    }
}