using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractableObject
{
	public abstract void Interaction(GameObject player);
	public abstract void ShowInfo();
}
 