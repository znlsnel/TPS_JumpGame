using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [Header ("Input System")]
    public InputActionAsset playerInput;
    public InputActionReference mouseMove;
    public InputActionReference move;
    public InputActionReference jump;
    public InputActionReference mouseWheel;

	protected override void Awake()
	{
		base.Awake();

		playerInput.Enable();
	}
}
