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
	 
	private InteractionUI interactionUI;
	private RectTransform aim;
	private interactableObject selectObject;


	private void Start()
	{
		InvokeRepeating(nameof(Find), 0, 0.1f);
		interactionUI = UIHandler.Instance.InteractionUI;

		 
	aim = Instantiate<GameObject>(aimUIPrefab).transform.GetChild(0).GetComponent<RectTransform>();
		if (aim != null)
		{
			var pos = new Vector3(Screen.width * xOffset, Screen.height * yOffset);
			//var worldPos = Camera.main.ScreenToWorldPoint(pos);
			aim.localPosition = aim.parent.InverseTransformPoint(pos); 
		} 

		InputManager.Instance.Interaction.action.started += InteractionInput;
	}
	  
	void Find()
    {
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * xOffset, Screen.height * yOffset, 0));
		RaycastHit hit;

		bool found = false;
		if (Physics.Raycast(ray, out hit, interactionDistance, layer))
		{
			var obj = hit.collider.gameObject.GetComponent<interactableObject>();
			if (obj != selectObject)
			{
				obj.ShowInfo();
				selectObject = obj;
			}
			found = obj != null;
		}

		if (!found && selectObject != null)
		{
			interactionUI.Init();
			selectObject = null;
		}
	} 


	void InteractionInput(InputAction.CallbackContext context)
	{
		if (selectObject == null)
			return;

		selectObject.Interaction(gameObject); 
	}
}
