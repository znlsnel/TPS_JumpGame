using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, interactableObject
{
	[SerializeField] public ItemData data;

	public void Interaction(GameObject player)
	{
		player.GetComponent<PlayerDataHandler>()?.PickupItem(this);
	}

	public void ShowInfo()
	{
		UIHandler.Instance.InteractionUI.ShowObjectInfo(data.name, data.description, data.GetStatDescription());
	}
}
