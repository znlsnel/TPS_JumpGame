using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum EItemType
{
	Equipable,
	Consumable,
}

[Serializable]
public enum EEquipType
{
	Cloak,
	Body,
	Head,
	Hair,
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] public string name;
    [SerializeField] public string description;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpPower;
    [SerializeField] public float stamina;
    [SerializeField] public EItemType type;

    [Header("Equipable Item Info")]
    [SerializeField] public GameObject dropItemPrefab;
    [SerializeField] public EEquipType equipType;


    [Header("Consumable Item Info")]
    [SerializeField] public float duration;

    [NonSerialized] public Action onUnequip;
}