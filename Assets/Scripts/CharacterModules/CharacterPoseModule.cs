using System;
using System.Collections;
using System.Collections.Generic;
using Leguar.TotalJSON;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterPoseModule : MonoBehaviour, ISaveAsJson
{
    public Animator animator;

    private AnimationClip _animationClip;
    
    private void Awake()
    {
        Canvas_PoseSelection.OnPoseSelected += SwapAnimationClip;
    }

    private void OnDestroy()
    {
        Canvas_PoseSelection.OnPoseSelected -= SwapAnimationClip;
    }

    [Button]
    private void SwapAnimationClip(AnimationClip anim)
    {
        if (anim)
        {
            AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
            var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            foreach (var a in aoc.animationClips)
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, anim));
            aoc.ApplyOverrides(anims);
            animator.runtimeAnimatorController = aoc;
            _animationClip = anim;
        }
    }

    #region ISaveLoadAsJson

    public string Key => "Pose";
    public int Version => 0;
    
    public JSON Save()
    {
        JSON data = new JSON();
        data.Add("ClipName",_animationClip!=null?_animationClip.name:"null");
        return data;
    }

    public void Load(JSON data)
    {
        if(!data.ContainsKey("ClipName")) return;
        string clipName = data.GetString("ClipName");
        SwapAnimationClip(CharacterPoseManager.Get(clipName));
    }

    public void UpdateSaveData(JSON data)
    {
        throw new NotImplementedException();
    }

    #endregion
}

