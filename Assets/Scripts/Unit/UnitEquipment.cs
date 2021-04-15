using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UnitEquipment : MonoBehaviour
{
    public List<Renderer> renderers;

    private void OnValidate()
    {
        GetComponentsInChildren<Renderer>(renderers);
    }

    [Button]
    public void SetSkin(SkinData skinData)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = skinData.Material;
            renderer.material.mainTexture = skinData.Texture;
            renderer.material.SetColor("Color_Primary", skinData.Color_Primary);
            renderer.material.SetColor("Color_Secondary", skinData.Color_Secondary);
            renderer.material.SetColor("Color_Leather_Primary", skinData.Color_Leather_Primary);
            renderer.material.SetColor("Color_Metal_Primary", skinData.Color_Metal_Primary);
            renderer.material.SetColor("Color_Leather_Secondary", skinData.Color_Leather_Secondary);
            renderer.material.SetColor("Color_Metal_Dark", skinData.Color_Metal_Dark);
            renderer.material.SetColor("Color_Metal_Secondary", skinData.Color_Metal_Secondary);
            renderer.material.SetColor("Color_Hair", skinData.Color_Hair);
            renderer.material.SetColor("Color_Skin", skinData.Color_Skin);
            renderer.material.SetColor("Color_Stubble", skinData.Color_Stubble);
            renderer.material.SetColor("Color_Scar", skinData.Color_Scar);
            renderer.material.SetColor("Color_BodyArt", skinData.Color_BodyArt);
            renderer.material.SetColor("Color_Eyes", skinData.Color_Eyes);
        }
    }
}