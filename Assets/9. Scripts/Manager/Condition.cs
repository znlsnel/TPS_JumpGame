using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum ConditionType
{
	Health = 0,
	Stamina = 1,
}

public class Condition : MonoBehaviour
{
	[Header("Info")]
	[SerializeField] public ConditionType type;
	[SerializeField] private float startValue;
	[SerializeField] private float maxValue;
	[SerializeField] private float passiveValue;
	[SerializeField] private float passiveTimeRate;
	[SerializeField] private Image uiBar;

	[NonSerialized] public float curValue;

	private void Awake()
	{
		curValue = startValue;
		InvokeRepeating(nameof(UpdatePassive), 0, passiveTimeRate);
		
	}

	private void Start()
	{
		GameManager.Instance.PlayerController.statHandler.RegistCondition(this);

	}

	void UpdatePassive()
	{
		Add(passiveValue);
	}



	private void Update()
	{
		uiBar.fillAmount = GetPercentage();
	}

	float GetPercentage()
	{
		return curValue / maxValue;
	}

	public void Add(float value)
	{
		curValue = Mathf.Min(curValue + value, maxValue);
	}

	public void Substract(float value)
	{
		curValue = Mathf.Max(curValue - value, 0f);
	}
}
