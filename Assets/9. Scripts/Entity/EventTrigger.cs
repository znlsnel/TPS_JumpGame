using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
	public UnityEvent onTrigger;
	public LayerMask layer;

	private void OnTriggerEnter(Collider other)
	{
		if ((layer.value | (1<<  other.gameObject.layer)) == layer.value)
			onTrigger?.Invoke();
	}
}
