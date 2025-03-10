using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Trap : MonoBehaviour
{
    private List<TrapSensor> sensors = new List<TrapSensor>();
	protected GameObject player;

	private void Awake()
	{
		var list = transform.GetComponentsInChildren<TrapSensor>();
		foreach (var s in list)
		{
			s.onFindPlayer.AddListener(TrapOn);
			sensors.Add(s);
		}
	}
	private void Start()
	{ 
		player = GameManager.Instance.PlayerController.gameObject;
	}

	protected abstract void TrapOn();
}
