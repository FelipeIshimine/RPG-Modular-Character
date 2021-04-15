using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class RuntimeScriptableSingleton<T> : BaseRuntimeScriptableSingleton where T : RuntimeScriptableSingleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            #if UNITY_EDITOR
            if (instance == null)
                instance = UnityEditorExtensions.FindAssetByType(typeof(T)) as T;
            #endif
            
            return instance;
        }
    }

    public override void Initialize()
    {
        Debug.Log($"<color=white> Initialize </color> <color=green> {typeof(T)} </color>");
        instance = this as T;
    }
}

public abstract class BaseRuntimeScriptableSingleton : ScriptableObject
{
    public abstract void Initialize();
}