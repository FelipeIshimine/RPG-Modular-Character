using UnityEngine;

[CreateAssetMenu(menuName = "Equipment/ColorPreset")]
public class ColorPreset : ScriptableObject
{
    [ColorUsage(false,false)]public Color primary;
    public Color secondary;
    public Color leatherPrimary;
    public Color metalPrimary;
    public Color leatherSecondary;
    public Color metalDark;
    public Color metalSecondary;
    public Color hair;
    public Color skin;
    public Color stubble;
    public Color scar;
    public Color bodyArt;
    public Color eyes;
}