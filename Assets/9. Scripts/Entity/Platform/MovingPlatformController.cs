using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : PlatformController
{
	[Header("Platform Info")]
	[SerializeField] protected float speed;
	[SerializeField] private bool isOneWay;
	protected Vector3[] positions;
	private Vector3 prevPosition;
	private LineRenderer lineRenderer;

	private bool isLoop = false;
	private bool isForward = true;
	private int curIdx = 0;
	private int nextIdx = 1;


	protected override void Awake() 
	{
		base.Awake();
		lineRenderer = GetComponent<LineRenderer>();
		positions = new Vector3[lineRenderer.positionCount];
		lineRenderer.GetPositions(positions); 
		LocalToWorld(positions);  

		platform.transform.position = prevPosition = positions[0];
		isLoop = lineRenderer.loop;
	}
	protected virtual void FixedUpdate()
	{
		if (isOneWay && curIdx > nextIdx)
			return;

		MovePlatform();
		MoveObjectOnPlatform();
	}

	private void MoveObjectOnPlatform()
	{
		Vector3 dir = platform.transform.position - prevPosition;
		foreach (var target in targets)
		{
			var rigid = target.GetComponent<Rigidbody>();
			if (rigid != null)
			{
				rigid.MovePosition(target.transform.position + dir);
			}
			else
				target.transform.position += dir;
		}

		prevPosition = platform.transform.position;
	}

	protected virtual void MovePlatform()
	{
		Vector3 dir = (positions[nextIdx] - positions[curIdx]).normalized;
		float goalDist = Vector3.Distance(positions[nextIdx], positions[curIdx]);

		platform.transform.position += dir * Time.fixedDeltaTime * speed;
		float curDist = Vector3.Distance(platform.transform.position, positions[curIdx]);

		if (goalDist <= curDist)
		{
			curIdx = nextIdx;

			if (isLoop)
			{
				nextIdx = (nextIdx + 1) % positions.Length;
			}
			else
			{
				nextIdx = isForward ? nextIdx + 1 : nextIdx - 1;

				if (nextIdx == positions.Length || nextIdx < 0)
				{
					isForward = !isForward;
					nextIdx = Mathf.Clamp(nextIdx, 1, positions.Length - 2);
				}
			}
		}
	}

	private void LocalToWorld(Vector3[] positions)
	{
		for (int i = 0; i < positions.Length; i++)
			positions[i] += transform.position;
		
	} 

	public void InitPlatform()
	{
		platform.transform.position = positions[0];
		platform.transform.eulerAngles = Vector3.zero;
	}
}
