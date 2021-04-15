using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ManagerInitializer : MonoSingleton<ManagerInitializer>
{
    [AssetList(AutoPopulate = true)] public List<BaseRuntimeScriptableSingleton> managers = new List<BaseRuntimeScriptableSingleton>();
    public Dictionary<Type, BaseRuntimeScriptableSingleton> _managers = new Dictionary<Type, BaseRuntimeScriptableSingleton>();

    protected override void Awake()
    {
        base.Awake();
        foreach (BaseRuntimeScriptableSingleton singleton in managers)
        {
            var t = singleton.GetType();
            _managers.Add(t, singleton);
            singleton.Initialize();
        }
    }

    public static T Get<T>() where T : BaseRuntimeScriptableSingleton
    {
        var t = typeof(T);
        Debug.Log($"Get: {t}");
        return Instance._managers[t] as T; 
    }
}