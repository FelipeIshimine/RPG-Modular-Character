using System.Collections;
using System.Collections.Generic;
using Leguar.TotalJSON;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Managers/Create CharacterPoolManager", fileName = "CharacterPoolManager", order = 0)]
public class CharacterPoolManager : RuntimeScriptableSingleton<CharacterPoolManager>
{
   public string fileName = "CharacterPool.json";
   private readonly Dictionary<string,JSON> _characters = new Dictionary<string,JSON>();

   public static void Add(string id, JSON characterJson)
   {
      Instance._characters.Add(id, characterJson);
   }

   public static void Replace(string  id, JSON characterJson)
   {
      Remove(id);
      Add(id, characterJson);
   }

   public static void Remove(string id)
   {
      Instance._characters.Remove(id);
   }

   [Button]
   public static void Load()
   {
     JSON data = SaveLoadManager.LoadEncryptedJson(Instance.fileName);
     if (data == null) return;
     Instance._characters.Clear();
     foreach (string dataKey in data.Keys)
        Instance._characters.Add(dataKey, data.GetJSON(dataKey));
   }
   
   [Button]
   public static void Save()
   {
      JSON data = new JSON();

      foreach (KeyValuePair<string,JSON> pair in Instance._characters)
         data.Add(pair.Key,pair.Value);
      
      SaveLoadManager.SaveEncryptedJson(data, Instance.fileName);
   }

   public static JSON Get(string id) => Instance._characters[id];
}
