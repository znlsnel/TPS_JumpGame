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

    private void Awake()
    {
        equipTs.Add(EEquipType.Head, Util.FindChildByName(transform, headEquip));
        equipTs.Add(EEquipType.Hair, Util.FindChildByName(transform, HairEquip));
        equipTs.Add(EEquipType.Body, Util.FindChildByName(transform, BodyEquip));
        equipTs.Add(EEquipType.Cloak, Util.FindChildByName(transform, BackpackEquip));

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



	public void EquipItem(Item item)
    {
        EEquipType type = item.data.equipType;
        Transform ts = equipTs[type];

		GameObject nextItem = item.gameObject;
        GameObject curItem = equipItems[type];

		// 현재 장착중인 아이템 장착 해제
		if (curItem != null && curItem.TryGetComponent(out Item myItem))
		{
			curItem.transform.SetParent(null, false);
			curItem.transform.position = transform.position + transform.forward * 0.3f;
			myItem.data.onUnequip?.Invoke();
			curItem.gameObject.GetComponent<Item>().SetActiveItem(true);
		}
		else
			Destroy(curItem); 
		 

		// 새로운 아이템 장착
		nextItem.gameObject.GetComponent<Item>().SetActiveItem(false);
		nextItem.transform.SetParent(ts, false);
		nextItem.transform.localPosition = Vector3.zero;
        equipItems[type] = nextItem; 
	}
}
