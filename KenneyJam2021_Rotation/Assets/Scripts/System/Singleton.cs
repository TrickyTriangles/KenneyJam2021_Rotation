using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get { return instance; }
    }

    private Action InitializationFailedCallback;
    protected void Subscribe_InitializationFailedCallback(Action sub) { InitializationFailedCallback += sub; }
    protected void Unsubscribe_InitializationFailedCallback(Action sub) { InitializationFailedCallback -= sub; }

    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("[Singleton] Trying to instantiate a second instance of a singleton class.");
            InitializationFailedCallback?.Invoke();
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}