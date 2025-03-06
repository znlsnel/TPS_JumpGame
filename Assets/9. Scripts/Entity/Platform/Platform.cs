using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Platform : MonoBehaviour
{
	[SerializeField] private LayerMask layer;

	protected HashSet<GameObject> targets = new HashSet<GameObject>();
	private void OnTriggerEnter(Collider other)
	{
		if ((layer.value | (1 << other.gameObject.layer)) == layer.value)
		{
			targets.Add(other.gameObject);
			OnPlatformImpact();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if ((layer.value | (1 << other.gameObject.layer)) == layer.value)
		{
			targets.Remove(other.gameObject);
		} 
	}

	protected abstract void OnPlatformImpact();
}
 