using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  RotaryHeart.Lib;
using Sirenix.OdinInspector;

public class BonesIndex : MonoBehaviour
{
    public StringToTransformDictionary dictionary = new StringToTransformDictionary();

    public Transform this[string key] => dictionary[key];
    
    [Button]
    public void Scan()
    {
        dictionary.Clear();
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
            dictionary.Add(child.transform.name,child.transform);
    }
    
}