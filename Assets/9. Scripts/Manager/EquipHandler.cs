using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EquipHandler : MonoBehaviour
{
    static private readonly string headEquip = "Head_Equip";
    static private readonly string HairEquip = "Hair_Equip";
    static private readonly string BackpackEquip = "Backpack_Equip";
    static private readonly string BodyEquip = "Body_Equip";

    private Dictionary<EEquipType, Transform> equipTs = new Dictionary<EEquipType, Transform>();
    private Dictionary<EEquipType, GameObject> equipItems = new Dictionary<EEquipType, GameObject>();
    private Dictionary<EEquipType, Action> onUnEquip = new Dictionary<EEquipType, Action>();

    private StatHandler statHandler;


    private void Awake()
    {
		statHandler = GetComponent<StatHandler>();

        equipTs.Add(EEquipType.Head, FindChildByName(transform, headEquip));
        equipTs.Add(EEquipType.Hair, FindChildByName(transform, HairEquip));
        equipTs.Add(EEquipType.Body, FindChildByName(transform, BodyEquip));
        equipTs.Add(EEquipType.Cloak, FindChildByName(transform, BackpackEquip));

		foreach (EEquipType type in Enum.GetValues(typeof(EEquipType)))
		{
            onUnEquip.Add(type, null);
			equipItems.Add(type, null);
			FindItems(type);
		}
	} 
     
    public void FindItems(EEquipType type)
    {
        equipItems[type] = equipTs[type].childCount > 0 ? equipTs[type].GetChild(0).gameObject : null;
	}

	Transform FindChildByName(Transform parent, string childName)
	{
		Transform[] allChildren = parent.GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren)
		{
			if (child.name == childName)
			{
				return child;
			}
		}
		return null;
	}

	public void EquipItem(Item item)
    {
        EEquipType type = item.data.equipType;
		var nextItem = Instantiate<GameObject>(item.data.dropItemPrefab);
        GameObject prevItem = equipItems[type];
        Transform ts = equipTs[type];

        //onUnEquip[type]?.Invoke();
        //onUnEquip[type] = null;


        Destroy(prevItem);
        nextItem.transform.SetParent(ts, false);
        nextItem.transform.localPosition = Vector3.zero;
        equipItems[type] = nextItem; 
	}
}
