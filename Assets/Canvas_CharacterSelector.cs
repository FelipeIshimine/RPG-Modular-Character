using System;
using System.Collections;
using System.Collections.Generic;
using Leguar.TotalJSON;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Canvas_CharacterSelector : MonoBehaviour, ISaveAsJson
{
    public SaveAsJsonComponent characterSaveAsJson;

    public CharacterComponent characterComponent;
    
    public Dropdown dropdown;

    public TMP_InputField nameInput;
    public TMP_InputField surnameInput;
    public TMP_InputField nicknameInput;
    
    private readonly List<JSON> _characterJsons = new List<JSON>();

    private int _index;

    private int Index
    {
        get => _index % _characterJsons.Count;
        set => _index = value;
    }
    
    private void OnEnable()
    {
        GameDataManager.Register(this);
        GameDataManager.OnLoadDone += OnLoadDone;
        dropdown.onValueChanged.AddListener(LoadCharacter);
        LoadRequest();
    }

    private void OnDisable()
    {
        GameDataManager.Unregister(this);
        GameDataManager.OnLoadDone -= OnLoadDone;
        dropdown.onValueChanged.RemoveListener(LoadCharacter);
    }

    #region SaveLoad

    public string Key => "CharacterPool";
    public int Version => 0;
    
    public JSON Save()
    {
        JSON data = new JSON();
        JArray array = new JArray();
        foreach (JSON json in _characterJsons)
           array.Add(json);
        data.Add("Characters", array);
        return data;
    }

    public void Load(JSON data)
    {
         var jsonArray = data.GetJArray("Characters").AsJSONArray();

         _characterJsons.Clear();
         foreach (JSON json in jsonArray)
             _characterJsons.Add(json);
    }

    public void UpdateSaveData(JSON data)
    {
        throw new NotImplementedException();
    }

    #endregion

    public void SaveRequest()
    {
        SaveCurrent();
        GameDataManager.Save();
    }
    
    public void LoadRequest() => GameDataManager.Load();

    [Button]
    public void Next()
    {
        Index++;
        LoadCharacter(Index % _characterJsons.Count);
    }

    [Button]
    public void Previous()
    {
        Index--;
        if (Index < 0)
            Index += _characterJsons.Count;
        LoadCharacter(Index % _characterJsons.Count);
    }

    [Button]
    private void LoadCharacter(int characterIndex)
    {
        Debug.Log(characterIndex);
        characterSaveAsJson.Load(_characterJsons[characterIndex]);
        nameInput.text = characterComponent.info.name;
        surnameInput.text = characterComponent.info.surname;
        nicknameInput.text = characterComponent.info.nickName;
    }
    
    [Button]
    public void NewCharacter()
    {
        JSON nCharacter = characterSaveAsJson.Save();
        _characterJsons.Add(nCharacter);    
        Index = _characterJsons.Count - 1;
        characterComponent.Set("Name", "Surname", "Nickname");
        LoadCharacter(Index);
    }

    public void SaveCurrent()
    {
        if(_characterJsons.Count == 0) return;
        
        characterComponent.info.name = nameInput.text;
        characterComponent.info.surname = surnameInput.text;
        characterComponent.info.nickName = nicknameInput.text;
        
        _characterJsons[Index] =  characterSaveAsJson.Save();
    }

    public void DeleteCurrent()
    {
        _characterJsons.RemoveAt(Index);
        Index = Index % _characterJsons.Count;
        LoadCharacter(Index);
        Save();
    }
    
    private void OnLoadDone()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < _characterJsons.Count; i++)
        {
            JSON json = _characterJsons[i].GetJSON("CharacterInfo");
            options.Add(new Dropdown.OptionData($"{json.GetString("name")} {(json.ContainsKey("nickname")?json.GetString("nickname") + " ":string.Empty)}{json.GetString("surname")}"));
        }
        dropdown.options = options;
    }

}
