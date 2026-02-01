using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected bool markedToDestroy = false;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            markedToDestroy = true;
            return;
        }

        PreserveSingleton(this as T);
    }

    protected static void PreserveSingleton(T singleton)
    {
        Instance = singleton;
        singleton.transform.parent = null;
        DontDestroyOnLoad(singleton);
    }

    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}