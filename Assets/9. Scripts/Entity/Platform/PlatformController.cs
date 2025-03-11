using Cinemachine.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlatformController : MonoBehaviour
{
	protected HashSet<GameObject> targets = new HashSet<GameObject>();
	protected GameObject platform;

	protected virtual void Awake()
	{
		platform = GetComponentInChildren<Platform>().gameObject;

	}
	public virtual void EnterObject(GameObject go) => targets.Add(go);
	public virtual void ExitObject(GameObject go)
	{
		if (targets.Contains(go))
			targets.Remove(go);
	}
}  