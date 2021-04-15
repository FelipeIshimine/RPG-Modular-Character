using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public class CharacterCrationModuleWindow : OdinEditorWindow
{
    [MenuItem("CharacterModuleCreation/ProcessWindow")]
    private static void OpenWindow()
    {
        GetWindow<CharacterCrationModuleWindow>().Show();
    }

    [FolderPath] public string processPath = "Assets/Your folder";
    public string bonesRootName = "CharacterRigV2";
  
     public Material[] materials;

     public GameObject defaultTarget;

     [Button]
     private void ProcessPrefabs(GameObject[] gameObjects)
     {
         foreach (GameObject gameObject in gameObjects)
             ProcessPrefab(gameObject);
     }

     [Button]
     private void ProcessPrefab(GameObject gameObject)
     {
         if (gameObject == null)
             gameObject = defaultTarget;
         
         Debug.Log(gameObject);

         List<Transform> children = new List<Transform>();

         foreach (Transform child in gameObject.transform)
             children.Add(child);
         
         children.Remove(gameObject.transform);
         
         Transform bonesRoot = children.Find(x => x.name == bonesRootName);
         if (bonesRoot == null)
             throw new Exception("Root boone Transform not found");

         children.Remove(bonesRoot);

         foreach (Transform child in children)
         {
             //GameObject prefabVariant = PrefabUtility.InstantiatePrefab(gameObject) as GameObject;
             //GameObject prefabVariant = Instantiate(gameObject) as GameObject;
             GameObject nPrefab = Instantiate(gameObject);
             nPrefab.name = nPrefab.name.Replace("(Clone)", string.Empty);
             List<Transform> subChildren = new List<Transform>();
             foreach (Transform subChild in nPrefab.transform)
                 subChildren.Add(subChild);

             for (int i = subChildren.Count - 1; i >= 0; i--)
                 if (subChildren[i].name != bonesRoot.name && subChildren[i].name != child.name)
                     DestroyImmediate(subChildren[i].gameObject);

             List<Renderer> renderers = new List<Renderer>(nPrefab.GetComponentsInChildren<Renderer>());
             //Agregar material a cada renderer

             foreach (Renderer renderer in renderers)
                 renderer.materials = materials;

             PrefabUtility.SaveAsPrefabAssetAndConnect(nPrefab, $"{processPath}/{child.name}.prefab",
                 InteractionMode.UserAction);
             DestroyImmediate(nPrefab);
         }
     }
      

}
