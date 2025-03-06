using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatHandler : MonoBehaviour
{
	[Header("Conditions")]
	Dictionary<ConditionType, Condition> conditions = new Dictionary<ConditionType, Condition>();

	[Header ("Player Stat")]
	[Range(1f, 20f)][SerializeField] private float moveSpeed = 3; 
	[Range(1f, 20f)][SerializeField] private float dashSpeed = 10;
	[Range(1f, 20f)][SerializeField] private float jumpPower = 10;
	[SerializeField] private float dashStaminaCost;
	[SerializeField] private float dashStaminaTimeRate;

	public float JumpPower { get => jumpPower; set => jumpPower = Mathf.Clamp(value, 0, 100); }
	public float MoveSpeed {
		get => onDash != null ? dashSpeed : moveSpeed;
		set => moveSpeed = Mathf.Clamp(value, 0, 100); 
	}

	private InputManager input;
	private Coroutine onDash;

	private void Start() 
	{
		input = InputManager.Instance;
		input.Dash.action.started += OnDashInput;
		input.Dash.action.canceled += OnDashInput;
	}
	 
	public void RegistCondition(Condition condition)
	{
		conditions.Add(condition.type, condition);
	} 

	public Condition GetCondition(ConditionType type)
	{
		if (!conditions.ContainsKey(type))
			return null;

		return conditions[type];
	}
	 
	void OnDashInput(InputAction.CallbackContext context)
	{ 
		var isPressed = context.ReadValue<float>() > 0;
		Condition stamina = conditions[ConditionType.Stamina];

		if (isPressed && stamina.curValue > dashStaminaCost)
		{
			onDash = StartCoroutine(GameManager.Instance.RepeatingAction(() =>
			{
				if (stamina.curValue < dashStaminaCost)
				{
					if (onDash != null)
					{
						StopCoroutine(onDash);
						onDash = null;
					}
					return;
				} 
				conditions[ConditionType.Stamina].Substract(dashStaminaCost);
			}, dashStaminaTimeRate));
		}
		 
		else if (onDash != null)
		{
			StopCoroutine(onDash);
			onDash = null;
		}
	}
}
