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
    [SerializeField] public float health; 
    [SerializeField] public int coin; 
    [SerializeField] public EItemType type;

    [Header("Equipable Item Info")]
    [SerializeField] public GameObject dropItemPrefab;
    [SerializeField] public EEquipType equipType;


    [Header("Consumable Item Info")]
    [SerializeField] public float duration;

    [NonSerialized] public Action onUnequip;


    public string GetStatDescription()
    {
        string ret = "";
        if (moveSpeed > 0)
            ret +=  $"스피드 + {moveSpeed}\n";
        if (jumpPower > 0)
            ret += $"점프력 + {jumpPower}\n";
        if (stamina > 0) 
            ret += $"스태미나 {stamina} 회복\n";
        if (health > 0)
			ret += $"체력 {health} 회복"; 

		return ret; 
    }
}