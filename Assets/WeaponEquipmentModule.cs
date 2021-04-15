using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponEquipmentModule : MonoBehaviour
{
    public event Action<WeaponData> OnEquip; 
    public event Action<WeaponData> OnUnequip;
    
    [TableList] public List<SlotTransformPair> pairs = new List<SlotTransformPair>();

    
    private Dictionary<EquipmentSlot, WeaponData> _dictionary = new Dictionary<EquipmentSlot, WeaponData>();
    
    [Button]
    public void Equip(WeaponData weaponData)
    {
        GameObject go = Instantiate(weaponData.gameObject);
        weaponData = go.GetComponent<WeaponData>();

        if (_dictionary.ContainsKey(weaponData.equipmentSlot))
            Unequip(weaponData.equipmentSlot);
        
        _dictionary.Add(weaponData.equipmentSlot, weaponData);
        
        go.transform.SetParent(GetTransformFromEquipmentSlot(weaponData));
        go.transform.localRotation = Quaternion.identity;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        
        OnEquip?.Invoke(weaponData);
    }

    private Transform GetTransformFromEquipmentSlot(WeaponData weaponData)
    {
        return pairs.Find(x => x.slot == weaponData.equipmentSlot).transform;
    }

    private void Unequip(EquipmentSlot equipmentSlot)
    {
        var go = _dictionary[equipmentSlot].gameObject;
        _dictionary.Remove(equipmentSlot);
        OnUnequip?.Invoke(go.GetComponent<WeaponData>());
        Destroy(go);
    }

 
}


[System.Serializable]
public class SlotTransformPair
{
    public EquipmentSlot slot;
    public Transform transform;
}