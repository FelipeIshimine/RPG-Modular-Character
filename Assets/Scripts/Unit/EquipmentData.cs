using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EquipmentData : MonoBehaviour
{
    [SerializeField, AssetList, ListDrawerSettings(NumberOfItemsPerPage = 100)] private List<EquipmentSlot> slots;
    public List<EquipmentSlot> Slots => slots;

    public List<Renderer> renderers = new List<Renderer>();

    private void OnValidate()
    {
        renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
    }
}