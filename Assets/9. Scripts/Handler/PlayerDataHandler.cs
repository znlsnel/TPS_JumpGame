using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerDataHandler : MonoBehaviour
{

	private int coin = 1; 
	public int Coin { get => coin; set {  coin = value; onUpdateCoin?.Invoke(); } }
	public event Action onUpdateCoin;

	EquipHandler equipHandler;
	StatHandler statHander;

	private void Awake()
	{
		equipHandler = GetComponent<EquipHandler>();
		statHander = GetComponent<StatHandler>();
	}
	 
	public void PickupItem(Item item)
    {
		if (item.data.type == EItemType.Equipable)
			equipHandler.EquipItem(item);

		ApplyItemStats(item, item.data.type == EItemType.Consumable);
	}

	  
	void ApplyItemStats(Item item, bool isConsumableItem)
	{
		float moveSpeed = item.data.moveSpeed;
		float jumpPower = item.data.jumpPower;
		float duration = item.data.duration;

		// 사용
		statHander.MoveSpeed += moveSpeed;
		statHander.JumpPower += jumpPower;
		statHander.GetCondition(ConditionType.Stamina).Add(item.data.stamina);
		statHander.GetCondition(ConditionType.Health).Add(item.data.health);
		Coin += item.data.coin;

		Action removeEffect = () =>
		{
			statHander.MoveSpeed -= moveSpeed;
			statHander.JumpPower -= jumpPower;
		};

		if (isConsumableItem)
		{
			// 소모형 아이템이라면, 일정 시간 뒤에 효과 제거
			StartCoroutine(GameManager.Instance.SetTimer(() =>
			{
				removeEffect?.Invoke();
			}, duration)); 
			item.UseConsumableItem();
		}
		else
		{
			item.data.onUnequip = null;
			item.data.onUnequip += removeEffect;
		}
	}

}
