using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAsJsonComponentLinker : MonoBehaviour
{
    public SaveAsJsonComponent target;

    private readonly List<ISaveAsJson> _saveAsJsons = new List<ISaveAsJson>();
    private void Awake()
    {
        GetComponents<ISaveAsJson>(_saveAsJsons);
        foreach (ISaveAsJson saveAsJson in _saveAsJsons)
            if(!ReferenceEquals(saveAsJson, target))
                target.Register(saveAsJson);
    }

    private void OnDestroy()
    {
        foreach (ISaveAsJson saveAsJson in _saveAsJsons)
            if(!ReferenceEquals(saveAsJson, target))
                target.Unregister(saveAsJson); 
    }
}
