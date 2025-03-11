using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
	List<GameObject> itemPrefabs = new List<GameObject>();
	List<Item> consumableItems = new List<Item>();
	protected override void Awake() 
	{
		base.Awake();
		Util.ItemInitialize();
		gameUIHandler = FindFirstObjectByType<GameUIHandler>(); 
		player = FindFirstObjectByType<PlayerController>();
		itemPrefabs = Util.GetItemPrefabs();

		Item[] myItem = FindObjectsByType<Item>(FindObjectsSortMode.None);

		foreach (Item item in myItem)
			if (item.data.type == EItemType.Consumable)
				consumableItems.Add(item);	
	}
	 
	public void GameClear()
	{
		gameUIHandler.GameClear();
	}

	public void GameOver()
	{
		if (isGameOver)
			return; 

		isGameOver = true;
		gameUIHandler?.GameOver();
		Invoke(nameof(RespawnPlayer), respawnTime);
	}  
	 
	void RespawnPlayer()
	{ 
		gameUIHandler?.InitUI();
		player.transform.position = StartPoint.position;
		player.StatHandler.Heal(100000);
		isGameOver = false; 

		foreach (Item item in consumableItems)
			item.InitItem();
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


	public GameObject GetRandomItem()
	{
		return itemPrefabs[Random.Range(0, itemPrefabs.Count)];
	}
}
 