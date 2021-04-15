using System;
using System.Collections;
using System.Collections.Generic;
using Leguar.TotalJSON;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Managers/GameDataManager")]
public class GameDataManager : RuntimeScriptableSingleton<GameDataManager>
{
    public static event Action OnLoadDone;
    public static event Action OnSaveDone;
    
    public string fileName = "Save.json";
   
    public SaveAsJsonModule saveAsJsonModule;
     
    [Button]
    public static void Register(ISaveAsJson target) => Instance.saveAsJsonModule.Register(target);
    
    [Button]
    public static void Unregister(ISaveAsJson target) => Instance.saveAsJsonModule.Unregister(target);
    
    [Button]
    [MenuItem("GameData/Save")]
    public static void Save()
    {
       JSON json = Instance.saveAsJsonModule.Save();
       SaveLoadManager.SaveEncryptedJson(json, Instance.fileName);
       OnSaveDone?.Invoke();
    }

    [Button]
    [MenuItem("GameData/Load")]
    public static void Load()
    {
        Instance.saveAsJsonModule.Load(SaveLoadManager.LoadEncryptedJson(Instance.fileName));
        OnLoadDone?.Invoke();
    }

}
