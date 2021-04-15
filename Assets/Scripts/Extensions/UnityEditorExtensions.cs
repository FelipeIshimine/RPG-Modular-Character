using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class UnityEditorExtensions
{
#if UNITY_EDITOR
    /// <summary>
    /// Empieza automaticamente desde Assets
    /// </summary>
    /// <param name="folders"></param>
    public static string CreateFoldersRecursive(params string[] folders)
    {
        string currentPath = "Assets/";
        for (int i = 0; i < folders.Length; i++)
        {
            Debug.Log(currentPath);
            string parentFolder = currentPath.Remove(currentPath.Length - 1, 1);
            string subfolder = folders[i];
            if(!AssetDatabase.IsValidFolder(currentPath + subfolder))
                AssetDatabase.CreateFolder(parentFolder, subfolder);
            currentPath += $"{folders[i]}/";
        }
        AssetDatabase.Refresh();
        return currentPath;
    }
    
    public static T AssetFromGUID<T>(string assetGUID) where T : Object
    {
        string path = AssetDatabase.GUIDToAssetPath(assetGUID);
        return  AssetDatabase.LoadAssetAtPath<T>(path);
    }

    public static T FindAssetByType<T>() where T : UnityEngine.Object => FindAssetsByType<T>()[0];

    public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            UnityEngine.Object[] found = AssetDatabase.LoadAllAssetsAtPath(assetPath);

            for (int index = 0; index < found.Length; index++)
                if (found[index] is T item && !assets.Contains(item))
                    assets.Add(item);
        }

        return assets;
    }

    public static UnityEngine.Object FindAssetByType(Type type) 
    {
        return FindAssetsByType(type)[0];
    }

    public static List<Object> FindAssetsByType(Type type) 
    {
        List<Object> assets = new List<Object>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", type));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            UnityEngine.Object[] found = AssetDatabase.LoadAllAssetsAtPath(assetPath);

            for (int index = 0; index < found.Length; index++)
                if (!assets.Contains(found[index]))
                    assets.Add(found[index]);
        }

        return assets;
    }


#endif

}
