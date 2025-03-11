using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI coinText;
    PlayerDataHandler playerDataHandler;
	private void Start()
	{
		playerDataHandler = GameManager.Instance.PlayerController.PlayerDataHandler;
		playerDataHandler.onUpdateCoin += UpdateCoinText;
		UpdateCoinText();
	}

	void UpdateCoinText()
	{
		coinText.text = playerDataHandler.Coin.ToString();
	}
}
 