using System;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;
    public static T Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance != null)
            throw new Exception($"MonoSingleton duplicate {this}");
        _instance = this as T;
    }
}