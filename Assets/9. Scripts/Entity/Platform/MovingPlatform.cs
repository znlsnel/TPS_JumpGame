using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : Platform
{
	[Header("Moving Platform")]
	[SerializeField] private float speed;

	private Vector3[] positions; 
	private LineRenderer lineRenderer;

	private bool isLoop = false;
	private bool isForward = true;

	private int curIdx = 0;
	private int nextIdx = 1;

	private Vector3 prevPosition;

	private void Awake()
	{
		lineRenderer = GetComponentInParent<LineRenderer>();
		positions = new Vector3[lineRenderer.positionCount];
		lineRenderer.GetPositions(positions);
		transform.position = prevPosition = positions[0];
		isLoop = lineRenderer.loop;
	}

	private void Update()
	{
		MovePlatform();
		MoveObjectOnPlatform();
	}

	private void MoveObjectOnPlatform()
	{
		Vector3 dir = transform.position - prevPosition;
		foreach (var target in targets)
			target.transform.position += dir; 

		prevPosition = transform.position;
	}

	private void MovePlatform()
	{
		Vector3 dir = (positions[nextIdx] - positions[curIdx]).normalized;
		float goalDist = Vector3.Distance(positions[nextIdx], positions[curIdx]);

		transform.position += dir * Time.deltaTime * speed;
		float curDist = Vector3.Distance(transform.position, positions[curIdx]);

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

	protected override void OnPlatformImpact()
	{
		
	}
}