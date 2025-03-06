using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIHandler : MonoBehaviour
{
	private static readonly int gameClear = Animator.StringToHash("GameClear");
	private static readonly int gameOver = Animator.StringToHash("GameOver");
	private static readonly int idle = Animator.StringToHash("Idle");

	private Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void GameClear()
	{
		anim.SetTrigger(gameClear);
	}

	public void GameOver()
	{
		anim.SetTrigger(gameOver);
	}

	public void InitUI()
	{
		anim.SetTrigger(idle);  

	}

}
