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
	[SerializeField] private float passiveValuePerSecond;
	[SerializeField] private Image uiBar;

	[NonSerialized] public float curValue;

	private void Start()
	{
		curValue = startValue;
		GameManager.Instance.PlayerController.StatHandler.RegistCondition(this);
	}





	private void Update()
	{
		uiBar.fillAmount = GetPercentage();
		Add(passiveValuePerSecond * Time.deltaTime); 
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
