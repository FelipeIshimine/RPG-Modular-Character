using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create UnitSkin", fileName = "UnitSkin", order = 0)]
public class SkinData : ScriptableObject
{
    [SerializeField] private Texture texture;
    public Texture Texture => texture;

    [SerializeField] private Material material;
    public Material Material => material;

    public Color Color_Primary;
    public Color Color_Secondary;
    public Color Color_Leather_Primary;
    public Color Color_Metal_Primary;
    public Color Color_Leather_Secondary;
    public Color Color_Metal_Dark;
    public Color Color_Metal_Secondary;
    public Color Color_Hair;
    public Color Color_Skin;
    public Color Color_Stubble;
    public Color Color_Scar;
    public Color Color_BodyArt;
    public Color Color_Eyes;

}
