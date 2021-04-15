using System;
using System.Collections;
using System.Collections.Generic;
using Leguar.TotalJSON;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterComponent : MonoBehaviour, ISaveAsJson
{
    public SaveAsJsonComponent saveAsJsonComponent;

    public CharacterInfo info;

    private void OnValidate()
    {
        if (saveAsJsonComponent == null)
            saveAsJsonComponent = GetComponent<SaveAsJsonComponent>();
    }

    [Button]
    public void AddToPool() => CharacterPoolManager.Add(saveAsJsonComponent.Key, saveAsJsonComponent.Save());
    
    [Button]
    public void LoadFromPool(string id) => saveAsJsonComponent.Load(CharacterPoolManager.Get(id));

    #region SaveLoad

    public string Key => info.Key;
    public int Version => info.Version;

    public JSON Save() => info.Save();

    public void Load(JSON data) => info.Load(data);

    public void UpdateSaveData(JSON data) => info.UpdateSaveData(data);

    #endregion

    public void Set(string nName, string surname, string nickname)
    {
        info = new CharacterInfo(nName, surname, nickname);
    }
}

[System.Serializable]
public struct CharacterInfo : ISaveAsJson
{
    public string name;
    public string surname;
    public string nickName;

    public CharacterInfo(string name, string surname, string nickName)
    {
        this.name = name;
        this.surname = surname;
        this.nickName = nickName;
    }

    public string FullName => $"{name} {surname}";

    public string FullNameWithNickname => $"{name} '{nickName}' {surname}";

   #region SaveLoad

    public string Key => "CharacterInfo";
    public int Version => 0;

    public JSON Save()
    {
        JSON data = new JSON();

        data.Add("name",name);
        data.Add("surname",surname);
        data.Add("nickName",nickName);
        return data;
    }

    public void Load(JSON data)
    {
        name = data.GetString("name");
        surname = data.GetString("surname");
        nickName = data.GetString("nickName");
    }

    public void UpdateSaveData(JSON data)
    {
        throw new NotImplementedException();
    }

    #endregion

}