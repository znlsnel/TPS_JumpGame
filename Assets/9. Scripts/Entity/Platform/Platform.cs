using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
	[SerializeField] private LayerMask layer;
	
	private PlatformController platformController;

	private void Awake()
	{
		platformController = GetComponentInParent<PlatformController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((layer.value | (1 << other.gameObject.layer)) == layer.value)
		{
			platformController.EnterObject(other.gameObject);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if ((layer.value | (1 << other.gameObject.layer)) == layer.value)
		{
			platformController.ExitObject(other.gameObject); 
		} 
	}

}
 