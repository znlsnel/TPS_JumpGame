using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionHandler : MonoBehaviour
{
    [Header("Interaction Info")]
    [SerializeField] private LayerMask layer;
	[SerializeField] private float interactionDistance;

	PlayerItemHandler playerItemHandler;
	PlayerUIHandler uiHandler;
	Item selectItem;

	private void Start()
	{
		InvokeRepeating(nameof(Find), 0, 0.1f);

		playerItemHandler = GetComponent<PlayerItemHandler>();

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

		playerItemHandler.PickupItem(selectItem);
	


	}
}
