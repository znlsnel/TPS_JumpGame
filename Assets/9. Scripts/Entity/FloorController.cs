using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
	[SerializeField] private LayerMask playerLayer;


	private void OnTriggerEnter(Collider other)
	{
		if ((playerLayer.value | (1 << other.gameObject.layer)) == playerLayer.value)
		{
			GameManager.Instance.GameOver();
		}
	}
}
