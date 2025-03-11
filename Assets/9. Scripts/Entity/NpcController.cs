using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NpcState
{
    Idle,
    Move,
}

public class NpcController : MonoBehaviour, IInteractableObject
{
	private static readonly int Greeting = Animator.StringToHash("Greeting");
	private static readonly int Throw = Animator.StringToHash("Throw");
	private static readonly int Dance = Animator.StringToHash("Dance");
	private static readonly int Move = Animator.StringToHash("Move");

	[Header("Npc Info")]
	[SerializeField] private string npcName;
	[SerializeField, TextArea(3, 10)] private string npcDescription;
	[SerializeField] DialogNodeSO dialog;
	[SerializeField] DialogNodeSO successDialog;
	[SerializeField] DialogNodeSO failedDialog;

	[Header("Stats")]
	[SerializeField] private float moveSpeed;

	[Header("Wandering")]
	public float minWanderDistance;
	public float maxWanderDistance;
	public float minWanderWaitTime;
	public float maxWanderWaitTime;

	private NavMeshAgent agent;
    private Animator anim;
	private NpcState aiState = NpcState.Idle;

	void Start()
    {
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		Invoke(nameof(WanderToNewLocation), Random.Range(minWanderWaitTime, maxWanderWaitTime));
	}
	 
	private void Update()
	{
		if (aiState == NpcState.Move)
			MoveUpdate();
	}

	void SetState(NpcState state)
	{
		aiState = state;
		anim.SetBool(Move, aiState == NpcState.Move);
		agent.isStopped = aiState != NpcState.Move;

		switch (aiState)
		{
			case NpcState.Idle:
				anim.SetBool(Dance, Random.Range(0, 2) == 0);
				break;
			case NpcState.Move:
				anim.SetBool(Dance, false);
				break; 
		}
	}
	 
	void MoveUpdate()
	{ 
		if (agent.remainingDistance < 0.1f)
		{
			SetState(NpcState.Idle);
			Invoke(nameof(WanderToNewLocation), Random.Range(minWanderWaitTime, maxWanderWaitTime));
		}
	} 

	void WanderToNewLocation()
	{
		SetState(NpcState.Move); 
		agent.SetDestination(GetWanderLocation());
	}

	Vector3 GetWanderLocation()
	{ 
		NavMeshHit hit;
		NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderWaitTime)),
			out hit, maxWanderDistance, NavMesh.AllAreas);

		return hit.position; 
	}

	public void Interaction(GameObject player)
	{
		UIHandler.Instance.DialogUI.OpenUI(npcName, dialog, () => CheckCoin());
	}

	public void ShowInfo()
	{
		UIHandler.Instance.InteractionUI.ShowObjectInfo(npcName, npcDescription);
	}

	public void CheckCoin()
	{
		int coin = GameManager.Instance.PlayerController.PlayerDataHandler.Coin;
		if (coin > 0)
		{
			GameManager.Instance.PlayerController.PlayerDataHandler.Coin -= 1;
			UIHandler.Instance.DialogUI.OpenUI(npcName, successDialog, ()=>SpawnItem());
			
		}
        else
        {
			UIHandler.Instance.DialogUI.OpenUI(npcName, failedDialog);
		}
	}

	public void SpawnItem()
	{
		GameObject item  = GameManager.Instance.GetRandomItem();
		var go = Instantiate(item);
		var player = GameManager.Instance.PlayerController;
		go.transform.position = player.transform.position + player.transform.forward * 0.5f;
		 
	}

}
