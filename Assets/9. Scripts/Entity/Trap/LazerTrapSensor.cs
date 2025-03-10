using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerTrapSensor : TrapSensor
{
	[SerializeField] private Transform laserStart;
	[SerializeField] private Transform laserEnd; 
	[SerializeField] private LayerMask playerMask;

	private float posY;

	private float delayTime = 0.3f;
	private float lastFindTime = 0.0f;

	private void Awake()
	{
		posY = transform.position.y;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(laserStart.position, laserEnd.position);
	}

    void Update()
    {
		Vector3 pos = transform.position;
		pos.y = posY + (Mathf.Sin(Time.time * 1.5f) + 1);
		transform.position = pos;

		if (Time.time - lastFindTime < delayTime)
			return;


		Ray ray = new Ray(laserStart.position, (laserEnd.position - laserStart.position).normalized);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, (laserEnd.position - laserStart.position).magnitude, playerMask))
		{
			lastFindTime = Time.time;
			onFindPlayer?.Invoke(); 
		}
	}
}
