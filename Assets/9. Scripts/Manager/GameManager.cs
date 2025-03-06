using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class GameManager : Singleton<GameManager>
{
	[Header("Game Settings")]
	public Transform StartPoint;
	public float yGameOverLimit;
	public float respawnTime;

	private GameUIHandler gameUIHandler;
	private PlayerController player; 
	private bool isGameOver = false; 
	public bool IsGameOver => isGameOver;
	public PlayerController PlayerController => player;
	protected override void Awake() 
	{
		base.Awake();
		gameUIHandler = FindFirstObjectByType<GameUIHandler>(); 
		player = FindFirstObjectByType<PlayerController>();
		InvokeRepeating(nameof(CheckGameOver), 0, 0.1f);
	}

	public void Clear()
	{
		gameUIHandler.GameClear();
	}
	void CheckGameOver()
	{
		if (isGameOver)
			return; 

		if (player.transform.position.y < yGameOverLimit)
		{
			isGameOver = true;
			gameUIHandler?.GameOver();
			Invoke(nameof(RespawnPlayer), respawnTime);
		}
	} 
	void RespawnPlayer()
	{
		gameUIHandler?.InitUI();
		player.transform.position = StartPoint.position;
		isGameOver = false;
	}


	public IEnumerator RepeatingAction(Action action, float timeRate)
	{
		while (true)
		{
			action?.Invoke();
			yield return new WaitForSeconds(timeRate);
		}
	}

	public IEnumerator SetTimer(Action action, float time)
	{
		yield return new WaitForSeconds(time);
		action?.Invoke();
	}
}
 