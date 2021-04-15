using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Managers/CharacterEquipmentManager")]
public class CharacterEquipmentManager : RuntimeScriptableSingleton<CharacterEquipmentManager>
{
    [AssetList(AutoPopulate = true), AssetsOnly]public List<EquipmentData> prefabs = new List<EquipmentData>();

    public Dictionary<EquipmentSlot, List<EquipmentData>> dictionary =
        new Dictionary<EquipmentSlot, List<EquipmentData>>();

    public override void Initialize()
    {
        base.Initialize();
        foreach (EquipmentData equipmentData in prefabs)
            foreach (EquipmentSlot equipmentSlot in equipmentData.Slots)
                if(!dictionary.ContainsKey(equipmentSlot))
                    dictionary.Add(equipmentSlot, new List<EquipmentData>());
    }

    public static GameObject Get(string equipmentId) => Instance.prefabs.Find(x => x.gameObject.name == equipmentId).gameObject;
}