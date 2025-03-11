using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : Singleton<UIHandler>
{
	[SerializeField] GameObject dialogUIPrefab;
	[SerializeField] GameObject conditionUIPrefab;
	[SerializeField] GameObject interactionUIPrefab;
	[SerializeField] GameObject GaugeUIPrefab;

	private InteractionUI interactionUI;
	private GaugeUI gaugeUI;
	private DialogUI dialogUI;

	public InteractionUI InteractionUI => interactionUI;
	public DialogUI DialogUI => dialogUI;
	public GaugeUI GaugeUI => gaugeUI;	

	protected override void Awake()
	{
		base.Awake();

		Instantiate<GameObject>(conditionUIPrefab);
		dialogUI = Instantiate(dialogUIPrefab).GetComponent<DialogUI>();
		interactionUI = Instantiate<GameObject>(interactionUIPrefab).GetComponent<InteractionUI>();
		gaugeUI = Instantiate<GameObject>(GaugeUIPrefab).GetComponent<GaugeUI>();
	}
} 
