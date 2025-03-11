using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoDisplay : MonoBehaviour, IInteractableObject
{
	[SerializeField] private string objectName;
	[SerializeField] private string description;

	public void Interaction(GameObject player)
	{
		
	}

	public void ShowInfo() 
	{
		UIHandler.Instance.InteractionUI.ShowObjectInfo(objectName, description);
	}
}
