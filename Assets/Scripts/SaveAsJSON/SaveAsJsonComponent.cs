using System;
using Leguar.TotalJSON;
using UnityEngine;

public class SaveAsJsonComponent : MonoBehaviour, ISaveAsJson
{
    public SaveAsJsonModule saveAsJsonModule;

    public string Key => saveAsJsonModule.Key;
    public int Version => saveAsJsonModule.Version;
    public JSON Save() => saveAsJsonModule.Save();
    public void Load(JSON data)=> saveAsJsonModule.Load(data);
    public void UpdateSaveData(JSON data) => saveAsJsonModule.UpdateSaveData(data);

    public void Register(ISaveAsJson saveAsJson) => saveAsJsonModule.Register(saveAsJson);
    public void Unregister(ISaveAsJson saveAsJson) => saveAsJsonModule.Unregister(saveAsJson);

    
}