using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Managers/ColorPresetsManager")]
public class ColorPresetsManager : RuntimeScriptableSingleton<ColorPresetsManager>
{
    [TableList] public List<ColorPreset> presets = new List<ColorPreset>();
    public static ColorPreset Get(string id) => Instance.presets.Find(x => id == x.name);
   
#if UNITY_EDITOR
    [Button]
    public static void CreateNew(string presetName, Color primary, Color secondary, Color leatherPrimary, Color metalPrimary, Color leatherSecondary, Color metalDark, Color metalSecondary, Color hair, Color skin, Color stubble, Color scar, Color bodyArt, Color eyes)
    {
        ColorPreset nColorPreset = CreateInstance<ColorPreset>();
        nColorPreset.primary = primary;
        nColorPreset.secondary = secondary;
        nColorPreset.leatherPrimary = leatherPrimary;
        nColorPreset.metalPrimary = metalPrimary;
        nColorPreset.leatherSecondary = leatherSecondary;
        nColorPreset.metalDark = metalDark;
        nColorPreset.metalSecondary = metalSecondary;
        nColorPreset.hair = hair;
        nColorPreset.skin = skin;
        nColorPreset.stubble = stubble;
        nColorPreset.scar = scar;
        nColorPreset.bodyArt = bodyArt;
        nColorPreset.eyes = eyes;

        if (string.IsNullOrEmpty(presetName))
            presetName = $"Color preset {Instance.presets.Count}";
        
        nColorPreset.name = presetName;
        AssetDatabase.AddObjectToAsset(nColorPreset, Instance);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Reload();
    }
    
    [Button]
    private static void Reload()
    {
        List<Object> objects = new List<Object>(AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(Instance)));

        for (int index = objects.Count - 1; index >= 0; index--)
            if (!(objects[index] is ColorPreset))
                objects.RemoveAt(index);

        Instance.presets = objects.ConvertAll(x => x as ColorPreset);
    }
#endif
}