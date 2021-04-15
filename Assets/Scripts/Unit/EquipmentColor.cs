using System;
using System.Collections;
using System.Collections.Generic;
using Leguar.TotalJSON;
using Sirenix.OdinInspector;
using UnityEngine;



[RequireComponent(typeof(BodyEquipmentModule))]
public class EquipmentColor : MonoBehaviour, ISaveAsJson
{
    public BodyEquipmentModule bodyEquipmentModule;

    [ShowInInspector] private List<Renderer> renderers = new List<Renderer>();
    private static readonly int ColorPrimary = Shader.PropertyToID("_Color_Primary");
    private static readonly int ColorSecondary = Shader.PropertyToID("_Color_Secondary");
    private static readonly int ColorLeatherPrimary = Shader.PropertyToID("_Color_Leather_Primary");
    private static readonly int ColorMetalPrimary = Shader.PropertyToID("_Color_Metal_Primary");
    private static readonly int ColorLeatherSecondary = Shader.PropertyToID("_Color_Leather_Secondary");
    private static readonly int ColorMetalDark = Shader.PropertyToID("_Color_Metal_Dark");
    private static readonly int ColorMetalSecondary = Shader.PropertyToID("_Color_Metal_Secondary");
    private static readonly int ColorHair = Shader.PropertyToID("_Color_Hair");
    private static readonly int ColorSkin = Shader.PropertyToID("_Color_Skin");
    private static readonly int ColorStubble = Shader.PropertyToID("_Color_Stubble");
    private static readonly int ColorScar = Shader.PropertyToID("_Color_Scar");
    private static readonly int ColorBodyArt = Shader.PropertyToID("_Color_BodyArt");
    private static readonly int ColorEyes = Shader.PropertyToID("_Color_Eyes");

    [SerializeField] private Color primary; 
    [SerializeField] private Color secondary; 
    [SerializeField] private Color leatherPrimary; 
    [SerializeField] private Color metalPrimary; 
    [SerializeField] private Color leatherSecondary; 
    [SerializeField] private Color metalDark; 
    [SerializeField] private Color metalSecondary; 
    [SerializeField] private Color hair; 
    [SerializeField] private Color skin; 
    [SerializeField] private Color stubble; 
    [SerializeField] private Color scar; 
    [SerializeField] private Color bodyArt; 
    [SerializeField] private Color eyes; 
    
    [Button]
    public void SetColorPreset(ColorPreset nColorPreset)
    {
        primary = nColorPreset.primary;
        secondary = nColorPreset.secondary;
        leatherPrimary = nColorPreset.leatherPrimary;
        metalPrimary = nColorPreset.metalPrimary;
        leatherSecondary = nColorPreset.leatherSecondary;
        metalDark = nColorPreset.metalDark;
        metalSecondary = nColorPreset.metalSecondary;
        hair = nColorPreset.hair;
        skin = nColorPreset.skin;
        scar = nColorPreset.scar;
        bodyArt = nColorPreset.bodyArt;
        eyes = nColorPreset.eyes;
        ApplyColors();
    }
    
    private void Awake()
    {
        bodyEquipmentModule.OnEquip += OnEquip;
        bodyEquipmentModule.OnUnequip += OnUnequip;
    }

    private void OnDestroy()
    {
        bodyEquipmentModule.OnEquip -= OnEquip;
        bodyEquipmentModule.OnUnequip -= OnUnequip;
    }

    private void OnValidate()
    {
        if(bodyEquipmentModule == null)
            bodyEquipmentModule = GetComponent<BodyEquipmentModule>();
    }

    private void OnEquip(EquipmentSlot slot, EquipmentData equipmentData)
    {
        foreach (Renderer equipmentDataRenderer in equipmentData.renderers)
        {
            if(!renderers.Contains(equipmentDataRenderer))
                renderers.Add(equipmentDataRenderer);
        }
        equipmentData.renderers.ForEach(ApplyColors);
    }
    
    private void OnUnequip(EquipmentSlot slot, EquipmentData equipmentData)
    {
        foreach (Renderer equipmentDataRenderer in equipmentData.renderers)
            if(renderers.Contains(equipmentDataRenderer))
                renderers.Remove(equipmentDataRenderer);
    }

    [Button]
    private void ApplyColors() => renderers.ForEach(ApplyColors);

    [Button]
    private void ApplyColors(Renderer renderer)
    {
        var material = renderer.material;
        
        material.SetColor(ColorPrimary, primary);
        material.SetColor(ColorSecondary, secondary);
        material.SetColor(ColorLeatherPrimary, leatherPrimary);
        material.SetColor(ColorMetalPrimary, metalPrimary);
        material.SetColor(ColorLeatherSecondary, leatherSecondary);
        material.SetColor(ColorMetalDark, metalDark);
        material.SetColor(ColorMetalSecondary, metalSecondary);
        material.SetColor(ColorHair, hair);
        material.SetColor(ColorStubble, stubble);
        material.SetColor(ColorSkin, skin);
        material.SetColor(ColorScar, scar);
        material.SetColor(ColorBodyArt, bodyArt);
        material.SetColor(ColorEyes, eyes);
        renderer.material = material;
    }

    #region SaveAsJson

    public string Key => "EquipmentColor";
    public int Version => 0;
    
    public JSON Save()
    {
        JSON json = new JSON();
        json.Add("Primary", primary.AsJson());
        json.Add("Secondary", secondary.AsJson());
        json.Add("LeatherPrimary", leatherPrimary.AsJson());
        json.Add("MetalPrimary", metalPrimary.AsJson());
        json.Add("LeatherSecondary", leatherSecondary.AsJson());
        json.Add("MetalDark", metalDark.AsJson());
        json.Add("MetalSecondary", metalSecondary.AsJson());
        json.Add("Hair", hair.AsJson());
        json.Add("Skin", skin.AsJson());
        json.Add("Stubble", stubble.AsJson());
        json.Add("Scar", scar.AsJson());
        json.Add("BodyArt", bodyArt.AsJson());
        json.Add("Eyes", eyes.AsJson());
        
        return json;
    }

    public void Load(JSON data)
    {
        primary = data.GetJSON("Primary").AsColor();
        secondary = data.GetJSON("Secondary").AsColor();
        leatherPrimary = data.GetJSON("LeatherPrimary").AsColor();
        metalPrimary = data.GetJSON("MetalPrimary").AsColor();
        leatherSecondary = data.GetJSON("LeatherSecondary").AsColor();
        metalDark = data.GetJSON("MetalDark").AsColor();
        metalSecondary = data.GetJSON("MetalSecondary").AsColor();
        hair = data.GetJSON("Hair").AsColor();
        skin = data.GetJSON("Skin").AsColor();
        stubble = data.GetJSON("Stubble").AsColor();
        scar = data.GetJSON("Scar").AsColor();
        bodyArt = data.GetJSON("BodyArt").AsColor();
        eyes = data.GetJSON("Eyes").AsColor();
        ApplyColors();
    }

    #if UNITY_EDITOR
    [Button]
    public void CreatePreset(string presetName)
    {
        ColorPresetsManager.CreateNew(presetName, 
            primary,
            secondary,
            leatherPrimary,
            metalPrimary, 
            leatherSecondary,
            metalDark,
            metalSecondary,
            hair,
            skin,
            stubble,
            scar,
            bodyArt,
            eyes
            );
    }
    #endif
    
    public void UpdateSaveData(JSON data)
    {
        throw new NotImplementedException();
    }
    #endregion
    
}

public static class ColorExtension
{
    public static JSON AsJson(this Color color)
    {
        JSON json = new JSON();
        json.Add("a",color.a);
        json.Add("r",color.r);
        json.Add("g",color.g);
        json.Add("b",color.b);
        return json;
    }
    
    public static Color AsColor(this JSON json)
    {
        return new Color(json.GetFloat("r"),json.GetFloat("g"),json.GetFloat("b"),json.GetFloat("a"));
    }
}