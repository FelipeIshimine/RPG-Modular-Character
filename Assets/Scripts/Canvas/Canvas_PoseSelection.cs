using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using  TMPro;

public class Canvas_PoseSelection : MonoSingleton<Canvas_PoseSelection>
{
    public static event Action<AnimationClip> OnPoseSelected;

    public TMP_Dropdown dropdown;
    private AnimationClip[] _clips;

    private void Start()
    {
        SetPoses(CharacterPoseManager.GetAll());
    }

    [Button]
    public void SetPoses(AnimationClip[] clips)
    {
        _clips = clips;

        List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();

        foreach (AnimationClip animationClip in clips)
            list.Add(new TMP_Dropdown.OptionData(animationClip.name));
        
        dropdown.options = list;
    }
    
    public void Select(int index)
    {
        OnPoseSelected?.Invoke(_clips[index]);
    }
}
