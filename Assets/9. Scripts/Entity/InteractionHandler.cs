using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionHandler : MonoBehaviour
{
    [Header("Interaction Info")]
    [SerializeField] private LayerMask layer;
	[SerializeField] private float interactionDistance;
	[SerializeField] private GameObject aimUIPrefab;

	[SerializeField, Range(0, 1)] private float yOffset;
	[SerializeField, Range(0, 1)] private float xOffset;
	 

	private RectTransform aim;
	private PlayerItemHandler playerItemHandler;
	private PlayerUIHandler uiHandler;
	private Item selectItem;

	private void Start()
	{
		InvokeRepeating(nameof(Find), 0, 0.1f);

		aim = Instantiate<GameObject>(aimUIPrefab).transform.GetChild(0).GetComponent<RectTransform>();
		if (aim != null)
		{
			var pos = new Vector3(Screen.width * xOffset, Screen.height * yOffset);
			//var worldPos = Camera.main.ScreenToWorldPoint(pos);
			aim.localPosition = aim.parent.InverseTransformPoint(pos); 
		} 

		playerItemHandler = GetComponent<PlayerItemHandler>();

		uiHandler = GameManager.Instance.PlayerController.PlayerUIHandler;
		InputManager.Instance.Interaction.action.started += InteractionInput;
	}
	 
	void Find()
    {
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * xOffset, Screen.height * yOffset, 0));
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
