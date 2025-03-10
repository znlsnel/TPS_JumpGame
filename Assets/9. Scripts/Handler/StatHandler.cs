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
	[SerializeField] private float dashCostPerSecond;

	public float JumpPower { get => jumpPower; set => jumpPower = Mathf.Clamp(value, 0, 100); }
	public float MoveSpeed {
		get => IsDashing ? moveSpeed + dashSpeed : moveSpeed;
		set => moveSpeed = Mathf.Clamp(value, 0, 100); 
	}

	bool onPressDashButton = false;
	bool IsDashing => onPressDashButton && conditions[ConditionType.Stamina].curValue >= dashCostPerSecond * Time.deltaTime;
	private InputManager input;

	public event Action onTakeDamage;
	private void Start() 
	{
		input = InputManager.Instance;
		input.Dash.action.started += OnDashInput;
		input.Dash.action.canceled += OnDashInput;
	}

	private void Update()
	{
		float cost = dashCostPerSecond * Time.deltaTime;
		if (onPressDashButton && cost <= conditions[ConditionType.Stamina].curValue)
			conditions[ConditionType.Stamina].Substract(dashCostPerSecond * Time.deltaTime); 
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
		onPressDashButton = context.ReadValue<float>() > 0; 
	}

	public bool IsDie() => conditions[ConditionType.Health].curValue <= 0.0f;


	public void OnDamage(float damage) 
	{
		var health = GetCondition(ConditionType.Health);
		if (health.curValue <= 0.0f)
			return;

		onTakeDamage?.Invoke();
		health.Substract(damage);
		if (health.curValue <= 0.0f)
			GameManager.Instance.GameOver();
	}
}
