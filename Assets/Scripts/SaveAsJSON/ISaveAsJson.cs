using System;
using System.Collections;
using System.Collections.Generic;
using Leguar.TotalJSON;
using UnityEngine;

public interface ISaveAsJson
{
    public string Key { get; }
    public int Version { get; }
    JSON Save();
    void Load(JSON data);
    void UpdateSaveData(JSON data);
}

[System.Serializable]
public class SaveAsJsonModule : ISaveAsJson
{
    private event Action<JSON> OnLoad;
    private event Action<JSON> OnSave;

    public event Action<JSON> OnLoadDone; 
    public event Action<JSON> OnSaveDone;

    [SerializeField] private string key; 
    [SerializeField] private int version;
    public string Key => key;
    public int Version => version;

    public SaveAsJsonModule() { }
    public SaveAsJsonModule(string key)
    {
        this.key = key;
    }
    
    public SaveAsJsonModule(string key, int version)
    {
        this.key = key;
        this.version = version;
    }
    
    public void Register(ISaveAsJson target)
    {
        OnLoad += target.LoadData;
        OnSave += target.SaveData;
    }

    public void Unregister(ISaveAsJson target)
    {
        OnLoad -= target.LoadData;
        OnSave -= target.SaveData;
    }
    
    public JSON Save()
    {
        JSON data = new JSON();
        OnSave?.Invoke(data);
        OnSaveDone?.Invoke(data);
        return data;
    }
     
    public void Load(JSON data)
    {
        OnLoad?.Invoke(data);
        OnLoadDone?.Invoke(data);
    }

    public void UpdateSaveData(JSON data)
    {
        throw new NotImplementedException();
    }
}


/// <summary>
/// Verifica si los datos requiere actualizacion y si los tienen ejecuta la funcion UpdateSaveData
/// </summary>
public static class ISaveAsJsonUtilities
{
    public const string VersionKey = "VERSION";

    public static bool IsOldVersion(this ISaveAsJson source, JSON data)
    {
        if (!data.ContainsKey(VersionKey))
            throw new KeyNotFoundException($"El archivo NO contiene la VersionKey '{VersionKey}' para el tipo de archivo: {source.GetType()}");

        int fileVersion = data.GetInt(VersionKey);
        return fileVersion < source.Version;
    }

    public static void SaveData(this ISaveAsJson source, JSON mainData)
    {
        JSON saveData = source.Save();
        saveData.AddOrReplace(VersionKey, source.Version);
        mainData.Add(source.Key, saveData);
    }

    public static void LoadData(this ISaveAsJson source, JSON mainData)
    {
        if(!mainData.ContainsKey(source.Key)) return;
        JSON loadData = mainData.GetJSON(source.Key);
        if (!mainData.ContainsKey(source.Key))
            return;
        if (IsOldVersion(source, loadData))
            source.UpdateSaveData(loadData);
        source.Load(loadData);
    }
}