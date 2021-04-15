using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Managers/CharacterPoseManager")]
public class CharacterPoseManager : RuntimeScriptableSingleton<CharacterPoseManager>
{
    public List<AnimationClip> clips = new List<AnimationClip>();

    public static AnimationClip Get(string id) => Instance.clips.Find(x => x.name == id);

    public static AnimationClip[] GetAll() => Instance.clips.ToArray();
}