using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionHandler : MonoBehaviour
{
    [Header("Interaction Info")]
    [SerializeField] private LayerMask layer;
	[SerializeField] private float interactionDistance;

	StatHandler statHander;
	PlayerUIHandler uiHandler;
	EquipHandler equipHandler;
	Item selectItem;

	private void Start()
	{
		InvokeRepeating(nameof(Find), 0, 0.1f);

		statHander = GetComponent<StatHandler>();
		equipHandler = GetComponent<EquipHandler>();

		uiHandler = GameManager.Instance.PlayerController.PlayerUIHandler;
		InputManager.Instance.Interaction.action.started += InteractionInput;
	}

	void Find()
    {
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
		RaycastHit hit;

		selectItem = null;
		if (Physics.Raycast(ray, out hit, interactionDistance, layer))
		{
			selectItem = hit.collider.gameObject.GetComponent<Item>();
		}

		uiHandler.InteractionUI.RegistItem(selectItem);
	} 


	void InteractionInput(InputAction.CallbackContext context)
	{
		if (selectItem == null)
			return;

		if (selectItem.data.type == EItemType.Equipable)
		{
			equipHandler.EquipItem(selectItem);
		}

		if (selectItem.data.type == EItemType.Consumable)
		{
			float moveSpeed = selectItem.data.moveSpeed;
			float jumpPower = selectItem.data.jumpPower;
			float duration = selectItem.data.duration;

			// 사용
			statHander.MoveSpeed += moveSpeed;
			statHander.JumpPower += jumpPower;
			statHander.GetCondition(ConditionType.Stamina).Add(selectItem.data.stamina);

			
			// 일정 시간 뒤에 효과 취소 
			StartCoroutine(GameManager.Instance.SetTimer(() =>
			{
				statHander.MoveSpeed -= moveSpeed;
				statHander.JumpPower -= jumpPower;
			}, duration));

			Destroy(selectItem.gameObject);
			selectItem = null;
		}


	}
}
