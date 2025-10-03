using UnityEngine;

public abstract class SingletonGeneric<T> : MonoBehaviour where T : MonoBehaviour
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
                }
            }
            return _instance;
        }
    }

    private static bool _isApplicationQuitting;

    protected abstract bool ShouldBeDestroyOnLoad();

    protected static bool _useResources;
    protected static string _resourcesPath;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (!ShouldBeDestroyOnLoad())
            {
                Debug.LogWarning($"-- Singleton {typeof(T)} inserito in DontDestroyOnLoad --");
                DontDestroyOnLoad(gameObject);
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