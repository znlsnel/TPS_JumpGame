using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, interactableObject
{
	[SerializeField] public ItemData data;
	private BoxCollider myCollider;

	float posY;
	private void Awake()
	{
		myCollider = GetComponent<BoxCollider>();
		posY = gameObject.transform.position.y;
	}

	private void Start()
	{
	}
	public void Interaction(GameObject player)
	{
		player.GetComponent<PlayerDataHandler>()?.PickupItem(this);
	}

	public void ShowInfo()
	{
		UIHandler.Instance.InteractionUI.ShowObjectInfo(data.name, data.description, data.GetStatDescription());
	}

	public void SetActiveItem(bool active)
	{
		myCollider.enabled = active;
	}

	private void Update()
	{
		if (!myCollider.enabled)
			return;

		Vector3 pos = transform.position;
		pos.y = posY + ((Mathf.Sin(Time.time) + 1) / 2);   
		transform.position = pos;
	}

	public void UseConsumableItem()
	{
		gameObject.SetActive(false);
	}

	public void InitItem()
	{
		gameObject.SetActive(true);

	}
}
