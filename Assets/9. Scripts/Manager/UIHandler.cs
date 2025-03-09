using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : Singleton<UIHandler>
{
	[SerializeField] GameObject dialogUIPrefab;
	[SerializeField] GameObject conditionUIPrefab;
	[SerializeField] GameObject interactionUIPrefab;

	private InteractionUI interactionUI;
	private DialogUI dialogUI;

	public InteractionUI InteractionUI => interactionUI;
	public DialogUI DialogUI => dialogUI;

	protected override void Awake()
	{
		base.Awake();
		dialogUI = Instantiate(dialogUIPrefab).GetComponent<DialogUI>();
		Instantiate<GameObject>(conditionUIPrefab);
		interactionUI = Instantiate<GameObject>(interactionUIPrefab).GetComponent<InteractionUI>();
	}
}
