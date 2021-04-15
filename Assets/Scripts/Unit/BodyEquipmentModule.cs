using System;
using System.Collections;
using System.Collections.Generic;
using Leguar.TotalJSON;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BodyEquipmentModule : MonoBehaviour, ISaveAsJson
{
    public event Action<EquipmentSlot, EquipmentData> OnEquip;
    public event Action<EquipmentSlot, EquipmentData> OnUnequip;
    
    public BonesIndex bonesIndex;
    
    public List<GameObject> equipment = new List<GameObject>();

    private CharacterEquipmentManager _characterEquipmentManager;
    
    private Dictionary<EquipmentSlot,GameObject> slotToGameobjectDictionary = new Dictionary<EquipmentSlot,GameObject>();

    
    #region ISaveAsJson

    public string Key => nameof(BodyEquipmentModule);
    public int Version => 0;
    
    public JSON Save()
    {
        JSON json = new JSON();
        JArray array = new JArray();
        foreach (GameObject go in equipment)
            array.Add(go.name);
        json.Add("Equipment", array);
        return json;
    }

    public void Load(JSON data)
    {
        JArray equipmentIds = data.GetJArray("Equipment");
        foreach (string equipmentId in equipmentIds.AsStringArray())
        {
            var equipment = CharacterEquipmentManager.Get(equipmentId);
            if(equipment != null)
                Equip(equipment);
            else
                Debug.Log($"Could not find equipment with id: {equipmentId}");
        }
    }

    public void UpdateSaveData(JSON data)
    {
        throw new NotImplementedException();
    }
    #endregion
    
    
    private void Start()
    {
        _characterEquipmentManager = ManagerInitializer.Get<CharacterEquipmentManager>();
    }

    [Button]
    public void Equip(GameObject prefab)
    {
        GameObject clone = Instantiate(prefab);
        clone.name = clone.name.Replace("(Clone)", string.Empty);
        clone.transform.SetParent(transform);
        EquipmentData equipmentData = clone.GetComponent<EquipmentData>();

        if (equipmentData == null)
        {
            Destroy(clone);
            throw new Exception("Invalid object");
        }
        
        //Unequip old
        Unequip(equipmentData.Slots);

        //Equip
        if (Application.isPlaying)
            Destroy(clone.transform.GetChild(0).gameObject);
        else
            DestroyImmediate(clone.transform.GetChild(0).gameObject);
        
        List<SkinnedMeshRenderer> childrenRenderers = new List<SkinnedMeshRenderer>(clone.GetComponentsInChildren<SkinnedMeshRenderer>());
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in childrenRenderers)
        {
            var skinBones = skinnedMeshRenderer.bones;
            skinnedMeshRenderer.rootBone = bonesIndex[skinnedMeshRenderer.rootBone.name];
            List<Transform> nBones = new List<Transform>();
            foreach (Transform skinBone in skinBones)
                nBones.Add(bonesIndex[skinBone.name]);

            skinnedMeshRenderer.bones = nBones.ToArray();
        }

        foreach (EquipmentSlot dataSlot in equipmentData.Slots)
        {
            slotToGameobjectDictionary.Add(dataSlot, clone);
            OnEquip?.Invoke(dataSlot,equipmentData);
        }
        
        equipment.Add(clone);
    }

    private void Unequip(List<EquipmentSlot> equipmentDataSlots)
    {
        List<GameObject> go = new List<GameObject>();

        foreach (EquipmentSlot equipmentSlot in equipmentDataSlots)
        {
            if(!slotToGameobjectDictionary.ContainsKey(equipmentSlot)) continue;

            var aux = slotToGameobjectDictionary[equipmentSlot];
            if (aux == null)
            {
                slotToGameobjectDictionary.Remove(equipmentSlot);
                continue;
            }
            
            if (!go.Contains(aux))
                go.Add(slotToGameobjectDictionary[equipmentSlot]);
            
            Unequip(equipmentSlot, false);
        }

        for (int i = go.Count - 1; i >= 0; i--)
        {
            if(Application.isPlaying)
                Destroy(go[i]);
            else
                DestroyImmediate(go[i]);
        }
    }

    private void Unequip(EquipmentSlot equipmentDataSlot, bool destroyGo = true)
    {
        var go = slotToGameobjectDictionary[equipmentDataSlot];
     
        slotToGameobjectDictionary.Remove(equipmentDataSlot);
        OnUnequip?.Invoke(equipmentDataSlot, go.GetComponent<EquipmentData>());
        equipment.Remove(go);

        if (!destroyGo) return;
        if(Application.isPlaying)
            Destroy(go);
        else
            DestroyImmediate(go);
    }

    [Button]
    public void Clear()
    {
        var keys = new List<EquipmentSlot>(slotToGameobjectDictionary.Keys);
        foreach (EquipmentSlot equipmentSlot in keys)
        {
            if (slotToGameobjectDictionary.ContainsKey(equipmentSlot))
            {
                if (slotToGameobjectDictionary[equipmentSlot] == null)
                    slotToGameobjectDictionary.Remove(equipmentSlot);
                else
                    Unequip(equipmentSlot);
            }
        }
    }
    
    [Button]
    public void EquipRandom()
    {
        Equip(_characterEquipmentManager.prefabs.GetRandom().gameObject);
    }

}