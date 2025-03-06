using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : Singleton<InputManager>
{
    [Header ("Input System")]
    [SerializeField] private InputActionAsset playerInput;
	[SerializeField] private InputActionReference mouseMove;
	[SerializeField] private InputActionReference move;
	[SerializeField] private InputActionReference jump;
	[SerializeField] private InputActionReference mouseWheel;
	[SerializeField] private InputActionReference dash;
	[SerializeField] private InputActionReference interaction;

	public InputActionReference MouseMove => mouseMove;
	public InputActionReference Move => move;
	public InputActionReference Jump => jump;
	public InputActionReference MouseWheel => mouseWheel;
	public InputActionReference Dash => dash;
	public InputActionReference Interaction => interaction;


	protected override void Awake()
	{
		base.Awake();
		playerInput.Enable();

	}
}
