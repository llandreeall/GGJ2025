using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    public bool Persistent = true;
    public static bool Exists { get { return instance != null; } private set { Debug.LogError("You cannot set this variable directly"); } }
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject()
                    {
                        name = typeof(T).Name
                    };
                    instance = obj.AddComponent<T>();
                }
                Debug.Log("created an instance of " + typeof(T).Name);
            }
            return instance;
        }
    }

    public bool Initialized { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            if (Persistent)
                DontDestroyOnLoad(gameObject);
            Initialized = Initialize();
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("Called to initialize a singleton a second time. Destroying " + name);
        }
    }

    public void Destroy()
    {
        Destroy(this);
        Destroy(this.gameObject);
        instance = null;
    }

    /// <summary>
    /// Any Singleton initialization routines that need to be done should be done here. This is a mandatory method
    /// </summary>
    protected abstract bool Initialize();

}
