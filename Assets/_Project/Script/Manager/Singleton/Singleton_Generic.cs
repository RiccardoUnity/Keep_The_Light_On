using UnityEngine;

public abstract class Singleton_Generic<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null && !_isApplicationQuitting)
            {
                Debug.LogWarning($"-- Nuovo Singleton {typeof(T)} generato --");
                if (_useResources)
                {
                    Instantiate(Resources.Load<GameObject>(_resourcesPath));
                }
                else
                {
                    GameObject gameObject = new GameObject(typeof(T).ToString(), typeof(T));
                    if (_useResources && !_correctInstantiate)
                    {
                        Destroy(gameObject);
                        _correctInstantiate = true;
                        Instantiate(Resources.Load<GameObject>(_resourcesPath));
                    }
                }
            }
            return _instance;
        }
    }

    private static bool _isApplicationQuitting;

    protected abstract bool ShouldBeDestroyOnLoad();

    protected static bool _useResources;
    protected static string _resourcesPath;
    private static bool _correctInstantiate;
    [SerializeField] private bool _bypass;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            if (_useResources && _correctInstantiate || !_useResources || _bypass)
            {
                _instance = this as T;
                if (!ShouldBeDestroyOnLoad())
                {
                    Debug.LogWarning($"-- Singleton {typeof(T)} inserito in DontDestroyOnLoad --");
                    DontDestroyOnLoad(gameObject);
                }
            }
        }
        else
        {
            Destroy(gameObject);
            if (ShouldBeDestroyOnLoad() && transform.parent != null)
            {
                Instance.transform.parent = transform.parent;
            }
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this as T)
        {
            _instance = null;
        }
    }

    void OnApplicationQuit()
    {
        _isApplicationQuitting = true;
    }
}