using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHandler : MonoBehaviour
{
    [SerializeField] GameObject conditionUIPrefab;
	[SerializeField] GameObject interactionUIPrefab;

	private InteractionUI interactionUI;
	public InteractionUI InteractionUI => interactionUI;

	private void Awake()
	{
		Instantiate<GameObject>(conditionUIPrefab);
		interactionUI = Instantiate<GameObject>(interactionUIPrefab).GetComponent<InteractionUI>();
	} 
}
